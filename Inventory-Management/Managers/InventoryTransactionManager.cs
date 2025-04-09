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
            try
            {
                if (dto == null)
                {
                    throw new ArgumentNullException(nameof(dto), "Transaction data cannot be null");
                }

                if (dto.ProductId <= 0)
                {
                    throw new ArgumentException("Product ID must be greater than zero", nameof(dto.ProductId));
                }

                if (userId <= 0)
                {
                    throw new ArgumentException("User ID must be greater than zero", nameof(userId));
                }

                if (string.IsNullOrWhiteSpace(dto.TransactionType))
                {
                    throw new ArgumentException("Transaction type cannot be empty", nameof(dto.TransactionType));
                }

                if (dto.Quantity <= 0)
                {
                    throw new ArgumentException("Quantity must be greater than zero", nameof(dto.Quantity));
                }

                // Find product with tracking enabled
                var product = await _context.Products.FindAsync(dto.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {dto.ProductId} not found");
                }

                // Find user with tracking enabled
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }

                // For sales, check if we have enough stock
                var normalizedType = dto.TransactionType.ToLower();
                if (normalizedType == "sale" && product.Quantity < dto.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product '{product.ProductName}'. Available: {product.Quantity}, Requested: {dto.Quantity}");
                }

                var transaction = new InventoryTransaction
                {
                    ProductId = dto.ProductId,
                    Product = product,
                    UserId = userId,
                    User = user,
                    TransactionType = dto.TransactionType.ToUpperInvariant(), // Standardize to uppercase
                    Quantity = dto.Quantity,
                    TransactionDate = DateTime.UtcNow
                };

                // Update product quantity based on transaction type
                switch (normalizedType)
                {
                    case "sale":
                        product.Quantity -= dto.Quantity;
                        break;
                    case "return":
                    case "transfer":
                        product.Quantity += dto.Quantity;
                        break;
                }

                _context.InventoryTransactions.Add(transaction);
                await _context.SaveChangesAsync();

                return transaction;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Database error while creating transaction: {ex.Message}", ex);
            }
            catch (Exception ex) when (ex is not ArgumentNullException &&
                                      ex is not ArgumentException &&
                                      ex is not InvalidOperationException)
            {
                throw new InvalidOperationException($"Error creating transaction: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsAsync(
            string? productName = null,
            string? transactionType = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? userId = null)
        {
            try
            {
                IQueryable<InventoryTransaction> query = _context.InventoryTransactions
                    .Include(t => t.Product)
                    .Include(t => t.User);

                // Apply filters
                if (!string.IsNullOrWhiteSpace(productName))
                {
                    query = query.Where(t => t.Product.ProductName.Contains(productName));
                }

                if (!string.IsNullOrWhiteSpace(transactionType))
                {
                    query = query.Where(t => t.TransactionType.ToLower() == transactionType.ToLower());
                }

                if (startDate.HasValue)
                {
                    query = query.Where(t => t.TransactionDate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(t => t.TransactionDate <= endDate.Value.AddDays(1).AddSeconds(-1));
                }

                if (userId.HasValue && userId.Value > 0)
                {
                    query = query.Where(t => t.UserId == userId.Value);
                }

                return await query.OrderByDescending(t => t.TransactionDate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving transactions: {ex.Message}", ex);
            }
        }

        public async Task<InventoryTransaction> GetTransactionByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Transaction ID must be greater than zero", nameof(id));
                }

                var transaction = await _context.InventoryTransactions
                    .Include(t => t.Product)
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.TransactionId == id);

                if (transaction == null)
                {
                    throw new InvalidOperationException($"Transaction with ID {id} not found");
                }

                return transaction;
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                throw new InvalidOperationException($"Error retrieving transaction with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByProductIdAsync(int productId)
        {
            try
            {
                if (productId <= 0)
                {
                    throw new ArgumentException("Product ID must be greater than zero", nameof(productId));
                }

                // Check if product exists
                bool productExists = await _context.Products.AnyAsync(p => p.ProductId == productId);
                if (!productExists)
                {
                    throw new InvalidOperationException($"Product with ID {productId} not found");
                }

                return await _context.InventoryTransactions
                    .Include(t => t.User)
                    .Where(t => t.ProductId == productId)
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                throw new InvalidOperationException($"Error retrieving transactions for product ID {productId}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByUserIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("User ID must be greater than zero", nameof(userId));
                }

                // Check if user exists
                bool userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }

                return await _context.InventoryTransactions
                    .Include(t => t.Product)
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                throw new InvalidOperationException($"Error retrieving transactions for user ID {userId}: {ex.Message}", ex);
            }
        }
    }
}