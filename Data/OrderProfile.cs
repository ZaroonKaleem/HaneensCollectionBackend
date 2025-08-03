// OrderProfile.cs
using AutoMapper;
using ECommerce.DTOs;
using ECommerce.Models;

namespace ECommerce.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(_ => Guid.NewGuid().ToString().Substring(0, 8).ToUpper()));

            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}