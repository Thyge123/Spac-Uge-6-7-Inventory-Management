using Inventory_Management.Context;
using Inventory_Management.DTO_S;
using Inventory_Management.Factories;
using Inventory_Management.Interfaces;
using Inventory_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Managers
{
    public class ProductManager
    {
        private readonly InventoryDbContext _context;
        private readonly IProductFactory _productFactory;

        public ProductManager(InventoryDbContext context, IProductFactory productFactory)
        {
            _context = context;
            _productFactory = productFactory;
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


        // Creates a new inventory item and saves it to the database
        public async Task<IProductItem> CreateProduct(int productId, int categoryId,
                                                 string productName, decimal price)
        {
            // Create domain object from factory
            var item = _productFactory.CreateProductItem(productId, categoryId, productName, price);

            // Map from domain model to database entity
            var productEntity = new Product
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                CategoryId = item.CategoryId
            };

            // Check if the product already exists in the database
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductName == productEntity.ProductName);

            if (existingProduct != null)
            {
                // If the product already exists, return the existing product
                return (IProductItem)existingProduct;
            }

            // Add entity to database
            _context.Products.Add(productEntity);
            await _context.SaveChangesAsync();

            // Return domain object
            return item;
        }
    }
}
