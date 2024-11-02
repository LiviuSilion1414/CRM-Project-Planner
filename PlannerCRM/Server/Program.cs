using PlannerCRM.Server.Extensions;
using PlannerCRM.Server.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration
            .GetConnectionString("ConnString")
                ?? throw new InvalidOperationException("ConnString not found!"))
);

builder.Services.AddHttpClient();

builder.Services
    .AddIdentity<Employee, EmployeeRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddUserManager<UserManager<Employee>>()
    .AddDefaultTokenProviders();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services
    .Configure<IdentityOptions>(o =>
    {
        o.User.RequireUniqueEmail = true;
        o.SignIn.RequireConfirmedEmail = false;
        o.Password.RequireNonAlphanumeric = false;
        o.Password.RequireDigit = false;
        o.Password.RequiredLength = 8;
        o.Lockout.AllowedForNewUsers = true;
        o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        o.Lockout.MaxFailedAccessAttempts = 5;
    });
builder.Services.AddScoped<CalculatorService>();
builder.Services.AddScoped<DtoValidatorUtillity>();

builder.Services.AddScoped<IRepository<EmployeeFormDto>, EmployeeRepository>();
builder.Services.AddScoped<IRepository<WorkOrderFormDto>, WorkOrderRepository>();
builder.Services.AddScoped<IRepository<ActivityFormDto>, ActivityRepository>();
builder.Services.AddScoped<IRepository<WorkTimeRecordFormDto>, WorkTimeRecordRepository>();
builder.Services.AddScoped<IRepository<ClientFormDto>, ClientRepository>();

builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<IWorkTimeRecordRepository, WorkTimeRecordRepository>();

builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging"));
var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

if (!await context.Users.AnyAsync()) {
    await app.SeedDataAsync();
}

if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
} else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();