using Microsoft.JSInterop;

namespace IBIZWebApp.Services;

public class TokenService : ITokenService
{
    private readonly IJSRuntime _js;
    private const string Key = "authToken";

    public TokenService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SetTokenAsync(string token)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", Key, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _js.InvokeAsync<string?>("localStorage.getItem", Key);
    }

    public async Task RemoveTokenAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", Key);
    }
}
