using ECommerce.DTOs;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto orderDto);
        Task<OrderResponseDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}