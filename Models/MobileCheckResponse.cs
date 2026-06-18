
#nullable enable
using System.Text.Json.Serialization;
namespace IBIZWebApp.Models;

public class MobileCheckResponse
{
    [JsonPropertyName("companyCode")]
    public string? CompanyCode { get; set; }

    [JsonPropertyName("licenceKey")]
    public string? LicenceKey { get; set; }

    [JsonPropertyName("companyName")]
    public string? CompanyName { get; set; }
}
