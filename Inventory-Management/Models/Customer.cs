using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inventory_Management.Model
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }


        [StringLength(100)]
        public string City { get; set; }

        // Navigation property
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
