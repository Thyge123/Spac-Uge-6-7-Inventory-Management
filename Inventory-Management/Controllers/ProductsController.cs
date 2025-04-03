using Inventory_Management.Managers;
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
    }
}
