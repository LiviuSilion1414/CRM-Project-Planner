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
        services.RegisterSpecificRepositories();
        services.RegisterDtoToModelMappings();
        services.RegisterModelToDtoMappings();
    }

    private static void RegisterModelToDtoMappings(this IServiceCollection services)
    {
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
    
    private static void RegisterDtoToModelMappings(this IServiceCollection services)
    {
        services.AddScoped<IRepository<ActivityDto, Activity>, Repository<ActivityDto, Activity>>();
        services.AddScoped<IRepository<EmployeeDto, Employee>, Repository<EmployeeDto, Employee>>();
        services.AddScoped<IRepository<FirmClientDto, FirmClient>, Repository<FirmClientDto, FirmClient>>();
        services.AddScoped<IRepository<RoleDto, Role>, Repository<RoleDto, Role>>();
        services.AddScoped<IRepository<SalaryDto, Salary>, Repository<SalaryDto, Salary>>();
        services.AddScoped<IRepository<WorkOrderDto, WorkOrder>, Repository<WorkOrderDto, WorkOrder>>();
        services.AddScoped<IRepository<WorkOrderCostDto, WorkOrderCost>, Repository<WorkOrderCostDto, WorkOrderCost>>();
        services.AddScoped<IRepository<WorkTimeDto, WorkTime>, Repository<WorkTimeDto, WorkTime>>();

        services.AddScoped<IRepository<ActivityWorkTimeDto, ActivityWorkTime>, Repository<ActivityWorkTimeDto, ActivityWorkTime>>();
        services.AddScoped<IRepository<ClientWorkOrderCostDto, ClientWorkOrderCost>, Repository<ClientWorkOrderCostDto, ClientWorkOrderCost>>();
        services.AddScoped<IRepository<EmployeeActivityDto, EmployeeActivity>, Repository<EmployeeActivityDto, EmployeeActivity>>();
        services.AddScoped<IRepository<EmployeeRoleDto, EmployeeRole>, Repository<EmployeeRoleDto, EmployeeRole>>();
        services.AddScoped<IRepository<EmployeeSalaryDto, EmployeeSalary>, Repository<EmployeeSalaryDto, EmployeeSalary>>();
        services.AddScoped<IRepository<EmployeeWorkTimeDto, EmployeeWorkTime>, Repository<EmployeeWorkTimeDto, EmployeeWorkTime>>();
        services.AddScoped<IRepository<WorkOrderActivityDto, WorkOrderActivity>, Repository<WorkOrderActivityDto, WorkOrderActivity>>();
    }

    private static void RegisterSpecificRepositories(this IServiceCollection services)
    {
        services.AddScoped<Repository<Activity, ActivityDto>, ActivityRepository>();
        services.AddScoped<Repository<Employee, EmployeeDto>, EmployeeRepository>();
        services.AddScoped<Repository<FirmClient, FirmClientDto>, FirmClientRepository>();
        services.AddScoped<Repository<Salary, SalaryDto> ,SalaryRepository>();
        services.AddScoped<Repository<WorkOrder, WorkOrderDto>, WorkOrderRepository>();
        services.AddScoped<Repository<WorkOrderCost, WorkOrderCostDto>, WorkOrderCostRepository>();
        services.AddScoped<Repository<WorkTime, WorkTimeDto>, WorkTimeRepository>();
        services.AddScoped<CalculatorService>();
    }
}