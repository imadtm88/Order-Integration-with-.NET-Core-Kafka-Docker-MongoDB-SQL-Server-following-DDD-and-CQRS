using Confluent.Kafka;
using Infrastructure.Interfaces;
using Infrastructure.KafkaInfraMessagerie.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Infrastructure.KafkaInfraMessagerie
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly ConsumerConfig _config;
        private readonly string? _topic;
        private readonly IOrderService _orderService;
        private readonly IOrderValidator _orderValidator;
        private readonly KafkaProducer _kafkaProducer;
        private readonly ConcurrentDictionary<string, bool> _processedMessages;

        public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaConsumer> logger, IOrderService orderService, IOrderValidator orderValidator, KafkaProducer kafkaProducer)
        {
            _logger = logger;
            _config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = "order-validator-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _topic = kafkaSettings.Value.Topic;
            _orderService = orderService;
            _orderValidator = orderValidator;
            _kafkaProducer = kafkaProducer;
            _processedMessages = new ConcurrentDictionary<string, bool>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => ConsumeMessages(stoppingToken));
        }

        private async Task ConsumeMessages(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            consumer.Subscribe(_topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    if (consumeResult != null && !string.IsNullOrWhiteSpace(consumeResult.Message.Value))
                    {
                        _logger.LogInformation($"Message received: {consumeResult.Message.Value}");

                        dynamic orderJson = JsonConvert.DeserializeObject(consumeResult.Message.Value);
                        string orderId = orderJson.Id;

                        if (!_processedMessages.ContainsKey(orderId))
                        {
                            var order = await _orderService.GetOrdersAsync(orderId);

                            if (_orderValidator.Validate(order, out string reason))
                            {
                                string orderString = JsonConvert.SerializeObject(order, Formatting.Indented);
                                _logger.LogInformation($"Object received and validated: {orderString}");
                                await _kafkaProducer.ProduceAsync("CONTROLESUCCESS", orderId);
                                _processedMessages[orderId] = true;
                            }
                            else
                            {
                                _logger.LogInformation($"Object validation failed: {reason}");
                                await _kafkaProducer.ProduceAsync("CONTROLFAILURE", orderId);
                                _processedMessages[orderId] = true;
                            }
                        }
                        else
                        {
                            _logger.LogInformation($"Message with Id: {orderId} already processed.");
                        }
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Error occurred: {ex.Error.Reason}");
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
