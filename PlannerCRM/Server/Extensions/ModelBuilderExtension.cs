using Microsoft.EntityFrameworkCore;

namespace PlannerCRM.Server.Extensions;

public static class ModelBuilderExtension
{
    public static void ConfigureEnums(this ModelBuilder builder)
    {
        builder.HasPostgresEnum<Roles>();
    }

    public static void ConfigureRelationships(this ModelBuilder builder)
    {

    }
}
