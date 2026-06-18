using IBIZWebApp.Models;

namespace IBIZWebApp.Services;


public class SalesOrderService : ISalesOrderService
{
    private List<SalesOrder> _orders = new();
    private int _nextId = 1;

    public SalesOrderService()
    {
       
    }

   

    public Task<PaginatedResult<SalesOrder>> GetOrdersAsync(PaginationParams @params)
    {
        var query = _orders.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(@params.SearchQuery))
        {
            query = query.Where(o =>
                o.CustomerName?.Contains(@params.SearchQuery, StringComparison.OrdinalIgnoreCase) == true ||
                o.InvoiceNo?.Contains(@params.SearchQuery) == true);
        }

        if (!string.IsNullOrWhiteSpace(@params.SortBy))
        {
            query = @params.SortBy.ToLower() switch
            {
                "customername" => @params.IsDescending ? query.OrderByDescending(o => o.CustomerName) : query.OrderBy(o => o.CustomerName),
                "invdate" => @params.IsDescending ? query.OrderByDescending(o => o.InvDate) : query.OrderBy(o => o.InvDate),
                "netamount" => @params.IsDescending ? query.OrderByDescending(o => o.NetAmount) : query.OrderBy(o => o.NetAmount),
                "status" => @params.IsDescending ? query.OrderByDescending(o => o.Status) : query.OrderBy(o => o.Status),
                _ => query.OrderBy(o => o.OrderId)
            };
        }
        else
        {
            query = query.OrderByDescending(o => o.InvDate);
        }

        var totalCount = query.Count();
        var items = query.Skip(@params.SkipCount).Take(@params.PageSize).ToList();

        var result = new PaginatedResult<SalesOrder>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = @params.PageNumber,
            PageSize = @params.PageSize
        };

        return Task.FromResult(result);
    }

    public Task<SalesOrder?> GetOrderByIdAsync(string orderId)
    {
        return Task.FromResult(_orders.FirstOrDefault(o => o.OrderId == orderId));
    }

    public Task<List<SalesOrder>> SearchOrdersAsync(string searchTerm)
    {
        var result = _orders
            .Where(o => o.CustomerName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                       o.InvoiceNo?.Contains(searchTerm) == true)
            .ToList();

        return Task.FromResult(result);
    }

    public Task<string> CreateOrderAsync(SalesOrder order)
    {
        order.OrderId = _nextId.ToString();
        _nextId++;
        order.CreatedDate = DateTime.Now;
        _orders.Add(order);
        return Task.FromResult(order.OrderId);
    }

    public Task<bool> UpdateOrderAsync(SalesOrder order)
    {
        var existing = _orders.FirstOrDefault(o => o.OrderId == order.OrderId);
        if (existing == null) return Task.FromResult(false);

        existing.CustomerName = order.CustomerName;
        existing.InvDate = order.InvDate;
        existing.GrossAmount = order.GrossAmount;
        existing.NetAmount = order.NetAmount;
        existing.Status = order.Status;
        existing.ModifiedDate = DateTime.Now;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteOrderAsync(string orderId)
    {
        var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null) return Task.FromResult(false);

        _orders.Remove(order);
        return Task.FromResult(true);
    }

    public Task<List<SalesOrder>> GetOrdersByStatusAsync(string status)
    {
        var result = _orders
            .Where(o => o.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult(result);
    }

    public Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate)
    {
        var total = _orders
            .Where(o => o.InvDate >= startDate && o.InvDate <= endDate)
            .Sum(o => o.NetAmount);

        return Task.FromResult(total);
    }
}
