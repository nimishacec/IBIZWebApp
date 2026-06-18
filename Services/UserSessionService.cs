using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace IBIZWebApp.Services;

public class UserSessionService
{
    private readonly IJSRuntime _jsRuntime;
    private const string UserIdKey = "UserId";
    private const string MobileNoKey = "MobileNo";

    public string? UserId { get; private set; }
    public string? MobileNo { get; private set; }

    public UserSessionService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        // Optionally, load MobileNo synchronously if needed (not possible with JSInterop)
    }

    public async Task SetUserIdAsync(string? userId)
    {
        UserId = userId;
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", UserIdKey, userId ?? "");
    }

    public async Task LoadUserIdAsync()
    {
        var userId = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", UserIdKey);
        UserId = string.IsNullOrEmpty(userId) ? null : userId;
    }

    public async Task SetMobileNoAsync(string? mobileNo)
    {
        MobileNo = mobileNo;
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", MobileNoKey, mobileNo ?? "");
    }

    public async Task LoadMobileNoAsync()
    {
        var mobileNo = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", MobileNoKey);
        MobileNo = string.IsNullOrEmpty(mobileNo) ? null : mobileNo;
    }

    public async Task ClearUserIdAsync()
    {
        UserId = null;
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", UserIdKey);
        MobileNo = null;
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", MobileNoKey);
    }
}
