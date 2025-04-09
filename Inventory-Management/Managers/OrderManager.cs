using Inventory_Management.Context;
using Inventory_Management.Model;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Managers
{
    public class OrderManager
    {
        private readonly InventoryDbContext _context;

        public OrderManager(InventoryDbContext context)
        {
            _context = context;
        }

        
        public async Task<(List<Order> Orders, int TotalCount)> GetAllOrdersAsync(int pageNumber = 1, int pageSize = 40)
        {
            // Get total count
            int totalCount = await _context.Orders.CountAsync();

            // Fetch paginated orders from the database
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalCount);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Order> UpdateOrderStatusAsync(int id, int newStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return null;
            }

            if (newStatus < 0 || newStatus > 2)
            {
                throw new ArgumentException("Invalid status value. Must be 0 (Pending), 1 (Completed), or 2 (Cancelled).");
            }

            order.OrderStatus = (Order.Status)newStatus;
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
