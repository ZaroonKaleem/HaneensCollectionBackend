using System;
using System.Collections.Generic;

namespace ECommerce.DTOs
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}