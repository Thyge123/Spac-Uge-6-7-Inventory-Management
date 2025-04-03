using Inventory_Management.Context;
using Inventory_Management.DTO_S;
using Inventory_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Managers
{
    public class ProductManager
    {
        private readonly InventoryDbContext _context;

        public ProductManager(InventoryDbContext context)
        {
            _context = context;
        }

        // Returns a list of all products with their details and category information 
        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            // Fetch all products from the database and map them to ProductDTO
            return await _context.Products
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Category = new CategoryDTO
                    {
                        CategoryId = p.Category.CategoryId,
                        CategoryName = p.Category.CategoryName
                    }
                })
                .ToListAsync(); // Asynchronous call to fetch all products
        }

        // Returns a single product by its ID with category information
        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            // Fetch a single product by ID from the database and map it to ProductDTO
            return await _context.Products
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Category = new CategoryDTO
                    {
                        CategoryId = p.Category.CategoryId,
                        CategoryName = p.Category.CategoryName
                    }
                })
                .FirstOrDefaultAsync(); // Asynchronous call to fetch a single product
        }
    }
}
