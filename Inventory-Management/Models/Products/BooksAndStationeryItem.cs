using Inventory_Management.Interfaces;

namespace Inventory_Management.Models.Products
{
    public class BooksAndStationeryItem : IProductItem
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
