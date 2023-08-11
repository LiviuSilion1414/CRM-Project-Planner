using PlannerCRM.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AuthenticationStateService>());
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<AuthenticationStateService>();
builder.Services.AddScoped<CurrentUserInfoService>();
builder.Services.AddScoped<NavigationLockService>();

builder.Services.AddScoped<OperationManagerCrudService>();
builder.Services.AddScoped<AccountManagerCrudService>();
builder.Services.AddScoped<DeveloperService>();
builder.Services.AddScoped<CustomDataAnnotationsValidator>();

builder.Services.AddScoped<ILogger<AuthenticationStateService>>();
builder.Services.AddScoped<ILogger<CurrentUserInfoService>>();
builder.Services.AddScoped<ILogger<AccountManagerCrudService>>();
builder.Services.AddScoped<ILogger<OperationManagerCrudService>>();

builder.Services.AddScoped<ILogger<LoginService>>();
builder.Services.AddScoped<ILogger<DeveloperService>>();
builder.Services.AddScoped<ILogger<EmployeeLoginDto>>();
builder.Services.AddScoped<ILogger<EmployeeFormDto>>();
builder.Services.AddScoped<ILogger<WorkOrderFormDto>>();
builder.Services.AddScoped<ILogger<ActivityFormDto>>();

builder.Services.AddScoped(sp => 
    new HttpClient {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    }
);

await builder.Build().RunAsync();