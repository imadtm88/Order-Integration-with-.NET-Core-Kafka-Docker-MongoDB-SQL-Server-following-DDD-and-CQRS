using Confluent.Kafka;
using Infrastructure.KafkaInfraMessagerie.Config;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Infrastructure.KafkaInfraMessagerie
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly string? _topic;
        private readonly IOrderPaymentHttp _orderPaymentHttp;
        private readonly IOrderPaymentValidator _orderPaymentValidator;
        private readonly KafkaProducer _kafkaProducer;

        public KafkaConsumer(ILogger<KafkaConsumer> logger, IOptions<KafkaConfig> kafkaConfigOptions, IOrderPaymentHttp orderPaymentHttp, IOrderPaymentValidator orderPaymentValidator, KafkaProducer kafkaProducer)
        {
            _logger = logger;
            var config = kafkaConfigOptions.Value;
            _consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig
            {
                BootstrapServers = config.BootstrapServers,
                GroupId = "order-payment-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            }).Build();
            _topic = config.Topic;
            _orderPaymentHttp = orderPaymentHttp;
            _orderPaymentValidator = orderPaymentValidator;
            _kafkaProducer = kafkaProducer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = _consumer.Consume(stoppingToken);
                        var message = JObject.Parse(cr.Message.Value);

                        if (message["NewOrderStatus"] != null)
                        {
                            _logger.LogInformation($"Message received: {cr.Message.Value}");

                            var orderId = message["Id"]?.ToString();
                            if (!string.IsNullOrEmpty(orderId))
                            {
                                // Fetch the order details from OrderIntegratorStatusMongo
                                var order = await _orderPaymentHttp.GetOrderAsync(orderId);
                                if (order != null)
                                {
                                    string validationMessage;
                                    if (_orderPaymentValidator.IsValid(order, out validationMessage))
                                    {
                                        _logger.LogInformation($"Order is valid: {JsonConvert.SerializeObject(order)}");
                                        var successMessage = new { Id = orderId, Message = "PAYMENTSUCCESS" };
                                        await _kafkaProducer.SendMessageAsync(successMessage);
                                    }
                                    else
                                    {
                                        _logger.LogWarning($"Order with Id {orderId} is not valid. Reason: {validationMessage}");
                                        var failureMessage = new { Id = orderId, Message = "PAYMENTFAILURE" };
                                        await _kafkaProducer.SendMessageAsync(failureMessage);
                                    }
                                }
                                else
                                {
                                    _logger.LogWarning($"Order with Id {orderId} not found.");
                                }
                            }
                            else
                            {
                                _logger.LogWarning("Order ID is missing in the message.");
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Consume error: {e.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Unexpected error: {ex.Message}");
                    }
                }
            }, stoppingToken);
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}