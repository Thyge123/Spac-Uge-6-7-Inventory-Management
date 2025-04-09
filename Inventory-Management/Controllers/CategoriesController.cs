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
            try
            {
                var categories = await _categoryManager.GetAllCategoriesAsync(); // Fetch all categories
                if (categories == null || !categories.Any()) // Check if the list is empty or null
                {
                    return NotFound("No Categories found"); // Return NotFound if no categories are found
                }
                return Ok(categories); // Return the list of categories
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message); // Return 500 Internal Server Error
            }
            
        }
        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                if (id <= 0) // Check if the ID is valid
                {
                    return BadRequest("Invalid category ID"); // Return BadRequest if the ID is invalid
                }
                var category = await _categoryManager.GetCategoryByIdAsync(id); // Fetch a single category by ID
                if (category == null)
                {
                    return NotFound("No category found with specified Id"); // Return NotFound if the category is not found
                }
                return Ok(category); // Return the category details
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message); // Return 500 Internal Server Error
            }  
        }
    }
}
