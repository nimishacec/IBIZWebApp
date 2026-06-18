namespace IBIZWebApp.Models
{
    public class LedgerBalanceResponse
    {
        public int LedgerId { get; set; }
        public string LedgerName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
