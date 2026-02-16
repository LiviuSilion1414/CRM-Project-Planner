using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Registrazioni dei servizi (adatta quelli browser‑specifici)
builder.Services.AddScoped<AuthenticationStateProvider, AuthService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FetchService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<LocalStorageService>();
//builder.Services.AddSingleton<WeatherForecastService>();

// Per chiamate HTTP lato server
builder.Services.AddHttpClient();
builder.Services.AddScoped(_ =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["ApiUrl"])
    }
);

builder.Services.AddRadzenComponents();
builder.Services.AddAuthorizationCore();

builder.Services.AddOptions();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
