using PlannerCRM.Server.Repositories;

namespace PlannerCRM.Server.Extensions;

public static class PipelineBuilderExtension
{
    public static void ConfigureDbConnectionString(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration
                    .GetConnectionString("DefaultDbString")
                        ?? throw new InvalidOperationException(""" "DefaultDbString" not found!""")));
    }

    public static void ConfigureIdentityOptions(this IServiceCollection services)
    {
        services
            .AddIdentity<Employee, EmployeeRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddUserManager<UserManager<Employee>>()
            .AddSignInManager<SignInManager<Employee>>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(o =>
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
    }

    public static void ConfigureCookiePolicy(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ActivityRepository>();
        services.AddScoped<EmployeeRepository>();
        services.AddScoped<FirmClientRepository>();
        services.AddScoped<SalaryRepository>();
        services.AddScoped<WorkOrderRepository>();
        services.AddScoped<WorkOrderCostRepository>();
        services.AddScoped<WorkTimeRepository>();
    }
}