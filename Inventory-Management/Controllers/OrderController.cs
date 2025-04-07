using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory_Management.Model;
using Inventory_Management.Context;
using Inventory_Management.DTO_S;
using Inventory_Management.DTOs;
using Inventory_Management.Managers;
using Microsoft.AspNetCore.Authorization;

namespace Inventory_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderManager _orderManager;

        public OrderController(OrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderManager.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            await _orderManager.DeleteOrderAsync(id);

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderManager.GetAllOrdersAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found");
            }

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
           var order = await _orderManager.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            try
            {
                var order = new Order
                {
                    CustomerId = orderDto.CustomerId,
                    OrderDate = DateTime.Now,
                    PaymentMethod = orderDto.PaymentMethod,
                    OrderItems = orderDto.OrderItems.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    }).ToList()
                };
                var createdOrder = await _orderManager.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating order: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult<Order>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDTO updateDto)
        {
            try 
            {
                var updatedOrder = await _orderManager.UpdateOrderStatusAsync(id, updateDto.NewStatus);
                
                if (updatedOrder == null)
                {
                    return NotFound($"Order with ID {id} not found");
                }

                return Ok(updatedOrder);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the order status: {ex.Message}");
            }
        }
    } 
}