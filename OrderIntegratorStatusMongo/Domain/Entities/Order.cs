using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class Order
    {
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
        public int Quantity { get; set; }

        [BsonRequired]
        public int SellerID { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonRequired]
        public string? OfferID { get; set; }
    }
}   