using Inventory_Management.DTO;
using Inventory_Management.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Inventory_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class InventoryTransactionController : ControllerBase
    {
        private readonly InventoryTransactionManager _manager;

        public InventoryTransactionController(InventoryTransactionManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Only staff can create transactions
        public async Task<IActionResult> CreateTransaction([FromBody] CreateInventoryTransactionDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var transaction = await _manager.CreateTransactionAsync(dto, userId);
                return Ok(transaction);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Staff and admins can view transactions
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _manager.GetTransactionsAsync();
            return Ok(transactions);
        }
    }
} 