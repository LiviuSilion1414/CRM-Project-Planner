var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.ConfigureDbConnectionString();

builder.Services.AddHttpClient();

builder.Services.ConfigureIdentityOptions();

builder.Services.ConfigureCookiePolicy();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.RegisterServices();

builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging"));
var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/DevError");

    //app.UseWebAssemblyDebugging();
} else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

//app.UseAuthentication();

app.UseRouting();
//app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();