//Scaffold-DbContext "Server=localhost;Database=Planner_crm;Trusted_Connection=true;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f

using PlannerCRM.Server.Profiles;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

builder.Services.ConfigureSqlConnection(builder.Configuration.GetConnectionString("DefaultDbString"));

builder.Services.ConfigureJwtAuth(builder.Configuration.GetSection("AppSettings"));

builder.Services.RegisterServices();

builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
    cfg.LicenseKey = builder.Configuration["AutoMapper:LicenseKey"];
});

builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging"));
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
