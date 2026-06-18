#nullable enable
using IBIZWebApp.Models;

namespace IBIZWebApp.Services;

public interface IReceiptService
{
    Task<ApiResponse<PaginatedResult<Receipt>>> GetReceiptsAsync(PaginationParams @params);
    Task<ApiResponse<Receipt?>> GetReceiptByIdAsync(int receiptId);
    Task<ApiResponse<PaginatedResult<Receipt>>> SearchReceiptsByDateAsync(DateTime from, DateTime to, int page, int pageSize);
    Task<ApiResponse<List<Receipt>>> GetReceiptsByStatusAsync(string status);
    Task<bool> CreateReceiptAsync(Receipt receipt);
    Task<bool> UpdateReceiptAsync(Receipt receipt);
    Task<bool> DeleteReceiptAsync(int id);

    Task<List<PendingBill>> GetPendingBillsAsync(int ledgerId);
}

public class PendingBill
{
    public string BillNo { get; set; }
    public string BillDate { get; set; }
    public decimal BillAmount { get; set; }
    public decimal PendingAmount { get; set; }
}
