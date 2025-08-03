using ECommerce.DTOs;
using ECommerce.Models;
using HaneensCollection.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto orderDto)
        {
            try
            {
                // Manual mapping from DTO to Entity
                var order = new Order
                {
                    OrderNumber = GenerateOrderNumber(),
                    OrderDate = DateTime.UtcNow,
                    CustomerName = orderDto.CustomerName,
                    CustomerEmail = orderDto.CustomerEmail,
                    CustomerPhone = orderDto.CustomerPhone,
                    ShippingAddress = orderDto.ShippingAddress,
                    PaymentMethod = "CashOnDelivery",
                    Subtotal = orderDto.Subtotal,
                    ShippingCost = orderDto.ShippingCost,
                    Total = orderDto.Total,
                    Status = "Pending",
                    OrderItems = orderDto.Items.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        ImageUrl = item.ImageUrl
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Manual mapping from Entity to Response DTO
                return new OrderResponseDto
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    CustomerName = order.CustomerName,
                    CustomerEmail = order.CustomerEmail,
                    CustomerPhone = order.CustomerPhone,
                    ShippingAddress = order.ShippingAddress,
                    PaymentMethod = order.PaymentMethod,
                    Subtotal = order.Subtotal,
                    ShippingCost = order.ShippingCost,
                    Total = order.Total,
                    Status = order.Status,
                    Items = order.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        Price = oi.Price,
                        Quantity = oi.Quantity,
                        ImageUrl = oi.ImageUrl
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                throw;
            }
        }

        private string GenerateOrderNumber()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            return new OrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                ShippingAddress = order.ShippingAddress,
                PaymentMethod = order.PaymentMethod,
                Subtotal = order.Subtotal,
                ShippingCost = order.ShippingCost,
                Total = order.Total,
                Status = order.Status,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                    ImageUrl = oi.ImageUrl
                }).ToList()
            };
        }

        public async Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            try
            {
                // Validate the new status
                var validStatuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
                if (!validStatuses.Contains(newStatus))
                {
                    throw new ArgumentException("Invalid order status");
                }

                // Find the order
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    throw new KeyNotFoundException("Order not found");
                }

                // Update the status
                order.Status = newStatus;
                order.OrderDate = DateTime.UtcNow; // Update the last modified date

                // Save changes
                await _context.SaveChangesAsync();

                // Return the updated order
                return new OrderResponseDto
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    CustomerName = order.CustomerName,
                    CustomerEmail = order.CustomerEmail,
                    CustomerPhone = order.CustomerPhone,
                    ShippingAddress = order.ShippingAddress,
                    PaymentMethod = order.PaymentMethod,
                    Subtotal = order.Subtotal,
                    ShippingCost = order.ShippingCost,
                    Total = order.Total,
                    Status = order.Status,
                    Items = order.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        Price = oi.Price,
                        Quantity = oi.Quantity,
                        ImageUrl = oi.ImageUrl
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status");
                throw;
            }
        }
        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            return orders.Select(order => new OrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                ShippingAddress = order.ShippingAddress,
                PaymentMethod = order.PaymentMethod,
                Subtotal = order.Subtotal,
                ShippingCost = order.ShippingCost,
                Total = order.Total,
                Status = order.Status,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                    ImageUrl = oi.ImageUrl
                }).ToList()
            });
        }
    }
}