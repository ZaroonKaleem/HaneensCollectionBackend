using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var order = new Order
            {
                CustomerName = orderDto.CustomerName,
                CustomerEmail = orderDto.CustomerEmail,
                CustomerPhone = orderDto.CustomerPhone,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = orderDto.Subtotal,
                ShippingCost = orderDto.ShippingCost,
                Total = orderDto.Total,
                OrderItems = orderDto.Items?.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl
                }).ToList() ?? new List<OrderItem>()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return null;

            return _mapper.Map<OrderResponseDto>(order);
        }
    }
}