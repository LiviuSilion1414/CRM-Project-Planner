using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

    public static void ConfigureJWTTokenAuthentication(this WebApplicationBuilder builder)
    {
        //JWToken
        IConfigurationSection appSettingsSection = builder.Configuration.GetSection("ClientAppSettings");
        builder.Services.Configure<ServerAppSettings>(appSettingsSection);
        ServerAppSettings appSettings = appSettingsSection.Get<ServerAppSettings>();
        byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);

        builder.Services.AddAuthentication(options =>
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

        services.AddScoped<ActivityRepository>();
        services.AddScoped<EmployeeRepository>();
        services.AddScoped<FirmClientRepository>();
        services.AddScoped<SalaryRepository>();
        services.AddScoped<WorkOrderRepository>();
        services.AddScoped<WorkOrderCostRepository>();
        services.AddScoped<WorkTimeRepository>();
    }
}