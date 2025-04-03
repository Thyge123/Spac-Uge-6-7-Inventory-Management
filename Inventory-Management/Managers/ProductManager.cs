﻿using Inventory_Management.Context;
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
        public async Task<List<ProductDTO>> GetAllProductsAsync(SortModel query)
        {
            var productsQuery = _context.Products
                .Include(p => p.Category); // Correctly include category information

            // Convert to DTOs before applying sorting
            var productDtos = await productsQuery
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

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy.Equals("ProductName", StringComparison.OrdinalIgnoreCase))
                {
                    productDtos = query.isDescending
                        ? productDtos.OrderByDescending(p => p.ProductName).ToList()
                        : productDtos.OrderBy(p => p.ProductName).ToList();
                }
                else if (query.SortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    productDtos = query.isDescending
                        ? productDtos.OrderByDescending(p => p.Price).ToList()
                        : productDtos.OrderBy(p => p.Price).ToList();
                }
                else if (query.SortBy.Equals("CategoryName", StringComparison.OrdinalIgnoreCase))
                {
                    productDtos = query.isDescending
                        ? productDtos.OrderByDescending(p => p.Category.CategoryName).ToList()
                        : productDtos.OrderBy(p => p.Category.CategoryName).ToList();
                }
                else if (query.SortBy.Equals("ProductId", StringComparison.OrdinalIgnoreCase))
                {
                    productDtos = query.isDescending
                        ? productDtos.OrderByDescending(p => p.ProductId).ToList()
                        : productDtos.OrderBy(p => p.ProductId).ToList();
                }
                else if (query.SortBy.Equals("CategoryId", StringComparison.OrdinalIgnoreCase))
                {
                    productDtos = query.isDescending
                        ? productDtos.OrderByDescending(p => p.Category.CategoryId).ToList()
                        : productDtos.OrderBy(p => p.Category.CategoryId).ToList();
                }
                else
                {
                    throw new ArgumentException("Invalid sort parameter");
                }
            }

            return productDtos;
        }
        /*
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
        */


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
