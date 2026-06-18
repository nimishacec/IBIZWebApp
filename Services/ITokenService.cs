namespace IBIZWebApp.Services;

public interface ITokenService
{
    Task SetTokenAsync(string token);
    Task<string?> GetTokenAsync();
    Task RemoveTokenAsync();
}
