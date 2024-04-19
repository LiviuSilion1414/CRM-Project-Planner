using PlannerCRM.Client;
using PlannerCRM.Client.Utilities.Navigation;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider>(s => 
    s.GetRequiredService<AuthenticationStateService>()
);

builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<AuthenticationStateService>();
builder.Services.AddScoped<CurrentUserInfoService>();
builder.Services.AddScoped<NavigationLockService>();

builder.Services.AddScoped<ProjectManagerService>();
builder.Services.AddScoped<OperationManagerCrudService>();
builder.Services.AddScoped<AccountManagerCrudService>();
builder.Services.AddScoped<DeveloperService>();
builder.Services.AddScoped<CustomDataAnnotationsValidator>();

builder.Services.AddScoped<Base64Converter>();

builder.Services.AddScoped<Logger<AuthenticationStateService>>();
builder.Services.AddScoped<Logger<CurrentUserInfoService>>();
builder.Services.AddScoped<Logger<AccountManagerCrudService>>();
builder.Services.AddScoped<Logger<OperationManagerCrudService>>();
builder.Services.AddScoped<Logger<ProjectManagerService>>();

builder.Services.AddScoped<Logger<LoginService>>();
builder.Services.AddScoped<Logger<DeveloperService>>();
builder.Services.AddScoped<Logger<EmployeeLoginDto>>();
builder.Services.AddScoped<Logger<EmployeeFormDto>>();
builder.Services.AddScoped<Logger<WorkOrderFormDto>>();
builder.Services.AddScoped<Logger<ActivityFormDto>>();

builder.Services.AddScoped(_ => 
    new HttpClient {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    }
);

await builder.Build().RunAsync();