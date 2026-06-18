
using IBIZWebApp.Models;

namespace IBIZWebApp.Services;

public class SalesItemService : ISalesItemService
{
    private List<SalesItem> _items = new();

    public SalesItemService()
    {
        InitializeSampleData();
    }

    private void InitializeSampleData()
    {
        _items = new List<SalesItem>
        {
            new SalesItem { ItemCode = "HARMIC-PW", ItemName = "HARMIC PWR 500ML", Rate = 107, MRP = 110, TaxPercent = 12, StockQuantity = 100 },
            new SalesItem { ItemCode = "PROD-002", ItemName = "Product 002", Rate = 150, MRP = 160, TaxPercent = 5, StockQuantity = 50 },
            new SalesItem { ItemCode = "PROD-003", ItemName = "Product 003", Rate = 200, MRP = 220, TaxPercent = 18, StockQuantity = 75 },
            new SalesItem { ItemCode = "PROD-004", ItemName = "Product 004", Rate = 50, MRP = 55, TaxPercent = 5, StockQuantity = 200 },
            new SalesItem { ItemCode = "PROD-005", ItemName = "Product 005", Rate = 300, MRP = 330, TaxPercent = 12, StockQuantity = 30 }
        };
    }

    public Task<PaginatedResult<SalesItem>> GetItemsAsync(PaginationParams @params)
    {
        var query = _items.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(@params.SearchQuery))
        {
            query = query.Where(i => 
                i.ItemCode.Contains(@params.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                i.ItemName.Contains(@params.SearchQuery, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = query.Count();
        var items = query.Skip(@params.SkipCount).Take(@params.PageSize).ToList();

        var result = new PaginatedResult<SalesItem>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = @params.PageNumber,
            PageSize = @params.PageSize
        };

        return Task.FromResult(result);
    }

    public Task<SalesItem?> GetItemByIdAsync(string itemCode)
    {
        return Task.FromResult(_items.FirstOrDefault(i => i.ItemCode == itemCode));
    }

    public Task<List<SalesItem>> GetAllItemsAsync()
    {
        return Task.FromResult(_items.Where(i => i.IsActive).ToList());
    }

    public Task<List<SalesItem>> SearchItemsAsync(string searchTerm)
    {
        var result = _items
            .Where(i => (i.ItemCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        i.ItemName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) &&
                       i.IsActive)
            .ToList();

        return Task.FromResult(result);
    }

    public Task<int> CreateItemAsync(SalesItem item)
    {
        _items.Add(item);
        return Task.FromResult(1);
    }

    public Task<bool> UpdateItemAsync(SalesItem item)
    {
        var existing = _items.FirstOrDefault(i => i.ItemCode == item.ItemCode);
        if (existing == null) return Task.FromResult(false);

        existing.ItemName = item.ItemName;
        existing.Rate = item.Rate;
        existing.MRP = item.MRP;
        existing.TaxPercent = item.TaxPercent;
        existing.StockQuantity = item.StockQuantity;
        existing.IsActive = item.IsActive;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteItemAsync(string itemCode)
    {
        var item = _items.FirstOrDefault(i => i.ItemCode == itemCode);
        if (item == null) return Task.FromResult(false);

        item.IsActive = false; // Soft delete
        return Task.FromResult(true);
    }
}
