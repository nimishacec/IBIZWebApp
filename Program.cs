using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using IBIZWebApp;
using IBIZWebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register ICompanySelectionService and its implementation
builder.Services.AddScoped<ICompanySelectionService, CompanySelectionService>();
// Register IToastService and its implementation
builder.Services.AddScoped<IToastService, ToastService>();

builder.Services.AddScoped<ITokenService, TokenService>();
// Register IAuthenticationService and its implementation
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddScoped<ISalesDropdownService, SalesDropdownService>();
// Register Receipt services
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IReceiptVoucherService, ReceiptVoucherService>();
await builder.Build().RunAsync();
