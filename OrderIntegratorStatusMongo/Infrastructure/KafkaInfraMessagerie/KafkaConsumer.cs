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
    public class KafkaConsumer : BackgroundService
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly ConsumerConfig _config;
        private readonly string? _topic;
        private readonly IOrderRepository _orderRepository;
        private readonly KafkaProducer _kafkaProducer;
        private readonly ConcurrentDictionary<string, bool> _processedMessages;

        public KafkaConsumer(IOptions<KafkaConfig> kafkaSettings, ILogger<KafkaConsumer> logger, IOrderRepository orderRepository, KafkaProducer kafkaProducer)
        {
            _logger = logger;
            _config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = "order-integrator-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _topic = kafkaSettings.Value.ConsumerTopic;
            _orderRepository = orderRepository;
            _kafkaProducer = kafkaProducer;
            _processedMessages = new ConcurrentDictionary<string, bool>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => ConsumeMessages(stoppingToken));
        }

        private void ConsumeMessages(CancellationToken stoppingToken)
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

                        dynamic messageObject = JsonSerializer.Deserialize<Dictionary<string, string>>(consumeResult.Message.Value);
                        string orderId = messageObject["Id"];

                        if (!_processedMessages.ContainsKey(orderId))
                        {
                            UpdateOrderStatusFromMessage(consumeResult.Message.Value).Wait();
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

        private async Task UpdateOrderStatusFromMessage(string message)
        {
            try
            {
                var messageObject = JsonSerializer.Deserialize<Dictionary<string, string>>(message);

                if (messageObject.ContainsKey("Id") && messageObject.ContainsKey("Message"))
                {
                    var orderId = messageObject["Id"];
                    var orderStatusMessage = messageObject["Message"];

                    if (Enum.TryParse<OrderStatus>(orderStatusMessage, out var orderStatus))
                    {
                        var order = await _orderRepository.GetOrderAsync(orderId);

                        if (order != null && order.OrderStatus != orderStatus)
                        {
                            var previousStatus = order.OrderStatus;

                            // Publish initial status
                            await PublishInitialStatusAsync(orderId, previousStatus);

                            // Update the order status
                            await _orderRepository.UpdateOrderStatus(orderId, orderStatus);

                            // Publish new status
                            await PublishNewStatusAsync(orderId, orderStatus);
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Invalid order status message received: {orderStatusMessage}");
                    }
                }
                else
                {
                    _logger.LogWarning($"Invalid message received: {message}");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error deserializing message: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order status: {ex.Message}");
            }
        }

        private async Task PublishInitialStatusAsync(string orderId, OrderStatus previousStatus)
        {
            var messageObject = new
            {
                Id = orderId,
                PreviousOrderStatus = previousStatus.ToString()
            };
            await _kafkaProducer.ProduceAsync(messageObject);
            _logger.LogInformation($"Initial status message sent to orders-status: {messageObject}");
        }

        private async Task PublishNewStatusAsync(string orderId, OrderStatus newStatus)
        {
            var messageObject = new
            {
                Id = orderId,
                NewOrderStatus = newStatus.ToString()
            };
            await _kafkaProducer.ProduceAsync(messageObject);
            _logger.LogInformation($"New status message sent to orders-status: {messageObject}");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
