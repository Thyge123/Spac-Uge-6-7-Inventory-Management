using Inventory_Management.Context;
using Inventory_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management.Managers
{
    public class UserManager
    {
        private readonly InventoryDbContext _context;

        public UserManager(InventoryDbContext context)
        {
            _context = context;
        }

        // Add a new user
        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                HashPassword(user);
                user.CreatedAt = DateTime.UtcNow;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                throw new Exception("Error adding user", e);
            }
        }

        // Get all users
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving users", e);
            }
        }

        // Get a user by ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving user", e);
            }
        }

        // Get a user by username
        public async Task<User> GetByUsername(string username)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving user by username", e);
            }
        }

        // Update a user
        public async Task UpdateUserAsync(User user)
        {
            try
            {
                user.UpdatedAt = DateTime.UtcNow; // Update the timestamp
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error updating user", e);
            }
        }

        // Delete a user
        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    throw new InvalidOperationException("Product not found"); // Product not found
                }
                // Remove the product from the database
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return; // Deletion successful
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting user", e);
            }
        }

        // Hash password
        public void HashPassword(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash the password before storing it
        }

        // Verify password
        public bool VerifyPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password); // Verify the password when user logs in
        }
    }
}
