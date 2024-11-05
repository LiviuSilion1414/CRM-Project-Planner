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

        services.AddScoped<IRepository<Activity, ActivityDto>, Repository<Activity, ActivityDto>>();
        services.AddScoped<IRepository<Employee, EmployeeDto>, Repository<Employee, EmployeeDto>>();
        services.AddScoped<IRepository<FirmClient, FirmClientDto>, Repository<FirmClient, FirmClientDto>>();
        services.AddScoped<IRepository<Role, RoleDto>, Repository<Role, RoleDto>>();
        services.AddScoped<IRepository<Salary, SalaryDto>, Repository<Salary, SalaryDto>>();
        services.AddScoped<IRepository<WorkOrder, WorkOrderDto>, Repository<WorkOrder, WorkOrderDto>>();
        services.AddScoped<IRepository<WorkOrderCost, WorkOrderCostDto>, Repository<WorkOrderCost, WorkOrderCostDto>>();
        services.AddScoped<IRepository<WorkTime, WorkTimeDto>, Repository<WorkTime, WorkTimeDto>>();

        services.AddScoped<IRepository<ActivityWorkTime, ActivityWorkTimeDto>, Repository<ActivityWorkTime, ActivityWorkTimeDto>>();
        services.AddScoped<IRepository<ClientWorkOrderCost, ClientWorkOrderCostDto>, Repository<ClientWorkOrderCost, ClientWorkOrderCostDto>>();
        services.AddScoped<IRepository<EmployeeActivity, EmployeeActivityDto>, Repository<EmployeeActivity, EmployeeActivityDto>>();
        services.AddScoped<IRepository<EmployeeRole, EmployeeRoleDto>, Repository<EmployeeRole, EmployeeRoleDto>>();
        services.AddScoped<IRepository<EmployeeSalary, EmployeeSalaryDto>, Repository<EmployeeSalary, EmployeeSalaryDto>>();
        services.AddScoped<IRepository<EmployeeWorkTime, EmployeeWorkTimeDto>, Repository<EmployeeWorkTime, EmployeeWorkTimeDto>>();
        services.AddScoped<IRepository<WorkOrderActivity, WorkOrderActivityDto>, Repository<WorkOrderActivity, WorkOrderActivityDto>>();
    }
}