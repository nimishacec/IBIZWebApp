using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using IBIZWebApp.Models;
using Microsoft.JSInterop;
using static IBIZWebApp.ApiConstants;

namespace IBIZWebApp.Services;

public class SalesDropdownService : ISalesDropdownService
{
    private readonly HttpClient _http;
    private readonly ICompanySelectionService _companyService;
    private readonly ITokenService _tokenService;
    private readonly IJSRuntime _js;
    private const string BaseUrl = ApiConstants.BaseUrl;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public SalesDropdownService(HttpClient http, ICompanySelectionService companyService, ITokenService tokenService, IJSRuntime js)
    {
        _http = http;
        _companyService = companyService;
        _tokenService = tokenService;
        _js = js;
    }

    public async Task<List<DropdownItem>> GetSalesmenAsync()
    {
        // API returns: [{ "empID": 1, "name": "DEFAULT EMPLOYEE" }, ...]
        var apiItems = await FetchAsync<List<EmployeeApiItem>>($"{BaseUrl}/api/Order/GetEmployees");
        return apiItems?.Select(x => new DropdownItem { Id = x.EmpID, Name = x.Name }).ToList() ?? new List<DropdownItem>();
    }

    public async Task<List<DropdownItem>> GetCostCentresAsync()
    {
        // API returns: [{ "ccid": 1, "ccName": "<MAIN>" }, ...]
        var apiItems = await FetchAsync<List<CostCentreApiItem>>($"{BaseUrl}/api/Order/GetCostCentres");
        return apiItems?.Select(x => new DropdownItem { Id = x.CCID, Name = x.CCName }).ToList() ?? new List<DropdownItem>();
    }

    public async Task<List<DropdownItem>> GetAgentsAsync()
    {
        // API returns: [{ "agentId": 1, "name": "Default Agent" }, ...]
        var apiItems = await FetchAsync<List<AgentApiItem>>($"{BaseUrl}/api/Order/GetAgents");
        return apiItems?.Select(x => new DropdownItem { Id = x.AgentID, Name = x.AgentName }).ToList() ?? new List<DropdownItem>();
    }

    public async Task<List<DropdownItem>> GetPriceListsAsync()
    {
        // API returns: [{ "plid": 1, "priceListName": "Rate1" }, ...]
        var apiItems = await FetchAsync<List<PriceListApiItem>>($"{BaseUrl}/api/Order/GetPriceList");
        return apiItems?.Select(x => new DropdownItem { Id = x.PLID, Name = x.PriceListName }).ToList() ?? new List<DropdownItem>();
    }
// Add API item classes for mapping
public class AgentApiItem
{
    [JsonPropertyName("agentID")]

    public int AgentID { get; set; }
    [JsonPropertyName("agentName")]

    public string AgentName { get; set; } = string.Empty;
}

public class PriceListApiItem
{
      [JsonPropertyName("pliD")]
    public int PLID { get; set; }
    [JsonPropertyName("priceListName")]
    public string PriceListName { get; set; } = string.Empty;
}

    public async Task<List<CustomerListItem>> GetCustomersAsync(string searchTerm = "")
    {
        // API returns: [{ "lid": 1, "lName": "Sales" }, ...]
        var url = string.IsNullOrWhiteSpace(searchTerm)
            ? $"{BaseUrl}/api/Order/GetLedgers"
            : $"{BaseUrl}/api/Order/GetLedgers?search={Uri.EscapeDataString(searchTerm)}";
        var apiItems = await FetchAsync<List<LedgerApiItem>>(url);
        return apiItems?.Select(x => new CustomerListItem {
            LID = (x.LID ?? 0),
            Name = x.LName,
            Mobile = x.Mobile, // Fix: use correct property from LedgerApiItem
            Address = x.Address,
            GSTIN = x.GSTIN,
            Balance = x.Balance
        }).ToList() ?? new List<CustomerListItem>();
    }

    private async Task<T?> FetchAsync<T>(string url)
    {
        try
        {
            // Ensure SelectedCompany is loaded from sessionStorage (fire-and-forget in constructor may not have finished)
            if (_companyService.SelectedCompany == null)
                await _companyService.LoadSelectedCompanyAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var cdkey = _companyService.SelectedCompany?.LicenceKey ?? string.Empty;
            var companyCode = _companyService.SelectedCompany?.CompanyCode ?? string.Empty;
            if (!string.IsNullOrEmpty(cdkey))
                request.Headers.Add("CdyKey", cdkey);
            if (!string.IsNullOrEmpty(companyCode))
                request.Headers.Add("CompanyCode", companyCode);

            await _js.InvokeVoidAsync("console.log", $"API [{url}] CdyKey={cdkey}, CompanyCode={companyCode}");

            var response = await _http.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                await _js.InvokeVoidAsync("console.log", $"API [{url}] response:", json?.Length > 500 ? json.Substring(0, 500) + "..." : json);
                return json != null ? JsonSerializer.Deserialize<T>(json, JsonOptions) : default;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await _js.InvokeVoidAsync("console.error", $"API [{url}] HTTP {(int)response.StatusCode}:", error);
            }
        }
        catch (Exception ex)
        {
            await _js.InvokeVoidAsync("console.error", $"API [{url}] error:", ex.Message);
        }
        return default;
    }

    private async Task<List<DropdownItem>> FetchDropdownAsync(string url)
    {
        var result = await FetchAsync<List<DropdownItem>>(url);
        return result ?? new List<DropdownItem>();
    }
}
