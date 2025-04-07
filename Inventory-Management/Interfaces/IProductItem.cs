using Inventory_Management.Models;

namespace Inventory_Management.Interfaces
{
    public interface IProductItem
    {
        int ProductId { get; set; }
        int CategoryId { get; set; }
        string ProductName { get; set; }
        decimal Price { get; set; }
    }
}
