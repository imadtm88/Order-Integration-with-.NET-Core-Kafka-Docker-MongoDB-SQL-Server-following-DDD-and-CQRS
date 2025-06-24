using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.KafkaInfraMessagerie;
using Infrastructure.Model;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderIntegratorDbContext _context;
        private readonly ILogger<OrderRepository> _logger;
        private readonly KafkaProducer _kafkaProducer;

        public OrderRepository(OrderIntegratorDbContext context, ILogger<OrderRepository> logger, KafkaProducer kafkaProducer)
        {
            _context = context;
            _logger = logger;
            _kafkaProducer = kafkaProducer;
        }

        public async Task SaveOrderAsync(Order order)
        {
            try
            {
                var orderWithStatus = new OrderModel
                {
                    Id = order.Id,
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
                    PriceWithTax = order.PriceWithTax,
                    Quantity = order.Quantity,
                    SellerID = order.SellerID,
                    OfferID = order.OfferID,
                    OrderStatus = "ACCEPTED"
                };

                _context.OrderModel.Add(orderWithStatus);
                await _context.SaveChangesAsync();

                // Envoi du message Kafka pour l'intégration réussie
                await _kafkaProducer.ProduceMessageAsync(order.Id, "INTEGRATESUCCESS");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving order: {ex.Message}");

                // Envoi du message Kafka pour l'intégration échouée
                await _kafkaProducer.ProduceMessageAsync(order.Id, "INTEGRATEFAILURE");

                throw;
            }
        }
    }
}
