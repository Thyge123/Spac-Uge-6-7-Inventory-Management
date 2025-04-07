using Inventory_Management.Context;
using Inventory_Management.DTO;
using Inventory_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Managers
{
    public class InventoryTransactionManager
    {
        private readonly InventoryDbContext _context;

        public InventoryTransactionManager(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<InventoryTransaction> CreateTransactionAsync(CreateInventoryTransactionDTO dto, int userId)
        {
            var product = await _context.Products.FindAsync(dto.ProductId) 
                ?? throw new ArgumentException($"Product with ID {dto.ProductId} not found");

            var user = await _context.Users.FindAsync(userId)
                ?? throw new ArgumentException($"User with ID {userId} not found");

            // For sales, check if we have enough stock
            if (dto.TransactionType.ToLower() == "sale" && product.Quantity < dto.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock. Available: {product.Quantity}, Requested: {dto.Quantity}");
            }

            var transaction = new InventoryTransaction
            {
                ProductId = dto.ProductId,
                Product = product,
                UserId = userId,
                User = user,
                TransactionType = dto.TransactionType,
                Quantity = dto.Quantity,
                TransactionDate = DateTime.UtcNow
            };

            // Update product quantity based on transaction type
            switch (dto.TransactionType.ToLower())
            {
                case "sale":
                    product.Quantity -= dto.Quantity;
                    break;
                case "return":
                case "transfer":
                    product.Quantity += dto.Quantity;
                    break;
                default:
                    throw new ArgumentException($"Invalid transaction type: {dto.TransactionType}");
            }

            _context.InventoryTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsAsync()
        {
            return await _context.InventoryTransactions
                .Include(t => t.Product)
                .Include(t => t.User)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
} 