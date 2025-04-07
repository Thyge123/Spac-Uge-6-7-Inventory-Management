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
        public async Task<List<ProductDTO>> GetAllProductsAsync(string? sortBy, bool isDescending, ProductsFilterModel? filter = null)
        {
            // Start with the base query including the Category
            IQueryable<Product> productsQuery = _context.Products
                .Include(p => p.Category);

            // Apply filters
            productsQuery = ApplyFilters(productsQuery, filter);

            // Convert to DTOs
            var productDtos = await ConvertToProductDTOs(productsQuery);

            // Apply sorting
            productDtos = ApplySorting(productDtos, sortBy, isDescending);

            return productDtos;
        }

        private IQueryable<Product> ApplyFilters(IQueryable<Product> query, ProductsFilterModel? filter)
        {
            if (filter == null)
                return query;

            if (!string.IsNullOrWhiteSpace(filter.ProductName))
            {
                query = query.Where(p => p.ProductName.Contains(filter.ProductName));
            }

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
            {
                query = query.Where(p => p.Category.CategoryName.Contains(filter.CategoryName));
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            if (filter.Category != null)
            {
                query = query.Where(p => p.CategoryId == filter.Category.CategoryId);
            }

            return query;
        }

        private async Task<List<ProductDTO>> ConvertToProductDTOs(IQueryable<Product> query)
        {
            return await query
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
                .ToListAsync();
        }

        private List<ProductDTO> ApplySorting(List<ProductDTO> products, string? sortBy, bool isDescending)
        {
            if (string.IsNullOrEmpty(sortBy))
                return products;

            return sortBy.ToLowerInvariant() switch
            {
                "productname" => isDescending
                    ? products.OrderByDescending(p => p.ProductName).ToList()
                    : products.OrderBy(p => p.ProductName).ToList(),

                "price" => isDescending
                    ? products.OrderByDescending(p => p.Price).ToList()
                    : products.OrderBy(p => p.Price).ToList(),

                "categoryname" => isDescending
                    ? products.OrderByDescending(p => p.Category.CategoryName).ToList()
                    : products.OrderBy(p => p.Category.CategoryName).ToList(),

                "productid" => isDescending
                    ? products.OrderByDescending(p => p.ProductId).ToList()
                    : products.OrderBy(p => p.ProductId).ToList(),

                "categoryid" => isDescending
                    ? products.OrderByDescending(p => p.Category.CategoryId).ToList()
                    : products.OrderBy(p => p.Category.CategoryId).ToList(),

                _ => throw new ArgumentException("Invalid sort parameter")
            };
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

        // Updates an existing product in the database
        public async Task<Product> UpdateProduct(int productId, string productName, decimal price, int? categoryId)
        {
            // Fetch the product from the database
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found"); // Product not found
            }
            // Update the product details
            product.ProductName = productName;
            product.CategoryId = (int)categoryId;

            // Only update category if a value was provided
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                product.CategoryId = categoryId.Value;
            }

            product.Price = price;
            // Save changes to the database
            await _context.SaveChangesAsync();
            return product; // Update successful
        }

        // Deletes a product from the database
        public async Task DeleteProduct(int productId)
        {
            // Fetch the product from the database
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found"); // Product not found
            }
            // Remove the product from the database
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return; // Deletion successful
        }
    }
}
