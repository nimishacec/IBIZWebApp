
#nullable enable
using IBIZWebApp.Models;
namespace IBIZWebApp.Services;

public interface ISalesItemService
{
    Task<PaginatedResult<SalesItem>> GetItemsAsync(PaginationParams @params);
    Task<SalesItem?> GetItemByIdAsync(string itemCode);
    Task<List<SalesItem>> GetAllItemsAsync();
    Task<List<SalesItem>> SearchItemsAsync(string searchTerm);
    Task<int> CreateItemAsync(SalesItem item);
    Task<bool> UpdateItemAsync(SalesItem item);
    Task<bool> DeleteItemAsync(string itemCode);
}
