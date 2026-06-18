
#nullable enable
using System.ComponentModel.DataAnnotations;
namespace IBIZWebApp.Models;

public class SalesItem
{
    public int ItemId { get; set; }

    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public decimal TaxPercent { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
    public int InvID { get; set; }
    public int Qty { get; set; }
    public decimal Rate { get; set; }
    public int UnitId { get; set; }
    public string Batch { get; set; } = string.Empty;
    public decimal TaxPer { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal MRP { get; set; }
    public int SlNo { get; set; }
    public decimal Prate { get; set; }
    public int Free { get; set; }
    public string SerialNos { get; set; } = string.Empty;
    public decimal ItemDiscount { get; set; }
    public string BatchCode { get; set; } = string.Empty;
    public decimal iCessOnTax { get; set; }
    public int blnCessOnTax { get; set; }
    public string? Expiry { get; set; }
    public decimal ItemDiscountPer { get; set; }
    public decimal RateInclusive { get; set; }
    public decimal ITaxableAmount { get; set; }
    public decimal INetAmount { get; set; }
    public decimal CGSTTaxPer { get; set; }
    public decimal CGSTTaxAmt { get; set; }
    public decimal SGSTTaxPer { get; set; }
    public decimal SGSTTaxAmt { get; set; }
    public decimal IGSTTaxPer { get; set; }
    public decimal IGSTTaxAmt { get; set; }
    public decimal iRateDiscPer { get; set; }
    public decimal iRateDiscount { get; set; }
    public string BatchUnique { get; set; } = string.Empty;
    public int blnQtyIN { get; set; }
    public decimal CRate { get; set; }
    public string Unit { get; set; } = string.Empty;
    public int ItemStockID { get; set; }
    public decimal IcessPercent { get; set; }
    public decimal IcessAmt { get; set; }
    public decimal IQtyCompCessPer { get; set; }
    public decimal IQtyCompCessAmt { get; set; }
    public decimal StockMRP { get; set; }
    public decimal BaseCRate { get; set; }
    public decimal InonTaxableAmount { get; set; }
    public decimal IAgentCommPercent { get; set; }
    public int BlnDelete { get; set; }
    public string StrOfferDetails { get; set; } = string.Empty;
    public int BlnOfferItem { get; set; }
    public decimal BalQty { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal iFloodCessPer { get; set; }
    public decimal iFloodCessAmt { get; set; }
    public decimal Costrate { get; set; }
    public decimal CostValue { get; set; }
    public decimal Profit { get; set; }
    public decimal ProfitPer { get; set; }
    public decimal CRateTaxInclusive { get; set; }
    public decimal BaseCrateTaxInclusive { get; set; }
    public decimal DiscMode { get; set; }
    public decimal srate1per { get; set; }
    public decimal srate2per { get; set; }
    public decimal srate3per { get; set; }
    public decimal srate4per { get; set; }
    public decimal srate5per { get; set; }
    public decimal srate1 { get; set; }
    public decimal srate2 { get; set; }
    public decimal srate3 { get; set; }
    public decimal srate4 { get; set; }
    public decimal srate5 { get; set; }
    public decimal APRATE { get; set; }
    public string HSNCode { get; set; } = string.Empty;
    public string ItemNarration { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int AppStatus { get; set; }
}
