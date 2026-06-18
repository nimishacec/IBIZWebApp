using System.Text.Json.Serialization;

namespace IBIZWebApp.Models;

public class ReceiptVoucher
{
    public int VoucherId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string VoucherNo { get; set; } = "";
    public string RefNo { get; set; } = "";
    public int CostCentreId { get; set; }
    public int DebitLedgerId { get; set; }
    public decimal CreditLedgerId { get; set; }
    public decimal Amount { get; set; }
    public string Narration { get; set; } = "";
    public string ReceiptMode { get; set; } = "Cash";
    public ChequeDetails ChequeDetails { get; set; } = new();
    public List<InvoiceAllocation> Allocations { get; set; } = new();
}

public class InvoiceAllocation
{
     [JsonPropertyName("invId")]
    public int InvoiceId { get; set; }

    [JsonPropertyName("invNo")]
    public string InvoiceNo { get; set; } = "";

    [JsonPropertyName("invDate")]
    public DateTime InvoiceDate { get; set; }

    public int CreditDays { get; set; }

    [JsonPropertyName("billAmt")]
    public decimal BillAmount { get; set; }

    [JsonPropertyName("currentAmt")]
    public decimal CurrentAmount { get; set; }

    public decimal SaAmount { get; set; }
    public int Order { get; set; } = 1;
}

public class ChequeDetails
{
    public string ChequeNo { get; set; } = "";
    public string IssuingBank { get; set; } = "";
}

public class LedgerOption
{
    [JsonPropertyName("ledgerId")]
    public int LedgerId { get; set; }

    [JsonPropertyName("ledgerName")]
    public string LedgerName { get; set; } = "";

    [JsonPropertyName("ledgerType")]
    public string LedgerType { get; set; } = "";
}

public class CostCentre
{
    [JsonPropertyName("costCentreId")]
    public int CostCentreId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
}
