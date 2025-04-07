namespace Inventory_Management.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        
        public enum Status
        {
            Sale,
            Return,
            Exchange
        }

        public Status TransactionStatus { get; set; }
    }
}
