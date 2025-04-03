using Inventory_Management.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly CategoryManager _categoryManager;
        public CategoriesController(CategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }
        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryManager.GetAllCategoriesAsync(); // Fetch all categories
            if (categories == null || !categories.Any()) // Check if the list is empty or null
            {
                return NotFound("No Categories found"); // Return NotFound if no categories are found
            }
            return Ok(categories); // Return the list of categories
        }
        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(id); // Fetch a single category by ID
            if (category == null)
            {
                return NotFound("No category found with specified Id"); // Return NotFound if the category is not found
            }
            return Ok(category); // Return the category details
        }

    }
}
