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


//using AutoMapper;
//using ECommerce.DTOs;
//using ECommerce.Models;
//using HaneensCollection.DTOs;
//using HaneensCollection.Models;

//namespace ECommerce
//{
//    public class AutoMapperProfile : Profile
//    {
//        public AutoMapperProfile()
//        {
//            // Existing mappings
//            CreateMap<Order, OrderResponseDto>();
//            CreateMap<OrderItem, OrderItemDto>();

//            // Shared Product mappings
//            CreateMap<Product, ProductDto>().ReverseMap();

//            // Shirt, Dupatta, and Trouser details
//            CreateMap<ShirtDetails, ShirtDetailsDto>().ReverseMap();
//            CreateMap<DupattaDetails, DupattaDetailsDto>().ReverseMap();
//            CreateMap<TrouserDetails, TrouserDetailsDto>().ReverseMap();

//            // Stitched and Unstitched suits
//            CreateMap<StitchedSuit, StitchedSuitDto>().ReverseMap();
//            CreateMap<UnstitchedSuit, UnstitchedSuitDto>().ReverseMap();

//            // Pret and Luxury suits
//            CreateMap<PretSuit, PretSuitDto>().ReverseMap();
//            CreateMap<LuxurySuit, LuxurySuitDto>().ReverseMap();
//        }
//    }
//}
