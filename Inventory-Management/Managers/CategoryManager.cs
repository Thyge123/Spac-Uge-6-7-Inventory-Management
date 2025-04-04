using Inventory_Management.Context;
using Inventory_Management.DTO_S;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Managers
{
    public class CategoryManager
    {
        private readonly InventoryDbContext _context;

        public CategoryManager(InventoryDbContext context)
        {
            _context = context;
        }

        // Returns a list of all categories with their details
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            // Fetch all categories from the database and map them to CategoryDTO
            return await _context.Categories
                .Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .ToListAsync(); // Asynchronous call to fetch all categories
        }

        // Returns a single category by its ID
        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            // Fetch a single category by ID from the database and map it to CategoryDTO
            return await _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .FirstOrDefaultAsync(); // Asynchronous call to fetch a single category
        }
    }
}
