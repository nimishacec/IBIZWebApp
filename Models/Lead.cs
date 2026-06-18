
#nullable enable
using System.ComponentModel.DataAnnotations;
namespace IBIZWebApp.Models;

public class LeadRecord
{
    public int LeadId { get; set; }

    [Required(ErrorMessage = "Enquiry date is required")]
    public DateTime EnquiryDate { get; set; } = DateTime.Now;

    [StringLength(50)]
    public string? CostCentre { get; set; }

    [Required(ErrorMessage = "Enquiry taken by is required")]
    [StringLength(100)]
    public string EnquiryTakenBy { get; set; } = "";

    [Required(ErrorMessage = "Assigned to is required")]
    [StringLength(100)]
    public string AssignedTo { get; set; } = "";

    public string Status { get; set; } = "Open";

    [StringLength(100)]
    public string? Agent { get; set; }

    [Required(ErrorMessage = "Customer name is required")]
    [StringLength(100)]
    public string CustomerName { get; set; } = "";

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Item { get; set; }

    public DateTime? FollowupDate { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    public int? LeadValue { get; set; }

    public List<LeadItemRecord> LeadItems { get; set; } = new();

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? ModifiedDate { get; set; }
}

public class LeadItemRecord
{
    public int LeadItemId { get; set; }
    public int LeadId { get; set; }
    public string ItemCode { get; set; } = "";
    public string ItemName { get; set; } = "";
    public int Quantity { get; set; }
    public string? Narration { get; set; }
    public int? PlanningMonths { get; set; }
}
