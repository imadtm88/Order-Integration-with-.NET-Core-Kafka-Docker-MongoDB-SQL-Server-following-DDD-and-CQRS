using Confluent.Kafka;
using Infrastructure.KafkaInfraMessagerie.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Domain.Repositories;
using System.Text.Json;
using Domain.Model.Enum;
using System.Collections.Concurrent;

namespace Infrastructure.KafkaInfraMessagerie
{
    public class KafkaIntegrationConsumer : BackgroundService
    {
        private readonly ILogger<KafkaIntegrationConsumer> _logger;
        private readonly ConsumerConfig _config;
        private readonly string? _integrationTopic;
        private readonly IOrderRepository _orderRepository;
        private readonly ConcurrentDictionary<string, bool> _processedMessages;

        public KafkaIntegrationConsumer(IOptions<KafkaConfig> kafkaSettings, ILogger<KafkaIntegrationConsumer> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = "integration-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _integrationTopic = kafkaSettings.Value.IntegrationTopic;
            _orderRepository = orderRepository;
            _processedMessages = new ConcurrentDictionary<string, bool>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => ConsumeMessages(stoppingToken));
        }

        private void ConsumeMessages(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            consumer.Subscribe(_integrationTopic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    if (consumeResult != null && !string.IsNullOrWhiteSpace(consumeResult.Message.Value))
                    {
                        _logger.LogInformation($"Message received: {consumeResult.Message.Value}");

                        dynamic messageObject = JsonSerializer.Deserialize<Dictionary<string, string>>(consumeResult.Message.Value);
                        string orderId = messageObject["Id"];
                        string message = messageObject["Message"];

                        if (!_processedMessages.ContainsKey(orderId))
                        {
                            UpdateOrderStatus(orderId, message).Wait();
                            _processedMessages[orderId] = true;
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

        private async Task UpdateOrderStatus(string orderId, string message)
        {
            try
            {
                var order = await _orderRepository.GetOrderAsync(orderId);

                if (order != null)
                {
                    var previousStatus = order.OrderStatus;
                    var newStatus = message == "INTEGRATESUCCESS" ? OrderStatus.INTEGRATESUCCESS : OrderStatus.INTEGRATEFAILURE;

                    if (previousStatus != newStatus)
                    {
                        // Update the order history
                        order.OrderHistoryStatus.Add(new Domain.Model.History.OrderHistoryStatus
                        {
                            PreviousStatus = previousStatus,
                            LastUpdated = DateTime.UtcNow
                        });

                        // Update the order status
                        order.OrderStatus = newStatus;
                        await _orderRepository.UpdateOrderAsync(order);

                        _logger.LogInformation($"Order {orderId} status updated to {newStatus}");
                    }
                    else
                    {
                        _logger.LogInformation($"Order {orderId} status is already {newStatus}. No update needed.");
                    }
                }
                else
                {
                    _logger.LogWarning($"Order with Id: {orderId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order status: {ex.Message}");
            }
        }
    }
}
