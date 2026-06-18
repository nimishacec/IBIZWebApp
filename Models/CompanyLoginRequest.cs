using System.Text.Json.Serialization;

namespace IBIZWebApp.Models;

public class CompanyLoginRequest
{
    [JsonPropertyName("CompanyCode")]
    public string CompanyCode { get; set; } = "";

    // API expects 'CdyKey'
    [JsonPropertyName("CdyKey")]
    public string CdyKey { get; set; } = "";

    [JsonPropertyName("UserName")]
    public string UserName { get; set; } = "";

    [JsonPropertyName("Password")]
    public string Password { get; set; } = "";

    // required by API
    [JsonPropertyName("APPID")]
    public string APPID { get; set; } = "";

    [JsonPropertyName("DeviceId")]
    public string DeviceId { get; set; } = "";
}