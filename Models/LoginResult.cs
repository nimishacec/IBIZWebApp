
#nullable enable
namespace IBIZWebApp.Models;

public class LoginResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; }
    public string? UserId { get; set; }
}
public class DataRequest
{
    public string APPID { get; set; } = string.Empty;

    // Login ID sent by Mobile App
    public string UserId { get; set; } = string.Empty;

    public string? DeviceName { get; set; }
    public string? MobileNo { get; set; }
    public string? DataRow { get; set; }

    public int Status { get; set; } // 0 = pending, 1 = success, 2 = no data

    public string EntryDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    public string EntryTime { get; set; } = DateTime.Now.ToString("HH:mm:ss");
    public string? UpdatedDate { get; set; }
    public string? UpdatedTime { get; set; }

    public int SyncStatus { get; set; } // 0 = pending, 1 = success, 2 = no data

    public string? CreatedFrom { get; set; }

    public string? VchType { get; set; }

    // IMPORTANT: This stores JSON object/string
    public FilterConditionModel? FilterCondition { get; set; }
}

public class FilterConditionModel
{
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? VchType { get; set; }
    public string? PaymentMode { get; set; }
    // Add more fields when needed
}