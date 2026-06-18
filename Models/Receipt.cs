using System.ComponentModel.DataAnnotations;

namespace IBIZWebApp.Models;

public class Receipt
{
    public string Id { get; set; } 
    public int ReceiptId { get; set; }

    [Required(ErrorMessage = "Order ID is required")]
    public int OrderId { get; set; }

    [Required(ErrorMessage = "Customer name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Customer name must be 2–100 characters")]
    public string CustomerName { get; set; } = "";

    [Required(ErrorMessage = "Receipt number is required")]
    [StringLength(20, ErrorMessage = "Receipt number must be at most 20 characters")]
    public string ReceiptNo { get; set; } = "";

    [Required(ErrorMessage = "Receipt date is required")]
    public DateTime ReceiptDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    public string Status { get; set; } = "Pending";

    [StringLength(200, ErrorMessage = "Reference number must be at most 200 characters")]
    public string ReferenceNo { get; set; } = "";

    [StringLength(500, ErrorMessage = "Narration must be at most 500 characters")]
    public string Narration { get; set; } = "";

    public string PaymentMode { get; set; } = "Cash";

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? ModifiedDate { get; set; }

    [StringLength(50)]
    public string CreatedBy { get; set; } = "System";
}
