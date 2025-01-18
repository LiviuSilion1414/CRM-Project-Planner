using Radzen;

namespace PlannerCRM.Client.Extensions;

public static class PipelineBuilderExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<DialogService>();

        services.AddScoped<FetchService<ActivityDto>>();
        services.AddScoped<FetchService<EmployeeDto>>();
        services.AddScoped<FetchService<FirmClientDto>>();
        services.AddScoped<FetchService<RoleDto>, FetchService<RoleDto>>();
        services.AddScoped<FetchService<SalaryDto>>();
        services.AddScoped<FetchService<WorkOrderDto>>();
        services.AddScoped<FetchService<WorkOrderCostDto>>();
        services.AddScoped<FetchService<WorkTimeDto>>();

        return services;
    }
}
