using System;

namespace IBIZWebApp.Models
{
    public class SalesReportItem
    {
        public int? Year { get; set; }
        public string? Month { get; set; }
        public decimal? TotalBillAmount { get; set; }
        public string? BillAmountDifference { get; set; }
        public string? BillAmountDifferencePercentage { get; set; }
        public DateTime? InvDate { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? ItemName { get; set; }
        public string? ItemCode { get; set; }
        public int? InvId { get; set; }
        public string? InvNo { get; set; }
        public string? VchType { get; set; }
        public string? Party { get; set; }
        public string? PartyAddress { get; set; }
        public string? MobileNo { get; set; }
        public string? Mop { get; set; }
        public string? PartyGstin { get; set; }
        public string? BillType { get; set; }
        public string? UserNarration { get; set; }
        public string? DeliveryDetails { get; set; }
        public string? DespatchDetails { get; set; }
        public string? TermsOfDelivery { get; set; }
        public string? OrderDetails { get; set; }
        public string? DeliveryNoteDetails { get; set; }
        public string? Gsttype { get; set; }
        public string? Email { get; set; }
        public decimal? GrossAmt { get; set; }
        public decimal? ItemDiscountPer { get; set; }
        public decimal? ItemDiscount { get; set; }
        public decimal? ItemDiscountTotal { get; set; }
        public decimal? DiscPer { get; set; }
        public decimal? Discount { get; set; }
        public decimal? NonTaxable { get; set; }
        public decimal? Taxable { get; set; }
        public decimal? TaxAmt { get; set; }
        public decimal? Cgsttotal { get; set; }
        public decimal? Sgsttotal { get; set; }
        public decimal? Igsttotal { get; set; }
        public string? FloodCessTot { get; set; }
        public string? CessAmountTot { get; set; }
        public string? QtyCompCessAmount { get; set; }
        public System.Text.Json.JsonElement? ExtraCharges { get; set; }
        public string? GiftVoucherAmount { get; set; }
        public decimal? CashDiscount { get; set; }
        public decimal? OtherExpense { get; set; }
        public decimal? INetAmount { get; set; }
        public decimal? RoundOff { get; set; }
        public decimal? BillAmt { get; set; }
        public string? CCName { get; set; }
        public string? TaxMode { get; set; }
        public string? Manufacturer { get; set; }
        public string? Category { get; set; }
        public string? Taxname { get; set; }
        public string? Salesman { get; set; }
        public decimal? Quantity { get; set; }
        public string? HSNCode { get; set; }
        public string? Unit { get; set; }
        public string? Batch { get; set; }
        public decimal? Rate { get; set; }
        public decimal? MRP { get; set; }
        public decimal? ITaxableAmount { get; set; }
        public decimal? InonTaxableAmount { get; set; }
        public decimal? TaxPer { get; set; }
        public decimal? TotalTaxamount { get; set; }
        public decimal? Totalgrossamount { get; set; }
        public string? ProductType { get; set; }
        public int? OrderSequence { get; set; }
    }
}
