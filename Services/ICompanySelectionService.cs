using IBIZWebApp.Models;

namespace IBIZWebApp.Services;

public interface ICompanySelectionService
{
    List<MobileCheckResponse>? Companies { get; set; }
    MobileCheckResponse? SelectedCompany { get; set; }
    Task SetSelectedCompanyAsync(MobileCheckResponse? company);
    Task SetCompaniesAsync(List<MobileCheckResponse>? companies);
    Task LoadCompaniesAsync();
    Task LoadSelectedCompanyAsync();
}