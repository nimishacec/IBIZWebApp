#nullable enable
using IBIZWebApp.Models;

namespace IBIZWebApp.Services;

/// <summary>
/// Service for sales order operations
/// </summary>
public interface ISalesOrderService
{
    Task<PaginatedResult<SalesOrder>> GetOrdersAsync(PaginationParams @params);
    Task<SalesOrder?> GetOrderByIdAsync(string orderId);
    Task<List<SalesOrder>> SearchOrdersAsync(string searchTerm);
    Task<string> CreateOrderAsync(SalesOrder order);
    Task<bool> UpdateOrderAsync(SalesOrder order);
    Task<bool> DeleteOrderAsync(string orderId);
    Task<List<SalesOrder>> GetOrdersByStatusAsync(string status);
    Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate);
}
