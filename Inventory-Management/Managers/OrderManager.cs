using Inventory_Management.Context;
using Inventory_Management.Interfaces;
using Inventory_Management.Model;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Managers
{
    public class OrderManager
    {
        private readonly InventoryDbContext _context;
        private readonly ProductManager _productManager;

        public OrderManager(InventoryDbContext context, ProductManager productManager)
        {
            _context = context;
            _productManager = productManager;
        }
        
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();

            // Observe product stock changes
            foreach (var orderItem in order.OrderItems)
            {
                await _productManager.UpdateProductQuantity(
                    orderItem.ProductId,
                    // Get current quantity and subtract order quantity
                    (await _context.Products.FindAsync(orderItem.ProductId)).Quantity - orderItem.Quantity
                );
            }
            return order;
        }

        /*
        public async Task<Order> CreateOrderAsync(Order order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add the order
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Update product quantities
                foreach (var orderItem in order.OrderItems)
                {
                    await _productManager.UpdateProductQuantity(
                        orderItem.ProductId,
                        // Get current quantity and subtract order quantity
                        (await _context.Products.FindAsync(orderItem.ProductId)).Quantity - orderItem.Quantity
                    );
                }

                await transaction.CommitAsync();
                return order;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        */
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
