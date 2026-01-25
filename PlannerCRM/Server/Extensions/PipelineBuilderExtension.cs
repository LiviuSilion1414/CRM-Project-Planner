using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using PlannerCRM.Server.Models;
using PlannerCRM.Server.Repositories;
using PlannerCRM.Server.System;
using System.Text;

namespace PlannerCRM.Server.Extensions;

public static class PipelineBuilderExtension
{
    public static void ConfigureSqlConnection(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<PlannerCrmContext>(options =>options.UseSqlServer(connectionString));
    }

    public static void ConfigureJwtAuth(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        //JWToken
        IConfigurationSection appSettingsSection = configurationSection;
        services.Configure<ServerAppSettings>(appSettingsSection);
        ServerAppSettings appSettings = appSettingsSection.Get<ServerAppSettings>();
        byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ServerAppSettings>();
        services.AddScoped<SystemLogHelper>();
        services.AddScoped<ActivityRepository>();
        services.AddScoped<EmployeeRepository>();
        services.AddScoped<FirmClientRepository>();
        services.AddScoped<WorkOrderRepository>();
        services.AddScoped<WorkTimeRepository>();
        services.AddScoped<RoleRepository>();
        services.AddScoped<MenuRoleRepository>();
        services.AddScoped<SettingRepository>();
    }
}