// Enable nullable reference types for this file
#nullable enable
namespace IBIZWebApp.Models
{
    public class LedgerBillStatusRequest
    {
        public int InvId { get; set; }
        public string? InvNo { get; set; }
        public string? InvDate { get; set; }
        public int CreditDays { get; set; }
        public decimal BillAmt { get; set; }
        public decimal? LedgerId { get; set; }
        public string? MOP { get; set; }
        public string? VcType { get; set; }
        public decimal CurrentAmt { get; set; }
        public decimal SaAmount { get; set; }
        public int VchTypeID { get; set; }
        public int SettleStatus { get; set; }
    }
    public class BillAllocationRequest
    {
        public decimal? LedgerId { get; set; }
        public List<LedgerBillStatusRequest>? Allocations { get; set; }
    }


}