using ECommerce.DTOs;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto orderDto);
        Task<OrderResponseDto> GetOrderByIdAsync(int id);
    }
}