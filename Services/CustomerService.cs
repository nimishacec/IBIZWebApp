using IBIZWebApp.Models;
namespace IBIZWebApp.Services;

public class CustomerService : ICustomerService
{
    private List<Customer> _customers = new();
    private int _nextId = 1;

    public CustomerService()
    {
        InitializeSampleData();
    }

    private void InitializeSampleData()
    {
        _customers = new List<Customer>
        {
            new Customer { CustomerId = 1, Name = "ABC Trading", Phone = "9876543210", Address = "123 Main St", GSTIN = "27AABCT1234A1Z5", CreditDays = 30 },
            new Customer { CustomerId = 2, Name = "XYZ Retail", Phone = "9876543211", Address = "456 Oak Ave", GSTIN = "27AABCT1234A1Z6", CreditDays = 15 },
            new Customer { CustomerId = 3, Name = "Quick Mart", Phone = "9876543212", Address = "789 Pine Rd", GSTIN = "27AABCT1234A1Z7", CreditDays = 0 },
            new Customer { CustomerId = 4, Name = "Super Store", Phone = "9876543213", Address = "321 Elm St", GSTIN = "27AABCT1234A1Z8", CreditDays = 45 },
            new Customer { CustomerId = 5, Name = "Mega Deals", Phone = "9876543214", Address = "654 Maple Dr", GSTIN = "27AABCT1234A1Z9", CreditDays = 60 }
        };
        _nextId = 6;
    }

    public Task<PaginatedResult<Customer>> GetCustomersAsync(PaginationParams @params)
    {
        var query = _customers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(@params.SearchQuery))
        {
            query = query.Where(c => 
                c.Name.Contains(@params.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(@params.SearchQuery, StringComparison.OrdinalIgnoreCase));
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(@params.SortBy))
        {
            query = @params.SortBy.ToLower() switch
            {
                "name" => @params.IsDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                "phone" => @params.IsDescending ? query.OrderByDescending(c => c.Phone) : query.OrderBy(c => c.Phone),
                "createddate" => @params.IsDescending ? query.OrderByDescending(c => c.CreatedDate) : query.OrderBy(c => c.CreatedDate),
                _ => query.OrderBy(c => c.CustomerId)
            };
        }
        else
        {
            query = query.OrderBy(c => c.CustomerId);
        }

        var totalCount = query.Count();
        var items = query.Skip(@params.SkipCount).Take(@params.PageSize).ToList();

        var result = new PaginatedResult<Customer>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = @params.PageNumber,
            PageSize = @params.PageSize
        };

        return Task.FromResult(result);
    }

    public Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        return Task.FromResult(_customers.FirstOrDefault(c => c.CustomerId == customerId));
    }

    public Task<List<Customer>> SearchCustomersAsync(string searchTerm)
    {
        var result = _customers
            .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       c.Phone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult(result);
    }

    public Task<int> CreateCustomerAsync(Customer customer)
    {
        customer.CustomerId = _nextId++;
        customer.CreatedDate = DateTime.Now;
        _customers.Add(customer);
        return Task.FromResult(customer.CustomerId);
    }

    public Task<bool> UpdateCustomerAsync(Customer customer)
    {
        var existing = _customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
        if (existing == null) return Task.FromResult(false);

        existing.Name = customer.Name;
        existing.Phone = customer.Phone;
        existing.Email = customer.Email;
        existing.Address = customer.Address;
        existing.GSTIN = customer.GSTIN;
        existing.CreditDays = customer.CreditDays;
        existing.ModifiedDate = DateTime.Now;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteCustomerAsync(int customerId)
    {
        var customer = _customers.FirstOrDefault(c => c.CustomerId == customerId);
        if (customer == null) return Task.FromResult(false);

        _customers.Remove(customer);
        return Task.FromResult(true);
    }
}
