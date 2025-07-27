using ECommerce.DTOs;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                if (orderDto == null)
                {
                    return BadRequest("Order data is null");
                }

                if (orderDto.Items == null || !orderDto.Items.Any())
                {
                    return BadRequest("Order items are null or empty");
                }

                // Validate individual items
                foreach (var item in orderDto.Items)
                {
                    if (item.Quantity <= 0)
                    {
                        return BadRequest($"Quantity must be greater than 0 for product {item.ProductId}");
                    }
                    if (item.Price <= 0)
                    {
                        return BadRequest($"Price must be greater than 0 for product {item.ProductId}");
                    }
                }

                var createdOrder = await _orderService.CreateOrderAsync(orderDto);

                return CreatedAtAction(
                    nameof(GetOrder),
                    new { id = createdOrder.Id },
                    createdOrder);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your order");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return order;
        }
    }
}