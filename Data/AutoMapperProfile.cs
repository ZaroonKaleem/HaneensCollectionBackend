using AutoMapper;
using ECommerce.DTOs;
using ECommerce.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerce
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Order, OrderResponseDto>();
            CreateMap<OrderItem, OrderItemDto>();
        }
    }
}