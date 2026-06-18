       
using System.Net.Http.Json;
using System.Text.Json;
using IBIZWebApp.Models;
using Microsoft.JSInterop;

namespace IBIZWebApp.Services;

public class ReceiptVoucherService : IReceiptVoucherService
{
    private readonly HttpClient _http;
    private readonly ICompanySelectionService _companyService;
    private readonly IJSRuntime _js;
    private readonly ITokenService _tokenService;
    private const string BaseUrl = ApiConstants.BaseUrl;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ReceiptVoucherService(HttpClient http, ICompanySelectionService companyService, IJSRuntime js)
    {
        _http = http;
        _companyService = companyService;
        _js = js;
    }

    public ReceiptVoucherService(HttpClient http, ICompanySelectionService companyService, IJSRuntime js, ITokenService tokenService)
    {
        _http = http;
        _companyService = companyService;
        _js = js;
        _tokenService = tokenService;
    }

    public async Task<string> GenerateVoucherNoAsync()
    {
        var result = await FetchAsync<string>($"{BaseUrl}/api/Receipt/GenerateVoucherNo");
        return result ?? $"RV-{DateTime.Now:yyyyMMddHHmmss}";
    }

    public async Task<List<CostCentre>> GetCostCentresAsync()
    {
        // Reuse the same cost centres endpoint as Order module
        var apiItems = await FetchAsync<List<CostCentreApiItem>>($"{BaseUrl}/api/Order/GetCostCentres");
        return apiItems?.Select(x => new CostCentre { CostCentreId = x.CCID, Name = x.CCName }).ToList()
            ?? new List<CostCentre>();
    }
   public async Task<int> SaveInvoiceAllocationSettlementAsync(AgeingSettlement settlement)
    {
        await EnsureCompanyLoaded();
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/Receipt/SaveInvoiceAllocationSettlement");
        AddCompanyHeaders(request);
        var token = await _tokenService.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(settlement), System.Text.Encoding.UTF8, "application/json");
        var response = await _http.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            if (int.TryParse(json.Trim().Trim('"'), out var id))
                return id;
            return 1;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await _js.InvokeVoidAsync("console.error", $"SaveInvoiceAllocationSettlement HTTP {(int)response.StatusCode}:", error);
            throw new Exception($"SaveInvoiceAllocationSettlement failed: {response.StatusCode}");
        }
    }
    public async Task<List<LedgerOption>> GetCashBankLedgersAsync()
    {
        var result = await FetchAsync<List<LedgerOption>>($"{BaseUrl}/api/Order/GetDebitLedgers");
        return result ?? new List<LedgerOption>();
    }
        public async Task<List<InvoiceAllocation>> GetPendingBillsAsync(int ledgerId)
    {
        var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
        var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
        var url = $"{BaseUrl}/api/Receipt/pending-bills/{ledgerId}";
        await _js.InvokeVoidAsync("console.log", $"[GetPendingBillsAsync] About to call API with URL: {url}, Request Headers: {{ CdyKey: {cdkey}, CompanyCode: {companyCode} }}");
        await _js.InvokeVoidAsync("console.log", $"[GetPendingBillsAsync] URL: {url}, CdyKey: {cdkey}, CompanyCode: {companyCode}");
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (!string.IsNullOrEmpty(cdkey))
            request.Headers.Add("CdyKey", cdkey);
        if (!string.IsNullOrEmpty(companyCode))
            request.Headers.Add("CompanyCode", companyCode);
        var response = await _http.SendAsync(request);
        await _js.InvokeVoidAsync("console.log", $"[GetPendingBillsAsync] HTTP Status: {response.StatusCode}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            await _js.InvokeVoidAsync("console.log", $"[GetPendingBillsAsync] Response JSON: {json}");
            var bills = System.Text.Json.JsonSerializer.Deserialize<List<InvoiceAllocation>>(json, JsonOptions);
            await _js.InvokeVoidAsync("console.log", $"[GetPendingBillsAsync] Parsed Bills Count: {bills?.Count ?? 0}");
            return (bills ?? new List<InvoiceAllocation>()).Select(b => new InvoiceAllocation
            {
                InvoiceId = b.InvoiceId,
                InvoiceNo = b.InvoiceNo, // or b.BillNo if that's your property
                InvoiceDate = b.InvoiceDate,
                CreditDays = b.CreditDays,
                BillAmount = b.BillAmount,
                CurrentAmount = b.CurrentAmount,
                SaAmount = b.CurrentAmount,
                Order = 1
            }).ToList();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await _js.InvokeVoidAsync("console.error", $"[GetPendingBillsAsync] Error: {error}");
        }
        return new List<InvoiceAllocation>();
    }

    public async Task<List<LedgerOption>> GetCustomerLedgersAsync()
    {
        // Reuse the same ledgers endpoint as Order module
        var apiItems = await FetchAsync<List<LedgerApiItem>>($"{BaseUrl}/api/Order/GetLedgers");
        return apiItems?.Select(x => new LedgerOption
        {
            LedgerId = (int)(x.LID ?? 0),
            LedgerName = x.LName,
            LedgerType = "Customer"
        }).ToList() ?? new List<LedgerOption>();
    }

    public async Task<decimal> GetLedgerBalanceAsync(int ledgerId)
    {
        var result = await FetchAsync<IBIZWebApp.Models.LedgerBalanceResponse>($"{BaseUrl}/api/Receipt/GetLedgerBalance/{ledgerId}");
        return result?.Balance ?? 0;
    }

    public async Task<List<InvoiceAllocation>> GetOpenInvoicesAsync(int ledgerId)
    {
        var result = await FetchAsync<List<IBIZWebApp.Models.InvoiceAllocation>>($"{BaseUrl}/api/Receipt/GetOpenInvoices?ledgerId={ledgerId}");
        return result ?? new List<InvoiceAllocation>();
    }
    public async Task<bool> UpdateLedgerBillStatusListAsync(BillAllocationRequest billStatusRequest)
    {
        await EnsureCompanyLoaded();
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/Receipt/UpdateLedgerBillStatus");
        AddCompanyHeaders(request);
        //var token = await _tokenService.GetTokenAsync();
        
        request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(billStatusRequest), System.Text.Encoding.UTF8, "application/json");
        var response = await _http.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await _js.InvokeVoidAsync("console.error", $"UpdateLedgerBillStatus HTTP {(int)response.StatusCode}:", error);
            return false;
        }
    }
    
    public async Task<List<ReceiptVoucher>> GetReceiptsByDateAsync(DateTime date)
    {
        var dateStr = date.ToString("yyyy-MM-dd");
        var result = await FetchAsync<List<ReceiptVoucher>>($"{BaseUrl}/api/Receipt/GetByDate?date={dateStr}");
        return result ?? new List<ReceiptVoucher>();
    }

    public async Task<ReceiptVoucher?> GetReceiptVoucherByIdAsync(int voucherId)
    {
        return await FetchAsync<ReceiptVoucher>($"{BaseUrl}/api/Receipt/GetVoucher/{voucherId}");
    }

        
    public async Task<int> SaveReceiptVoucherAsync(ReceiptVoucher voucher)
    {
        try
        {
            await EnsureCompanyLoaded();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/App/UploadDataRequest");
            AddCompanyHeaders(request);

            // Add bearer token to Authorization header
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // Only include allocations with SaAmount > 0
            voucher.Allocations = voucher.Allocations.Where(a => a.SaAmount > 0).ToList();

            // Wrap voucher in SalesJsonRequest as FilteredJsonData
            var filteredJson = System.Text.Json.JsonSerializer.Serialize(voucher, JsonOptions);
            var body = new SalesJsonRequest
            {
                Id = string.Empty,
                JsonData = string.Empty,
                FilteredJsonData = filteredJson
            };
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json");

            var response = await _http.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                await _js.InvokeVoidAsync("console.log", "SaveVoucher response:", json);

                // Try to parse returned voucher ID
                if (int.TryParse(json.Trim().Trim('"'), out var id))
                    return id;

                var result = JsonSerializer.Deserialize<JsonElement>(json, JsonOptions);
                if (result.TryGetProperty("voucherId", out var vid))
                    return vid.GetInt32();

                return 1; // success but no ID returned
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await _js.InvokeVoidAsync("console.error", $"SaveVoucher HTTP {(int)response.StatusCode}:", error);
                throw new Exception($"Save failed: {response.StatusCode}");
            }
        }
        catch (Exception ex) when (ex.Message.StartsWith("Save failed"))
        {
            throw;
        }
        catch (Exception ex)
        {
            await _js.InvokeVoidAsync("console.error", "SaveVoucher error:", ex.Message);
            throw;
        }
    }

    private async Task EnsureCompanyLoaded()
    {
        if (_companyService.SelectedCompany == null)
            await _companyService.LoadSelectedCompanyAsync();
    }

    private void AddCompanyHeaders(HttpRequestMessage request)
    {
        var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
        var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
        if (!string.IsNullOrEmpty(cdkey))
            request.Headers.Add("CdyKey", cdkey);
        if (!string.IsNullOrEmpty(companyCode))
            request.Headers.Add("CompanyCode", companyCode);
        // Add VchrType and IsSync headers for receipt APIs
        request.Headers.Add("VchrType", "RECEIPT");
        request.Headers.Add("IsSync", "false");
    }

    private async Task<T?> FetchAsync<T>(string url)
    {
        try
        {
            await EnsureCompanyLoaded();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddCompanyHeaders(request);

            await _js.InvokeVoidAsync("console.log", $"Receipt API [{url}]");

            var response = await _http.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                await _js.InvokeVoidAsync("console.log", $"Receipt API [{url}] response:", json?.Length > 500 ? json.Substring(0, 500) + "..." : json);
                return json != null ? JsonSerializer.Deserialize<T>(json, JsonOptions) : default;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await _js.InvokeVoidAsync("console.error", $"Receipt API [{url}] HTTP {(int)response.StatusCode}:", error);
            }
        }
        catch (Exception ex)
        {
            await _js.InvokeVoidAsync("console.error", $"Receipt API [{url}] error:", ex.Message);
        }
        return default;
    }
}
