using System.Net.Http.Json;
using System.Net.Http.Headers;
using IBIZWebApp.Models;

namespace IBIZWebApp.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private const string AppID = "GFW79L496PM4557UD3KEERR0W";

    public AuthenticationService(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    public async Task<AuthenticationResult> GetOTPAsync(string mobileNumber)
    {
        try
        {
            var url = $"{ApiConstants.BaseUrl}/api/MobileStatus/checkmobileregn?mobileNo={Uri.EscapeDataString(mobileNumber)}&APPID={AppID}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var raw = await response.Content.ReadAsStringAsync();
                // Try deserialize to AuthenticationResult
                try
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<AuthenticationResult>(raw, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (result != null) return result;
                }
                catch { }

                // Try alternate response shape: { isRegistered: bool, message: string }
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(raw);
                    var root = doc.RootElement;
                    if (root.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        if (root.TryGetProperty("isRegistered", out var isRegElem))
                        {
                            var isRegistered = isRegElem.GetBoolean();
                            var message = root.TryGetProperty("message", out var msgElem) ? msgElem.GetString() ?? raw : raw;
                            return new AuthenticationResult { IsSuccess = isRegistered, Message = message };
                        }
                        else
                        {
                            // Log available properties for debugging
                            var props = string.Join(", ", root.EnumerateObject().Select(p => p.Name));
                            return new AuthenticationResult { IsSuccess = false, Message = $"'isRegistered' property not found. Available properties: {props}. Raw: {raw}" };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new AuthenticationResult { IsSuccess = false, Message = $"Failed to parse response: {ex.Message}. Raw: {raw}" };
                }

                // Fallback: return raw as message and treat as success
                return new AuthenticationResult { IsSuccess = true, Message = raw ?? "OTP sent successfully" };
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new AuthenticationResult { IsSuccess = false, Message = "Mobile number not registered" };
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                return new AuthenticationResult { IsSuccess = false, Message = $"Server returned {(int)response.StatusCode} {response.StatusCode}: {errorBody}" };
            }
        }
        catch (Exception ex)
        {
            return new AuthenticationResult { IsSuccess = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<AuthenticationResult> VerifyOTPAsync(string mobileNumber, string otp, string deviceId)
    {
        try
        {
            // Send JSON body with API-expected field names: mobileNos, otp, deviceId, appid
            var requestBody = new 
            { 
                mobileNos = mobileNumber,
                otp,
                deviceId = deviceId ?? "TP1A.220624.014",
                appid = AppID
            };
            var response = await _httpClient.PostAsJsonAsync($"{ApiConstants.BaseUrl}/api/MobileStatus/setmobileregnstatus", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var raw = await response.Content.ReadAsStringAsync();

                // Try alternate shapes: { response: [...], message: ... }
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(raw);
                    var root = doc.RootElement;

                    var authResult = new AuthenticationResult();

                    if (root.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        // Get message at top level
                        if (root.TryGetProperty("message", out var msgElem))
                        {
                            authResult.Message = msgElem.GetString();
                        }

                        // Check for "response" array (lowercase)
                        if (root.TryGetProperty("response", out var responseElem) && responseElem.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            var comps = System.Text.Json.JsonSerializer.Deserialize<List<MobileCheckResponse>>(responseElem.GetRawText(), new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            authResult.Companies = comps;
                            if (comps != null && comps.Count > 0) authResult.IsSuccess = true;
                            return authResult;
                        }

                        // Check for old Response property wrapper
                        if (root.TryGetProperty("Response", out var respElem))
                        {
                            var innerRoot = respElem;
                            if (innerRoot.ValueKind == System.Text.Json.JsonValueKind.Object)
                            {
                                if (innerRoot.TryGetProperty("isRegistered", out var isRegElem))
                                {
                                    authResult.IsSuccess = isRegElem.GetBoolean();
                                }

                                if (innerRoot.TryGetProperty("matchedCompanies", out var compsElem) && compsElem.ValueKind == System.Text.Json.JsonValueKind.Array)
                                {
                                    var comps = System.Text.Json.JsonSerializer.Deserialize<List<MobileCheckResponse>>(compsElem.GetRawText(), new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                    authResult.Companies = comps;
                                    if (comps != null && comps.Count > 0) authResult.IsSuccess = true;
                                    return authResult;
                                }

                                if (innerRoot.TryGetProperty("companyCode", out var compCodeElem))
                                {
                                    var comp = new MobileCheckResponse
                                    {
                                        CompanyCode = compCodeElem.GetString(),
                                        LicenceKey = innerRoot.TryGetProperty("cdKey", out var cdKeyElem) ? cdKeyElem.GetString() : innerRoot.TryGetProperty("licenceKey", out var lk) ? lk.GetString() : null,
                                        CompanyName = innerRoot.TryGetProperty("companyName", out var cn) ? cn.GetString() : null
                                    };
                                    authResult.Companies = new List<MobileCheckResponse> { comp };
                                    authResult.IsSuccess = true;
                                    return authResult;
                                }
                            }
                        }
                    }

                    if (authResult.Message != null || authResult.Companies != null)
                    {
                        return authResult;
                    }
                }
                catch { }

                // If nothing matched, return raw for debugging but mark failure
                return new AuthenticationResult { IsSuccess = false, Message = $"Failed to parse response: {raw}" };
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new AuthenticationResult { IsSuccess = false, Message = $"Invalid OTP or server error: {response.StatusCode} - {error}" };
            }
        }
        catch (Exception ex)
        {
            return new AuthenticationResult { IsSuccess = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<AuthenticationResult> ConnectCompanyAsync(string companyCode, string cdKey)
    {
        try
        {
            // connection status is a GET request with query params
            var url = $"{ApiConstants.BaseUrl}/api/Company/ConnectionStatus?CompanyCode={Uri.EscapeDataString(companyCode)}&CDKey={Uri.EscapeDataString(cdKey)}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var raw = await response.Content.ReadAsStringAsync();

                // first check for the { result, Message } form which our model doesn't cover
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(raw);
                    var root = doc.RootElement;
                    if (root.ValueKind == System.Text.Json.JsonValueKind.Object &&
                        root.TryGetProperty("result", out var resElem))
                    {
                        bool ok = resElem.GetBoolean();
                        var msg = root.TryGetProperty("Message", out var msgElem) ? msgElem.GetString() : raw;
                        return new AuthenticationResult { IsSuccess = ok, Message = msg };
                    }
                }
                catch { /* ignore parse errors */ }

                // fall back to deserializing into AuthenticationResult (isRegistered, matchedCompanies, etc.)
                try
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<AuthenticationResult>(raw, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (result != null) return result;
                }
                catch { }

                // final fallback: success with raw message
                return new AuthenticationResult { IsSuccess = true, Message = raw };
            }
            else
            {
                var err = await response.Content.ReadAsStringAsync();
                return new AuthenticationResult { IsSuccess = false, Message = $"Connection check failed: {err}" };
            }
        }
        catch (Exception ex)
        {
            return new AuthenticationResult { IsSuccess = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<LoginResult> CompanyLoginAsync(CompanyLoginRequest request)
    {
        try
        {
            // Use the new user login endpoint which returns a token
            // ensure API-required fields are populated
            request.APPID = AppID;
           // request.DeviceId = "TP1A.220624.014";

            // prepare message manually so we can include headers
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{ApiConstants.BaseUrl}/api/User/UserLogin");
            // add company code and cdkey headers as requested
            if (!string.IsNullOrEmpty(request.CompanyCode))
                httpRequest.Headers.Add("CompanyCode", request.CompanyCode);
            if (!string.IsNullOrEmpty(request.CdyKey))
                httpRequest.Headers.Add("CdyKey", request.CdyKey);
            httpRequest.Content = JsonContent.Create(request);

            var response = await _httpClient.SendAsync(httpRequest);
            var raw = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Try direct model bind first
                try
                {
                    var lr = System.Text.Json.JsonSerializer.Deserialize<LoginResult>(raw, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (lr != null)
                    {
                        if (!lr.Success && !string.IsNullOrEmpty(lr.Token)) lr.Success = true;
                        if (!string.IsNullOrEmpty(lr.Token))
                        {
                            await _tokenService.SetTokenAsync(lr.Token);
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lr.Token);
                            // propagate company details as default headers for future requests
                            if (!string.IsNullOrEmpty(request.CompanyCode))
                            {
                                _httpClient.DefaultRequestHeaders.Remove("CompanyCode");
                                _httpClient.DefaultRequestHeaders.Add("CompanyCode", request.CompanyCode);
                            }
                            if (!string.IsNullOrEmpty(request.CdyKey))
                            {
                                _httpClient.DefaultRequestHeaders.Remove("CDKey");
                                _httpClient.DefaultRequestHeaders.Add("CdyKey", request.CdyKey);
                            }
                        }
                        return lr;
                    }
                }
                catch { }

                // Try to find token in common JSON shapes
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(raw);
                    var root = doc.RootElement;

                    if (root.ValueKind == System.Text.Json.JsonValueKind.String)
                    {
                        // response is a bare string token
                        var token = root.GetString();
                        if (!string.IsNullOrEmpty(token))
                        {
                            await _tokenService.SetTokenAsync(token);
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                        return new LoginResult { Success = true, Token = token };
                    }

                    if (root.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        // token property
                        if (root.TryGetProperty("token", out var t) || root.TryGetProperty("Token", out t))
                        {
                            var token = t.GetString();
                            if (!string.IsNullOrEmpty(token))
                            {
                                await _tokenService.SetTokenAsync(token);
                                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            }
                            return new LoginResult { Success = true, Token = token };
                        }

                        // sometimes wrapped in 'response'
                        if (root.TryGetProperty("response", out var resp) && resp.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            if (resp.TryGetProperty("token", out var rt))
                            {
                                var token = rt.GetString();
                                if (!string.IsNullOrEmpty(token))
                                {
                                    await _tokenService.SetTokenAsync(token);
                                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                }
                                return new LoginResult { Success = true, Token = token };
                            }
                        }

                        // fallback: return success with raw message so UI shows server text
                        return new LoginResult { Success = true, Message = raw };
                    }
                }
                catch { }

                // final fallback
                return new LoginResult { Success = true, Message = raw };
            }
            else
            {
                return new LoginResult { Success = false, Message = raw };
            }
        }
        catch (Exception ex)
        {
            return new LoginResult { Success = false, Message = $"Error: {ex.Message}" };
        }
    }
}
