using PlannerCRM.Server.Models.Entities;
using PlannerCRM.Server.Models.JunctionEntities;

namespace PlannerCRM.Server.Extensions;

public static class PipelineBuilderExtension
{
    public static void ConfigureDbConnectionString(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration
                    .GetConnectionString("ConnString")
                        ?? throw new InvalidOperationException("ConnString not found!"))
);
    }

    public static void ConfigureIdentityOptions(this IServiceCollection services)
    {
        services
            .AddIdentity<Employee, EmployeeRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddUserManager<UserManager<Employee>>()
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
        services.AddScoped<CalculatorService>();
        services.AddScoped<DtoValidatorUtillity>();
        services.AddScoped<IRepository<EmployeeFormDto>, EmployeeRepository>();
        services.AddScoped<IRepository<WorkOrderFormDto>, WorkOrderRepository>();
        services.AddScoped<IRepository<ActivityFormDto>, ActivityRepository>();
        services.AddScoped<IRepository<WorkTimeRecordFormDto>, WorkTimeRepository>();
        services.AddScoped<IRepository<ClientFormDto>, ClientRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        services.AddScoped<IWorkTimeRepository, WorkTimeRepository>();
    }
}