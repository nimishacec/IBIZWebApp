// Enable nullable reference types for this file
#nullable enable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace IBIZWebApp.Models
{
    public class SalesOrderApiModel
    {
        [JsonPropertyName("salesOrder")]
        public SalesData? salesOrder { get; set; }
        [JsonPropertyName("salesOrderItem")]
        public List<SalesItemData>? salesOrderItem { get; set; }
    }
public class SalesApiModel
    {
        [JsonPropertyName("sales")]
        public SalesData? sales { get; set; }
        [JsonPropertyName("salesItem")]
        public List<SalesItemData>? salesItem { get; set; }
    }
    public class SalesData
    {
        public double InvId { get; set; }
        public string? InvNo { get; set; }
        public double AutoNum { get; set; }
        public string? Prefix { get; set; }
        public DateTime? InvDate { get; set; }
        public string? VchType { get; set; }
        public string? MOP { get; set; }
        public double TaxModeID { get; set; }
        public double LedgerId { get; set; }
        public string? Party { get; set; }
        public double Discount { get; set; }
        public double TaxAmt { get; set; }
        public double GrossAmt { get; set; }
        public double BillAmt { get; set; }
        public double Cancelled { get; set; }
        public double OtherExpense { get; set; }
        public double SalesManID { get; set; }
        public double Taxable { get; set; }
        public double NonTaxable { get; set; }
        public double ItemDiscountTotal { get; set; }
        public double RoundOff { get; set; }
        public string? UserNarration { get; set; }
        public double SortNumber { get; set; }
        public double DiscPer { get; set; }
        public double VchTypeID { get; set; }
        public double CCID { get; set; }
        public double CurrencyID { get; set; }
        public string? PartyAddress { get; set; }
        public int UserID { get; set; }
        public double AgentID { get; set; }
        public double CashDiscount { get; set; }
        public double DPerType_ManualCalc_Customer { get; set; }
        public double NetAmount { get; set; }
        public string? RefNo { get; set; }
        public double CashPaid { get; set; }
        public double CardPaid { get; set; }
        public double blnWaitforAuthorisation { get; set; }
        public double UserIDAuth { get; set; }
        public string? BillTime { get; set; }
        public double StateID { get; set; }
        public string? ImplementingStateCode { get; set; }
        public string? GSTType { get; set; }
        public double CGSTTotal { get; set; }
        public double SGSTTotal { get; set; }
        public double IGSTTotal { get; set; }
        public string? PartyGSTIN { get; set; }
        public string? BillType { get; set; }
        public string? blnHold { get; set; }
        public double PriceListID { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? partyCode { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? TaxType { get; set; }
        public double QtyTotal { get; set; }
        public double DestCCID { get; set; }
        public string? AgentCommMode { get; set; }
        public double AgentCommAmount { get; set; }
        public double AgentLID { get; set; }
        public double BlnStockInsert { get; set; }
        public string? BlnConverted { get; set; }
        public string? ConvertedParentVchTypeID { get; set; }
        public string? ConvertedVchTypeID { get; set; }
        public string? ConvertedVchNo { get; set; }
        public string? ConvertedVchID { get; set; }
        public string? DeliveryNoteDetails { get; set; }
        public string? OrderDetails { get; set; }
        public string? IntegrityStatus { get; set; }
        public string? CustomerpointsSettled { get; set; }
        public string? blnCashPaid { get; set; }
        public string? originalsalesinvid { get; set; }
        public string? retuninvid { get; set; }
        public string? returnamount { get; set; }
        public string? SystemName { get; set; }
        public string? LastUpdateDate { get; set; }
        public string? LastUpdateTime { get; set; }
        public string? DeliveryDetails { get; set; }
        public string? DespatchDetails { get; set; }
        public string? TermsOfDelivery { get; set; }
        public string? FloodCessTot { get; set; }
        public double CounterID { get; set; }
        public System.Text.Json.JsonElement? ExtraCharges { get; set; }
        public string? processed { get; set; }
        public string? CessAmountTot { get; set; }
        public string? QtyCompCessAmount { get; set; }
        public string? GiftVoucherNo { get; set; }
        public string? GiftVoucherAmount { get; set; }
        public string? AutoNumRefNo { get; set; }
        public string? AckNo { get; set; }
        public string? AckDt { get; set; }
        public string? Irn { get; set; }
        public string? SignedInvoice { get; set; }
        public string? SignedQRCode { get; set; }
        public string? Status { get; set; }
        public string? EwbNo { get; set; }
        public string? EwbDt { get; set; }
        public string? EwbValidTill { get; set; }
        public string? Remarks { get; set; }
        public string? EInvoiceIRN { get; set; }
        public string? StockEffectiveDate { get; set; }
        public string? InTransit { get; set; }
        public string? VsInvDate { get; set; }
        public string? VsEffectiveDate { get; set; }
        public string? PackingList { get; set; }
        public string? BuyersName { get; set; }
        public string? PreCarriageBy { get; set; }
        public string? PlaceOfReceiptBy { get; set; }
        public string? FlightNo { get; set; }
        public string? FinalDestination { get; set; }
        public string? CountryOfDestination { get; set; }
        public string? PortCode { get; set; }
        public string? GrowsWeight { get; set; }
        public string? CountryOfOrigin { get; set; }
        public double CurrencyAmt { get; set; }
        public double CurrencyRate { get; set; }
        public int AppStatus { get; set; }
        public string? RivInvDate { get; set; }
        public string? ProjectName { get; set; }
        public int Projectstatus { get; set; }
        public bool IsModified { get; set; }
    }

    public class SalesItemData
    {
        public int InvID { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public int UnitId { get; set; }
        public double TaxPer { get; set; }
          public decimal INetAmount { get; set; }
        public double TaxAmount { get; set; }
        public double Discount { get; set; }
        public double MRP { get; set; }
        public int SlNo { get; set; }
        public double Prate { get; set; }
        public double Free { get; set; }
        public double ItemDiscount { get; set; }
        public string? BatchCode { get; set; }
        public string? Expiry { get; set; }
        public double IGSTTaxAmt { get; set; }
        public double IRateDiscPer { get; set; }
        public double IRateDiscount { get; set; }
        public string? BatchUnique { get; set; }
        public int blnQtyIN { get; set; }
        public double CRate { get; set; }
        public string? Unit { get; set; }
        public int ItemStockID { get; set; }
        public double IcessPercent { get; set; }
        public double IcessAmt { get; set; }
        public double IQtyCompCessPer { get; set; }
        public double IQtyCompCessAmt { get; set; }
        public double StockMRP { get; set; }
        public double BaseCRate { get; set; }
        public double InonTaxableAmount { get; set; }
        public double IAgentCommPercent { get; set; }
        public int BlnDelete { get; set; }
        public int Id { get; set; }
        public double BlnOfferItem { get; set; }
        public double BalQty { get; set; }
        public double GrossAmount { get; set; }
        public double IFloodCessPer { get; set; }
           
        public double CostValue { get; set; }
        public double Profit { get; set; }
        public double ProfitPer { get; set; }
        public double CRateTaxInclusive { get; set; }
        public double BaseCrateTaxInclusive { get; set; }
        public double DiscMode { get; set; }
        public double Srate1Per { get; set; }
        public double Srate2Per { get; set; }
        public double Srate3Per { get; set; }
        public double Srate4Per { get; set; }
        public double Srate5Per { get; set; }
        public double Srate1 { get; set; }
        public double Srate2 { get; set; }
        public double Srate3 { get; set; }
        public double Srate4 { get; set; }
        public double Srate5 { get; set; }
        public double APRATE { get; set; }
        public int AppStatus { get; set; }
        public double Costrate { get; set; }
        public string HSNCode { get; set; } = string.Empty;
        public string ItemNarration { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
    }
}
