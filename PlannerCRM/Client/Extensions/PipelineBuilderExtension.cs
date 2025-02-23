using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace PlannerCRM.Client.Extensions;

public static class PipelineBuilderExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {   
        services.AddSingleton<LocalStorageService>();

        services.AddScoped<AuthenticationStateProvider, AuthService>();
        services.AddScoped<AuthService>();
        services.AddScoped<AuthService>();

        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();
        services.AddScoped<ContextMenuService>();

        services.AddScoped<FetchService>();

        return services;
    }
}
