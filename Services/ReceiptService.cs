
using System.Net.Http.Json;
using IBIZWebApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using IBIZWebApp.Services;


namespace IBIZWebApp.Services;

public class ReceiptService : IReceiptService
{
    // Base URL for API endpoints (replace with real URL later)
    private const string BaseUrl = ApiConstants.BaseUrl;
    private readonly List<Receipt> _receipts;
    private int _nextId;
    private readonly ICompanySelectionService _companyService;

    public ReceiptService(ICompanySelectionService companyService)
    {
        _companyService = companyService;
        var today = DateTime.Today;
        _receipts = new List<Receipt>
        {
            // Today's receipts (Daily filter)
            new() { ReceiptId = 1, OrderId = 1001, CustomerName = "Rajesh Kumar", ReceiptNo = "RCP-000001", ReceiptDate = today, Amount = 15000m, Status = "Received", PaymentMode = "Cash", ReferenceNo = "", Narration = "Cash payment received", CreatedBy = "System", CreatedDate = today },
            new() { ReceiptId = 2, OrderId = 1002, CustomerName = "Priya Sharma", ReceiptNo = "RCP-000002", ReceiptDate = today, Amount = 28500.50m, Status = "Pending", PaymentMode = "Bank", ReferenceNo = "NEFT-98234", Narration = "Bank transfer pending confirmation", CreatedBy = "System", CreatedDate = today },
            new() { ReceiptId = 3, OrderId = 1003, CustomerName = "Amit Patel", ReceiptNo = "RCP-000003", ReceiptDate = today, Amount = 7200m, Status = "Received", PaymentMode = "Online", ReferenceNo = "UPI-44521", Narration = "UPI payment", CreatedBy = "System", CreatedDate = today },

            // Yesterday
            new() { ReceiptId = 4, OrderId = 1004, CustomerName = "Sandeep Verma", ReceiptNo = "RCP-000004", ReceiptDate = today.AddDays(-1), Amount = 42000m, Status = "Reconciled", PaymentMode = "Cheque", ReferenceNo = "CHQ-112233", Narration = "Cheque cleared", CreatedBy = "System", CreatedDate = today.AddDays(-1) },
            new() { ReceiptId = 5, OrderId = 1005, CustomerName = "Neha Gupta", ReceiptNo = "RCP-000005", ReceiptDate = today.AddDays(-1), Amount = 5500m, Status = "Received", PaymentMode = "Cash", ReferenceNo = "", Narration = "Walk-in cash payment", CreatedBy = "System", CreatedDate = today.AddDays(-1) },

            // This week
            new() { ReceiptId = 6, OrderId = 1006, CustomerName = "Vikram Singh", ReceiptNo = "RCP-000006", ReceiptDate = today.AddDays(-3), Amount = 18750m, Status = "Pending", PaymentMode = "Bank", ReferenceNo = "RTGS-55443", Narration = "Awaiting bank confirmation", CreatedBy = "System", CreatedDate = today.AddDays(-3) },
            new() { ReceiptId = 7, OrderId = 1007, CustomerName = "Deepa Nair", ReceiptNo = "RCP-000007", ReceiptDate = today.AddDays(-4), Amount = 33000m, Status = "Cancelled", PaymentMode = "Cheque", ReferenceNo = "CHQ-445566", Narration = "Cheque bounced", CreatedBy = "System", CreatedDate = today.AddDays(-4) },
            new() { ReceiptId = 8, OrderId = 1008, CustomerName = "Suresh Reddy", ReceiptNo = "RCP-000008", ReceiptDate = today.AddDays(-5), Amount = 12400m, Status = "Received", PaymentMode = "Online", ReferenceNo = "UPI-77889", Narration = "Google Pay payment", CreatedBy = "System", CreatedDate = today.AddDays(-5) },

            // Earlier this month
            new() { ReceiptId = 9, OrderId = 1009, CustomerName = "Kavita Joshi", ReceiptNo = "RCP-000009", ReceiptDate = today.AddDays(-10), Amount = 64000m, Status = "Reconciled", PaymentMode = "Bank", ReferenceNo = "NEFT-33221", Narration = "Quarterly payment", CreatedBy = "System", CreatedDate = today.AddDays(-10) },
            new() { ReceiptId = 10, OrderId = 1010, CustomerName = "Manoj Tiwari", ReceiptNo = "RCP-000010", ReceiptDate = today.AddDays(-12), Amount = 9800m, Status = "Received", PaymentMode = "Cash", ReferenceNo = "", Narration = "Partial payment", CreatedBy = "System", CreatedDate = today.AddDays(-12) },
            new() { ReceiptId = 11, OrderId = 1011, CustomerName = "Anjali Mehta", ReceiptNo = "RCP-000011", ReceiptDate = today.AddDays(-15), Amount = 21500m, Status = "Pending", PaymentMode = "Cheque", ReferenceNo = "CHQ-667788", Narration = "Post-dated cheque", CreatedBy = "System", CreatedDate = today.AddDays(-15) },
            new() { ReceiptId = 12, OrderId = 1012, CustomerName = "Rahul Saxena", ReceiptNo = "RCP-000012", ReceiptDate = today.AddDays(-18), Amount = 37250m, Status = "Received", PaymentMode = "Online", ReferenceNo = "IMPS-99001", Narration = "IMPS transfer", CreatedBy = "System", CreatedDate = today.AddDays(-18) },

            // Last month (Custom filter range)
            new() { ReceiptId = 13, OrderId = 1013, CustomerName = "Pooja Desai", ReceiptNo = "RCP-000013", ReceiptDate = today.AddDays(-35), Amount = 45000m, Status = "Reconciled", PaymentMode = "Bank", ReferenceNo = "NEFT-11223", Narration = "Full settlement", CreatedBy = "System", CreatedDate = today.AddDays(-35) },
            new() { ReceiptId = 14, OrderId = 1014, CustomerName = "Arun Mishra", ReceiptNo = "RCP-000014", ReceiptDate = today.AddDays(-40), Amount = 8600m, Status = "Cancelled", PaymentMode = "Cash", ReferenceNo = "", Narration = "Returned goods – receipt cancelled", CreatedBy = "System", CreatedDate = today.AddDays(-40) },
            new() { ReceiptId = 15, OrderId = 1015, CustomerName = "Sita Rao", ReceiptNo = "RCP-000015", ReceiptDate = today.AddDays(-45), Amount = 52300m, Status = "Received", PaymentMode = "Bank", ReferenceNo = "RTGS-22334", Narration = "Advance payment", CreatedBy = "System", CreatedDate = today.AddDays(-45) },
        };
        _nextId = 16;
    }
    public async Task<List<PendingBill>> GetPendingBillsAsync(int ledgerId)
    {
        using var httpClient = new HttpClient();
        var apiUrl = $"{BaseUrl}/api/Receipt/pending-bills/{ledgerId}";
        var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
        var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        if (!string.IsNullOrEmpty(cdkey))
            request.Headers.Add("CdyKey", cdkey);
        if (!string.IsNullOrEmpty(companyCode))
            request.Headers.Add("CompanyCode", companyCode);
        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var bills = await response.Content.ReadFromJsonAsync<List<PendingBill>>();
            return bills ?? new List<PendingBill>();
        }
        return new List<PendingBill>();
    }
    public async Task<ApiResponse<PaginatedResult<Receipt>>> GetReceiptsAsync(PaginationParams @params)
    {
        using var httpClient = new HttpClient();
        var apiUrl = $"{BaseUrl}/api/Receipt/GetReceipts?pageNumber={@params.PageNumber}&pageSize={@params.PageSize}";
        if (!string.IsNullOrWhiteSpace(@params.SearchQuery))
        {
            apiUrl += $"&searchQuery={Uri.EscapeDataString(@params.SearchQuery)}";
        }
        var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
        var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        if (!string.IsNullOrEmpty(cdkey))
            request.Headers.Add("CdyKey", cdkey);
        if (!string.IsNullOrEmpty(companyCode))
            request.Headers.Add("CompanyCode", companyCode);
        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<PaginatedResult<Receipt>>>();
            return apiResult ?? new ApiResponse<PaginatedResult<Receipt>> { Status = "error", Message = "No data", Data = null };
        }
        else
        {
            return new ApiResponse<PaginatedResult<Receipt>>
            {
                Status = "error",
                Message = $"Failed to fetch receipts: {response.ReasonPhrase}",
                Data = null
            };
        }
    }

    public async Task<ApiResponse<Receipt?>> GetReceiptByIdAsync(int receiptId)
    {
        using var httpClient = new HttpClient();
        var apiUrl = $"{BaseUrl}/api/Receipt/{receiptId}";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
        var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
        if (!string.IsNullOrEmpty(cdkey))
            request.Headers.Add("CdyKey", cdkey);
        if (!string.IsNullOrEmpty(companyCode))
            request.Headers.Add("CompanyCode", companyCode);
     

        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var receipt = await response.Content.ReadFromJsonAsync<Receipt>();
            return new ApiResponse<Receipt?>
            {
                Status = "success",
                Message = "Receipt found",
                Data = receipt
            };
        }
        else
        {
            return new ApiResponse<Receipt?>
            {
                Status = "error",
                Message = "Receipt not found",
                Data = null
            };
        }
    }

    public async Task<ApiResponse<PaginatedResult<Receipt>>> SearchReceiptsByDateAsync(DateTime from, DateTime to, int page, int pageSize)
    {
        using var httpClient = new HttpClient();
        var apiUrl = $"{BaseUrl}/api/Receipt/GetReceipts?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}&page={page}&pageSize={pageSize}";
        var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
        var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        if (!string.IsNullOrEmpty(cdkey))
            request.Headers.Add("CdyKey", cdkey);
        if (!string.IsNullOrEmpty(companyCode))
            request.Headers.Add("CompanyCode", companyCode);
        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var pagedResult = await response.Content.ReadFromJsonAsync<PaginatedResult<Receipt>>();
            return new ApiResponse<PaginatedResult<Receipt>>
            {
                Status = "success",
                Message = "Receipts filtered by date",
                Data = pagedResult
            };
        }
        else
        {
            return new ApiResponse<PaginatedResult<Receipt>>
            {
                Status = "error",
                Message = $"Failed to fetch receipts: {response.ReasonPhrase}",
                Data = null
            };
        }
    }

    public async Task<ApiResponse<List<Receipt>>> GetReceiptsByStatusAsync(string status)
    {
        await Task.Delay(300); // Simulate API latency
        var items = _receipts.Where(r => r.Status == status).ToList();
        return new ApiResponse<List<Receipt>>
        {
            Status = "success",
            Message = $"Receipts with status '{status}'",
            Data = items
        };
    }

    public async Task<bool> CreateReceiptAsync(Receipt receipt)
    {
        using var httpClient = new HttpClient();
        var apiUrl = $"{BaseUrl}/api/Receipt/CreateReceipt"; // Adjust endpoint as needed

        // Optionally set created date here or let the API handle it
        receipt.CreatedDate = DateTime.Now;

        // Remove Id property if present (for MongoDB compatibility)
        var receiptDict = new Dictionary<string, object?>();
        foreach (var prop in typeof(Receipt).GetProperties())
        {
            if (!string.Equals(prop.Name, "Id", StringComparison.OrdinalIgnoreCase))
            {
                receiptDict[prop.Name] = prop.GetValue(receipt);
            }
        }

        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
        {
            Content = JsonContent.Create(receiptDict)
        };

        // Add company headers as in ReceiptVoucherService
        var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
        var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
        if (!string.IsNullOrEmpty(cdkey))
            request.Headers.Add("CdyKey", cdkey);
        if (!string.IsNullOrEmpty(companyCode))
            request.Headers.Add("CompanyCode", companyCode);

        var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateReceiptAsync(Receipt receipt)
    {
        using var httpClient = new HttpClient();
        var apiUrl = $"{BaseUrl}/api/Receipt/{receipt.ReceiptId}";

        // Add company headers as in CreateReceiptAsync
        var request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
        {
            Content = JsonContent.Create(receipt)
        };
        var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
        var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
        if (!string.IsNullOrEmpty(cdkey))
            request.Headers.Add("CdyKey", cdkey);
        if (!string.IsNullOrEmpty(companyCode))
            request.Headers.Add("CompanyCode", companyCode);
        

        var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteReceiptAsync(int id)
    {
        using var httpClient = new HttpClient();
        var apiUrl = $"{BaseUrl}/api/Receipt/{id}";

        var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl);
        var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
        var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
        if (!string.IsNullOrEmpty(cdkey))
            request.Headers.Add("CdyKey", cdkey);
        if (!string.IsNullOrEmpty(companyCode))
            request.Headers.Add("CompanyCode", companyCode);

        var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}
