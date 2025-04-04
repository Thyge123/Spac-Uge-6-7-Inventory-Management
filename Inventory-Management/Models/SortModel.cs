namespace Inventory_Management.Models
{
    public class SortModel
    {
        public string SortBy { get; set; } = "ProductId"; // Default sort by ProductId

        public bool isDescending { get; set; } = false; // Default sort order is ascending
    }
}
