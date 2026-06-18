
#nullable enable
using System.ComponentModel.DataAnnotations;
namespace IBIZWebApp.Models;

public class SalesOrder
{
    public string? Id { get; set; } // MongoDB/ObjectId from API
    public string? OrderId { get; set; }
public string? InvId { get; set; }
    [Required(ErrorMessage = "Customer is required")]
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    [Required(ErrorMessage = "Invoice date is required")]
    public DateTime? InvDate { get; set; } = DateTime.Now;

    public string? InvoiceNo { get; set; }

    [StringLength(50)]
    public string? Reference { get; set; }

    public string TaxMode { get; set; } = "GST";

    public string BillType { get; set; } = "Regular Sales";

    public string? SalesMan { get; set; }

    public string? Agent { get; set; }

    public string ModeOfPayment { get; set; } = "Cash";

    public string CostCentre { get; set; } = "<MAIN>";

    public string PriceList { get; set; } = "Rate1";
    public int PriceListId { get; set; } // For robust dropdown binding

    public DateTime? EffectiveDate { get; set; }

    public TimeSpan? OrderTime { get; set; }

    [Range(0, 999999, ErrorMessage = "Gross amount must be positive")]
    public decimal GrossAmount { get; set; }

    [Range(0, 99, ErrorMessage = "Tax percent must be between 0 and 99")]
    public decimal TaxPercent { get; set; }

    [Range(0, 999999)]
    public decimal TaxAmount { get; set; }

    [Range(0, 999999)]
    public decimal NetAmount { get; set; }

    public string Status { get; set; } = "Draft";

    [StringLength(500)]
    public string? Narration { get; set; }

    public List<SalesOrderItem> OrderItems { get; set; } = new();

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? ModifiedDate { get; set; }
}

public class SalesOrderItem
{
    public int OrderItemId { get; set; }
    public string? OrderId { get; set; }
    public string ItemCode { get; set; } = "";
    public string ItemName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal MRP { get; set; }
    public decimal TaxPercent { get; set; }
       public string? Batch { get; set; }
}
 public class BatchUniqueModel
 {
     public string BatchUnique { get; set; }

     public string BatchCode { get; set; }
 }
