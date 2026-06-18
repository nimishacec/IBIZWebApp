using IBIZWebApp.Models;
namespace IBIZWebApp.Services;

public interface ICustomerService
{
    Task<PaginatedResult<Customer>> GetCustomersAsync(PaginationParams @params);
    Task<Customer?> GetCustomerByIdAsync(int customerId);
    Task<List<Customer>> SearchCustomersAsync(string searchTerm);
    Task<int> CreateCustomerAsync(Customer customer);
    Task<bool> UpdateCustomerAsync(Customer customer);
    Task<bool> DeleteCustomerAsync(int customerId);
}
