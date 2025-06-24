using Confluent.Kafka;
using Infrastructure.Interfaces;
using Infrastructure.KafkaInfraMessagerie.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.KafkaInfraMessagerie
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly string? _topic;
        private readonly IOrderIntegrationHttp _orderIntegrationHttp;
        private readonly IServiceProvider _serviceProvider;

        public KafkaConsumer(
            ILogger<KafkaConsumer> logger,
            IOptions<KafkaConfig> kafkaConfigOptions,
            IOrderIntegrationHttp orderIntegrationHttp,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            var config = kafkaConfigOptions.Value;
            _consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig
            {
                BootstrapServers = config.BootstrapServers,
                GroupId = "order-integration-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            }).Build();
            _topic = config.Topic;
            _orderIntegrationHttp = orderIntegrationHttp;
            _serviceProvider = serviceProvider;
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
                        var message = JsonConvert.DeserializeObject<dynamic>(cr.Message.Value);

                        if (message?.NewOrderStatus == "PAYMENTSUCCESS")
                        {
                            _logger.LogInformation($"Message received: {cr.Message.Value}");

                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                                var orderId = (string)message.Id;
                                var order = await _orderIntegrationHttp.GetOrderAsync(orderId);

                                if (order != null)
                                {
                                    await orderRepository.SaveOrderAsync(order);
                                }
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Consume error: {e.Error.Reason}");
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
