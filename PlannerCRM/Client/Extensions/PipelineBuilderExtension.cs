using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace PlannerCRM.Client.Extensions;

public static class PipelineBuilderExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {   
        services.AddScoped<AuthenticationStateProvider, AuthService>();
        services.AddScoped<AuthService>();
        
        services.AddScoped<LocalStorageService>();
        services.AddScoped<FetchService>();

        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();
        services.AddScoped<ContextMenuService>();


        return services;
    }
}
