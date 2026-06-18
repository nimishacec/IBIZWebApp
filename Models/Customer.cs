using System.ComponentModel.DataAnnotations;
namespace IBIZWebApp.Models;

public class Customer
{
    [Required(ErrorMessage = "Customer ID is required")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Customer name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Customer name must be between 2 and 100 characters")]
    public string Name { get; set; } = "";

    [Phone(ErrorMessage = "Invalid phone number")]
    [Required(ErrorMessage = "Phone number is required")]
    public string Phone { get; set; } = "";

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(10)]
    public string? Pincode { get; set; }

    [StringLength(20)]
    public string? GSTIN { get; set; }

    public string? CityPin { get; set; }

    [Range(0, 9999999, ErrorMessage = "Credit days must be between 0 and 9999999")]
    public int CreditDays { get; set; }

    public string CustomerType { get; set; } = "Regular";

    [StringLength(500)]
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? ModifiedDate { get; set; }
}
