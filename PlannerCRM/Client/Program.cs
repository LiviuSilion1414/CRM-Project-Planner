using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, AuthService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<FetchService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(_ => 
    new HttpClient {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    }
);

builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();