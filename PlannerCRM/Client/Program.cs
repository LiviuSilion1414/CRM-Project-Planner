var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.RegisterServices();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(_ => 
    new HttpClient {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    }
);

await builder.Build().RunAsync();