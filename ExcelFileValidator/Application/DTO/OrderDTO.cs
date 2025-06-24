using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public int CustomerOrderRef { get; set; }
        public DateTime CreationDate { get; set; }
        public string CustomersReference { get; set; }
        public string CustomerNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ShippingAddress1 { get; set; }
        public int ShippingZipCode { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingCountry { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProductId { get; set; }
        public decimal PriceWithoutTax { get; set; }
        public decimal PriceWithTax { get; set; }
        public int Quantity { get; set; }
        public int SellerID { get; set; }
        public string OfferID { get; set; }
        public string OrderStatus { get; set; }
    }
}
