using System;
using System.Collections.Generic;

namespace IBIZWebApp.Models
{
    public class AgeingSettlement
    {
        public string Id { get; set; }
        public int InvId { get; set; }
        public string InvNo { get; set; }
        public DateTime InvDate { get; set; }
        public int LedgerId { get; set; }
        public decimal BillAmount { get; set; }
        public List<SettlementDetail> Settlements { get; set; } = new();
    }

    public class SettlementDetail
    {
        public decimal SaAmount { get; set; }
        public int ReceiptId { get; set; }
        public string ReceiptNo { get; set; }
        public int VchTypeID { get; set; }
        public DateTime Date { get; set; }
    }
}
