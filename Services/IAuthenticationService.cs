using IBIZWebApp.Models;


namespace IBIZWebApp.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResult> GetOTPAsync(string mobileNumber);
    Task<AuthenticationResult> VerifyOTPAsync(string mobileNumber, string otp, string deviceId);

    // establishes DB connection for selected company
    Task<AuthenticationResult> ConnectCompanyAsync(string companyCode, string cdKey);

    // login user with company credentials
    Task<LoginResult> CompanyLoginAsync(CompanyLoginRequest request);
}
