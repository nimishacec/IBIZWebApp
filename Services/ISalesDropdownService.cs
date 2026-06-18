using IBIZWebApp.Models;

namespace IBIZWebApp.Services;

public interface ISalesDropdownService
{
    Task<List<DropdownItem>> GetSalesmenAsync();
    Task<List<DropdownItem>> GetCostCentresAsync();
    Task<List<DropdownItem>> GetAgentsAsync();
    Task<List<DropdownItem>> GetPriceListsAsync();
    Task<List<CustomerListItem>> GetCustomersAsync(string searchTerm = "");
}
