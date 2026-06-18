
using IBIZWebApp.Models;
#nullable enable
using System.Text.Json.Serialization;
namespace IBIZWebApp.Models;

public class AuthenticationResult
{
    [JsonPropertyName("isRegistered")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    public string? Token { get; set; }

    [JsonPropertyName("matchedCompanies")]
    public List<MobileCheckResponse>? Companies { get; set; }
}
