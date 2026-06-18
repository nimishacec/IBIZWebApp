using System.Text.Json.Serialization;

namespace IBIZWebApp.Models;

public class DropdownItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

// API model: { "empID": 1, "name": "DEFAULT EMPLOYEE" }
public class EmployeeApiItem
{
    [JsonPropertyName("empID")]
    public int EmpID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
}

// API model: { "ccid": 1, "ccName": "<MAIN>" }
public class CostCentreApiItem
{
    [JsonPropertyName("ccid")]
    public int CCID { get; set; }

    [JsonPropertyName("ccName")]
    public string CCName { get; set; } = "";
}

// API model: { "lid": 1, "lName": "Sales" }
public class LedgerApiItem
{
    [JsonPropertyName("lid")]
    public decimal? LID { get; set; }

    [JsonPropertyName("LName")]
    public string LName { get; set; } = "";

    [JsonPropertyName("MobileNo")]
    public string Mobile { get; set; } = "";

    [JsonPropertyName("Address")]
    public string Address { get; set; } = "";

    [JsonPropertyName("GSTIN")]
    public string GSTIN { get; set; } = "";

    [JsonPropertyName("OpBalance")]
    public decimal Balance { get; set; }
}

public class CustomerListItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
 [JsonPropertyName("lid")]
    public decimal LID { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("mobile")]
    public string Mobile { get; set; } = "";

    [JsonPropertyName("address")]
    public string Address { get; set; } = "";

    [JsonPropertyName("gstin")]
    public string GSTIN { get; set; } = "";

    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }
}
