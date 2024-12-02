namespace PlannerCRM.Client.Extensions;

public static class PipelineBuilderExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IFetchService<ActivityDto>, FetchService<ActivityDto>>();
        services.AddScoped<IFetchService<EmployeeDto>, FetchService<EmployeeDto>>();
        services.AddScoped<IFetchService<FirmClientDto>, FetchService<FirmClientDto>>();
        services.AddScoped<IFetchService<RoleDto>, FetchService<RoleDto>>();
        services.AddScoped<IFetchService<SalaryDto>, FetchService<SalaryDto>>();
        services.AddScoped<IFetchService<WorkOrderDto>, FetchService<WorkOrderDto>>();
        services.AddScoped<IFetchService<WorkOrderCostDto>, FetchService<WorkOrderCostDto>>();
        services.AddScoped<IFetchService<WorkTimeDto>, FetchService<WorkTimeDto>>();

        return services;
    }
}
