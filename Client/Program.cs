using Blazored.LocalStorage;
using Client;
using Client.Handlers;
using Client.Logging;
using Client.Services.AppService;
using Client.Services.Auth;
using Client.Services.Charges;
using Client.Services.Company;
using Client.Services.Customers;
using Client.Services.Dashboard;
using Client.Services.Labs;
using Client.Services.Orders;
using Client.Services.Products;
using Client.Services.Referers;
using Client.Services.Users;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using QuestPDF.Infrastructure;
using Shared.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("AppUrl", http =>
{
    http.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
}).AddHttpMessageHandler<CustomAuthorizationHandler>();
builder.Services.AddHttpClient<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(options => options.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddTransient<CustomAuthorizationHandler>();
builder.Services.AddLogging(logging => {
    var httpClient = builder.Services.BuildServiceProvider().GetRequiredService<HttpClient>();
    var authenticationStateProvider = builder.Services.BuildServiceProvider().GetRequiredService<AuthenticationStateProvider>();
    logging.SetMinimumLevel(LogLevel.Error);
    logging.AddProvider(new ApplicationLoggerProvider(httpClient, authenticationStateProvider));
});

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ILabService, LabService>();
builder.Services.AddTransient<IDashboardService, DashboardService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IStoreService, StoreService>();
builder.Services.AddTransient<IRefererService, RefererService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IChargeService, ChargeService>();

QuestPDF.Settings.License = LicenseType.Community;

await builder.Build().RunAsync();
