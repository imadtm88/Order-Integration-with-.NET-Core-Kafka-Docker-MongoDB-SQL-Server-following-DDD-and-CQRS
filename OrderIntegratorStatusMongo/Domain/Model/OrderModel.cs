using Domain.Model.Enum;
using Domain.Model.History;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
    public class OrderModel
    {
        [BsonId]
        [BsonRequired]
        public string? Id { get; set; }

        [BsonRequired]
        public int CustomerOrderRef { get; set; }

        [BsonRequired]
        public DateTime CreationDate { get; set; }

        [BsonRequired]
        public string? CustomersReference { get; set; }

        [BsonRequired]
        public string? CustomerNumber { get; set; }

        [BsonRequired]
        public string? LastName { get; set; }

        [BsonRequired]
        public string? FirstName { get; set; }

        [BsonRequired]
        public string? ShippingAddress1 { get; set; }

        [BsonRequired]
        public int ShippingZipCode { get; set; }

        [BsonRequired]
        public string? ShippingCity { get; set; }

        [BsonRequired]
        public string? ShippingCountry { get; set; }

        [BsonRequired]
        public string? PhoneNumber { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string? Email { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string? ProductId { get; set; }

        [BsonRequired]
        public double PriceWithoutTax { get; set; }

        [BsonRequired]
        public double PriceWithTax { get; set; }

        [BsonRequired]
        public int Quantity { get; set; }

        [BsonRequired]
        public int SellerID { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string? OfferID { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus OrderStatus { get; set; }

        public List<OrderHistoryStatus> OrderHistoryStatus { get; set; } = new List<OrderHistoryStatus>();
    }
}

