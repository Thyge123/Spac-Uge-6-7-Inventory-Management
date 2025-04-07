using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Inventory_Management.Models;

namespace Inventory_Management.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // User role (e.g., Admin, User)
        public DateTime CreatedAt { get; set; } // Date and time when the user was created
        public DateTime UpdatedAt { get; set; } // Date and time when the user was last updated

        // Navigation properties
        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
    }
}
