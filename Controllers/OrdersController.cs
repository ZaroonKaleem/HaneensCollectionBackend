using ECommerce.DTOs;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                if (orderDto == null)
                {
                    return BadRequest("Order data is required");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (orderDto.Items == null || !orderDto.Items.Any())
                {
                    return BadRequest("At least one order item is required");
                }

                //var validationErrors = ValidateOrderItems(orderDto.Items);
                //if (validationErrors.Any())
                //{
                    //return BadRequest(new { Errors = validationErrors });
                //}

                var createdOrder = await _orderService.CreateOrderAsync(orderDto);

                return CreatedAtAction(
                    nameof(GetOrder),
                    new { id = createdOrder.Id },
                    createdOrder);
            }
            catch (DbUpdateException ex)
            {
                //_logger.LogError(ex, "Database error while creating order");
                return StatusCode(500, new { Message = "Database error", Details = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Unexpected error creating order");
                return StatusCode(500, new { Message = "An unexpected error occurred", Details = ex.Message });
            }
        }
        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    return BadRequest("Status cannot be empty");
                }

                var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, status);
                return Ok(updatedOrder);
            }
            catch (ArgumentException ex)
            {
                //_logger.LogWarning(ex, "Invalid status update attempt");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                //_logger.LogWarning(ex, "Order not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error updating order status");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the order status");
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