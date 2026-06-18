using IBIZWebApp.Models;
using Microsoft.JSInterop;
using System.Text.Json;

#nullable enable
using System.Threading.Tasks;
namespace IBIZWebApp.Services;

public class CompanySelectionService : ICompanySelectionService
{
    private readonly IJSRuntime _jsRuntime;
    private const string SelectedCompanyKey = "SelectedCompany";
    private const string CompaniesKey = "Companies";

    public List<MobileCheckResponse>? Companies { get; set; }
    public MobileCheckResponse? SelectedCompany { get; set; }

    public CompanySelectionService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        // Try to load selected company and companies from storage on service creation
        _ = LoadSelectedCompanyAsync();
        _ = LoadCompaniesAsync();
    }

    public async Task SetSelectedCompanyAsync(MobileCheckResponse? company)
    {
        SelectedCompany = company;
        if (company != null)
        {
            var json = JsonSerializer.Serialize(company);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", SelectedCompanyKey, json);
        }
        else
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", SelectedCompanyKey);
        }
    }

    public async Task SetCompaniesAsync(List<MobileCheckResponse>? companies)
    {
        Companies = companies;
        if (companies != null)
        {
            var json = JsonSerializer.Serialize(companies);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", CompaniesKey, json);
        }
        else
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", CompaniesKey);
        }
    }

    public async Task LoadCompaniesAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", CompaniesKey);
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                Companies = JsonSerializer.Deserialize<List<MobileCheckResponse>>(json);
            }
            catch
            {
                Companies = null;
            }
        }
        else
        {
            Companies = null;
        }
    }

    public async Task LoadSelectedCompanyAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", SelectedCompanyKey);
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                SelectedCompany = JsonSerializer.Deserialize<MobileCheckResponse>(json);
            }
            catch
            {
                SelectedCompany = null;
            }
        }
        else
        {
            SelectedCompany = null;
        }
    }
}