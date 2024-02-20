using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Bookings.Client;
using Bookings.Client.Authentication;
using Bookings.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddHttpClient<AuthenticationHttpClient>();
builder.Services.AddHttpClient<ContentHttpClient>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5176/") });
builder.Services.AddScoped<AccountService>(); 
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
await builder.Build().RunAsync();