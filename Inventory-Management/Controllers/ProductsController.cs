using Inventory_Management.DTO_S;
using Inventory_Management.Factories;
using Inventory_Management.Interfaces;
using Inventory_Management.Managers;
using Inventory_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ProductManager _productManager;
        public ProductsController(ProductManager productManager)
        {
            _productManager = productManager;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productManager.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No Products found");
            }
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productManager.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound("No product found with specified Id");
            }
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO item)
        {
            // Validate the incoming data
            try
            {
                // Create a new inventory item
                var createdItem = await _productManager.CreateProduct(
                    item.ProductId,
                    item.CategoryId,
                    item.ProductName,
                    item.Price
                );

                // Check if the item was created successfully, if not return a bad request
                if (createdItem == null)
                {
                    return BadRequest("Failed to create the product");
                }

                return CreatedAtAction(nameof(GetProductById), new { id = createdItem.ProductId }, createdItem); // Return the created item with a 201 status code
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
