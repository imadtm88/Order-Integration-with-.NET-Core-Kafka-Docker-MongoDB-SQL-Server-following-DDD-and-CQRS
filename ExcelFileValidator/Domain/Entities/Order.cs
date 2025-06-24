using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Order
    {
        [Required]
        public int CustomerOrderRef { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string? CustomersReference { get; set; }
        
        [Required]
        public string? CustomerNumber { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? FirstName { get; set; }


        [Required]
        public string? ShippingAddress1 { get; set; }

        [Required]
        public int ShippingZipCode { get; set; }

        [Required]
        public string? ShippingCity { get; set; }


        [Required]
        public string? ShippingCountry { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? ProductId { get; set; }

        [Required]
        public double PriceWithoutTax { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int SellerID { get; set; }

        public string? OfferID { get; set; }
    }
}