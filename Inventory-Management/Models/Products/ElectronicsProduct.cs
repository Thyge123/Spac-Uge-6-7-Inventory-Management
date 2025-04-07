using Inventory_Management.DTO_S;
using Inventory_Management.Interfaces;

namespace Inventory_Management.Models.Products
{
    public class ElectronicsProduct : IProductItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

    }
}
