using Infrastructure.Interfaces;
using Infrastructure.Model;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class OrderValidator : IOrderValidator
    {
        private double _referencePrice;

        public void SetReferencePrice(double referencePrice)
        {
            _referencePrice = referencePrice;
        }

        public bool Validate(Order order, out string reason)
        {
            reason = string.Empty;

            if (order == null)
            {
                reason = "Order is null.";
                return false;
            }

            if (!ValidatePrice(order, out string priceReason))
            {
                reason = priceReason;
                return false;
            }
            if (!ValidateShippingAddress(order, out string addressReason))
            {
                reason = addressReason;
                return false;
            }
            if (!ValidateEmail(order, out string emailReason))
            {
                reason = emailReason;
                return false;
            }

            return true;
        }
        private bool ValidatePrice(Order order, out string reason)
        {
            reason = string.Empty;

            if (order.PriceWithoutTax <= 0)
            {
                reason = "PriceWithoutTax is not valid.";
                return false;
            }

            double tolerance = _referencePrice * 0.02;
            if (order.PriceWithoutTax > _referencePrice + tolerance || order.PriceWithoutTax < _referencePrice - tolerance)
            {
                reason = $"Price exceeds tolerance range. Reference price: {_referencePrice}, Tolerance: {tolerance}, Order price: {order.PriceWithoutTax}";
                return false;
            }

            return true;
        }
        public bool ValidateShippingAddress(Order order, out string reason)
        {
            reason = string.Empty;

            if (string.IsNullOrWhiteSpace(order.ShippingAddress1) ||
                string.IsNullOrWhiteSpace(order.ShippingCity) ||
                string.IsNullOrWhiteSpace(order.ShippingCountry) ||
                order.ShippingZipCode <= 0)
            {
                reason = "Shipping address is not valid.";
                return false;
            }

            return true;
        }
        public bool ValidateEmail(Order order, out string reason)
        {
            reason = string.Empty;

            if (string.IsNullOrWhiteSpace(order.Email))
            {
                reason = "Email is not valid.";
                return false;
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(order.Email, emailPattern))
            {
                reason = "Email format is not valid.";
                return false;
            }

            return true;
        }
    }
}