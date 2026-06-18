using IBIZWebApp.Models;

namespace IBIZWebApp.Services;

public interface IReceiptVoucherService
{
    Task<string> GenerateVoucherNoAsync();
    Task<List<CostCentre>> GetCostCentresAsync();
    Task<List<LedgerOption>> GetCashBankLedgersAsync();
    Task<List<LedgerOption>> GetCustomerLedgersAsync();
    Task<decimal> GetLedgerBalanceAsync(int ledgerId);
    Task<List<InvoiceAllocation>> GetOpenInvoicesAsync(int ledgerId);
    Task<List<ReceiptVoucher>> GetReceiptsByDateAsync(DateTime date);
    Task<ReceiptVoucher?> GetReceiptVoucherByIdAsync(int voucherId);
    Task<int> SaveReceiptVoucherAsync(ReceiptVoucher voucher);
    Task<int> SaveInvoiceAllocationSettlementAsync(AgeingSettlement settlement);
 Task<bool> UpdateLedgerBillStatusListAsync(BillAllocationRequest billStatusRequest);
 
    // Add pending bills fetch for voucher UI
    Task<List<InvoiceAllocation>> GetPendingBillsAsync(int ledgerId);
}

