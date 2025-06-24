using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Order
    {
        [Key]
        public string? Id { get; set; }
        public int CustomerOrderRef { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CustomersReference { get; set; }
        public string? CustomerNumber { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? ShippingAddress1 { get; set; }
        public int ShippingZipCode { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingCountry { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ProductId { get; set; }
        public double PriceWithoutTax { get; set; }
        public double PriceWithTax { get; set; }
        public int Quantity { get; set; }
        public int SellerID { get; set; }
        public string? OfferID { get; set; }
    }
}