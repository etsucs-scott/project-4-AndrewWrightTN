namespace Project4Web.Models
{
    /// <summary>
    /// This holds the info for a full transaction
    /// </summary>
    public class Transaction
    {
     
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsIncome { get; set; }
    }
}
