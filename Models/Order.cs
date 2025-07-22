using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OrderNumber { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = "CashOnDelivery";

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}