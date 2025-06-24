using Domain.Entities;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Model.History;
using Domain.Repositories;
using Infrastructure.KafkaInfraMessagerie;
using Microsoft.Extensions.Logging;

namespace Application.Command.Services
{
    public class OrderServiceApplication
    {
        private readonly IOrderRepository _orderRepository;
        private readonly KafkaProducer _kafkaProducer;
        private readonly ILogger<OrderServiceApplication> _logger;

        public OrderServiceApplication(IOrderRepository orderRepository, KafkaProducer kafkaProducer, ILogger<OrderServiceApplication> logger)
        {
            _orderRepository = orderRepository;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task CreateOrderAsync(Order order)
        {
            var orderModel = MapToOrderModel(order);
            //orderModel.OrderHistoryStatus.Add(new OrderHistoryStatus
            //{
            //    PreviousStatus = OrderStatus.CREATED,
            //    LastUpdated = DateTime.UtcNow
            //});
            await _orderRepository.AddOrderAsync(orderModel);
            await PublishOrderStatusAsync(orderModel.Id, orderModel.OrderStatus);
        }

        public async Task CreateOrdersAsync(List<Order> orders)
        {
            foreach (var order in orders)
            {
                await CreateOrderAsync(order);
            }
        }

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<OrderModel> GetOrderAsync(string Id)
        {
            return await _orderRepository.GetOrderAsync(Id);
        }

        public async Task UpdateOrderStatusAsync(string orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetOrderAsync(orderId);

            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            _logger.LogInformation($"Updating order {orderId} status from {order.OrderStatus} to {newStatus}");

            var previousStatus = order.OrderStatus;

            // Check if "CREATED" already exists in history
            var createdExists = order.OrderHistoryStatus.Any(s => s.PreviousStatus == OrderStatus.CREATED);

            if (previousStatus != newStatus)
            {
                // Add "CREATED" to history if it's the first status change
                if (!createdExists && previousStatus == OrderStatus.CREATED)
                {
                    order.OrderHistoryStatus.Add(new OrderHistoryStatus
                    {
                        PreviousStatus = OrderStatus.CREATED,
                        LastUpdated = DateTime.UtcNow
                    });
                }

                order.OrderHistoryStatus.Add(new OrderHistoryStatus
                {
                    PreviousStatus = previousStatus,
                    LastUpdated = DateTime.UtcNow
                });

                // Update order status in the database
                order.OrderStatus = newStatus;
                await _orderRepository.UpdateOrderAsync(order);

                // Publish new status
                await PublishNewStatusAsync(orderId, newStatus);

                _logger.LogInformation($"Order {orderId} status updated to {newStatus}");
            }
            else
            {
                _logger.LogInformation($"Order {orderId} status is already {newStatus}. No update needed.");
            }
        }

        private async Task PublishOrderStatusAsync(string orderId, OrderStatus status)
        {
            var messageObject = new
            {
                Id = orderId,
                OrderStatus = status.ToString()
            };

            _logger.LogInformation($"Publishing status update for order {orderId} with status {status}");

            await _kafkaProducer.ProduceAsync(messageObject);
        }

        private async Task PublishInitialStatusAsync(string orderId, OrderStatus status)
        {
            var messageObject = new
            {
                Id = orderId,
                PreviousOrderStatus = status.ToString()
            };

            _logger.LogInformation($"Initial status message sent to orders-status: {{ Id = {orderId}, PreviousOrderStatus = {status} }}");

            await _kafkaProducer.ProduceAsync(messageObject);
        }

        private async Task PublishNewStatusAsync(string orderId, OrderStatus status)
        {
            var messageObject = new
            {
                Id = orderId,
                NewOrderStatus = status.ToString()
            };

            _logger.LogInformation($"New status message sent to orders-status: {{ Id = {orderId}, NewOrderStatus = {status} }}");

            await _kafkaProducer.ProduceAsync(messageObject);
        }

        private OrderModel MapToOrderModel(Order order)
        {
            return new OrderModel
            {
                Id = Guid.NewGuid().ToString(),
                CustomerOrderRef = order.CustomerOrderRef,
                CreationDate = order.CreationDate,
                CustomersReference = order.CustomersReference,
                CustomerNumber = order.CustomerNumber,
                LastName = order.LastName,
                FirstName = order.FirstName,
                ShippingAddress1 = order.ShippingAddress1,
                ShippingZipCode = order.ShippingZipCode,
                ShippingCity = order.ShippingCity,
                ShippingCountry = order.ShippingCountry,
                PhoneNumber = order.PhoneNumber,
                Email = order.Email,
                ProductId = order.ProductId,
                PriceWithoutTax = order.PriceWithoutTax,
                PriceWithTax = order.PriceWithoutTax * 1.20,
                Quantity = order.Quantity,
                SellerID = order.SellerID,
                OfferID = order.OfferID,
                OrderStatus = OrderStatus.CREATED
            };
        }
    }
}