using Confluent.Kafka;
using Infrastructure.KafkaInfraMessagerie.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Domain.Repositories;
using System.Text.Json;
using Domain.Model.Enum;
using System.Collections.Concurrent;
using Domain.Model.History;

namespace Infrastructure.KafkaInfraMessagerie
{
    public class KafkaPaymentConsumer : BackgroundService
    {
        private readonly ILogger<KafkaPaymentConsumer> _logger;
        private readonly ConsumerConfig _config;
        private readonly string? _topic;
        private readonly IOrderRepository _orderRepository;
        private readonly KafkaProducer _kafkaProducer;
        private readonly ConcurrentDictionary<string, bool> _processedMessages;

        public KafkaPaymentConsumer(IOptions<KafkaConfig> kafkaSettings, ILogger<KafkaPaymentConsumer> logger, IOrderRepository orderRepository, KafkaProducer kafkaProducer)
        {
            _logger = logger;
            _config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = "payment-integrator-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _topic = kafkaSettings.Value.PaymentTopic;
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
                        _logger.LogInformation($"Payment message received: {consumeResult.Message.Value}");

                        dynamic messageObject = JsonSerializer.Deserialize<Dictionary<string, string>>(consumeResult.Message.Value);
                        string orderId = messageObject["Id"];
                        string paymentStatus = messageObject["Message"];

                        if (!_processedMessages.ContainsKey(orderId))
                        {
                            UpdateOrderStatusFromPayment(orderId, paymentStatus).Wait();
                            _processedMessages[orderId] = true;
                        }
                        else
                        {
                            _logger.LogInformation($"Payment message with Id: {orderId} already processed.");
                        }
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Error occurred: {ex.Error.Reason}");
                }
            }
        }

        private async Task UpdateOrderStatusFromPayment(string orderId, string paymentStatus)
        {
            try
            {
                var order = await _orderRepository.GetOrderAsync(orderId);

                if (order != null)
                {
                    OrderStatus newStatus;
                    bool isValidStatus = Enum.TryParse<OrderStatus>(paymentStatus, out newStatus);

                    if (isValidStatus)
                    {
                        var previousStatuses = new List<OrderStatus> { OrderStatus.CONTROLESUCCESS, OrderStatus.CREATED };

                        // Add current statuses to history
                        foreach (var prevStatus in previousStatuses)
                        {
                            order.OrderHistoryStatus.Add(new OrderHistoryStatus
                            {
                                PreviousStatus = prevStatus,
                                LastUpdated = DateTime.UtcNow
                            });
                        }

                        // Update main status
                        order.OrderStatus = newStatus;

                        // Update order in database
                        await _orderRepository.UpdateOrderAsync(order);

                        // Log and produce Kafka message for new status
                        _logger.LogInformation($"Order {orderId} status updated from {string.Join(", ", previousStatuses)} to {newStatus}");
                        await PublishNewStatusAsync(orderId, newStatus);
                    }
                    else
                    {
                        _logger.LogWarning($"Invalid payment status message received: {paymentStatus}");
                    }
                }
                else
                {
                    _logger.LogWarning($"Order with Id {orderId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order status from payment: {ex.Message}");
            }
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
