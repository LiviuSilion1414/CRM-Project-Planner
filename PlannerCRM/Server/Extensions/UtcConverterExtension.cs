namespace PlannerCRM.Server.Extensions;

public static class UtcConverterExtension
{
    public static void ConvertToUtcDateTime(this ModelBuilder modelBuilder) {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
            foreach (var property in entityType.GetProperties().Where(p => p.ClrType == typeof(DateTime))) {
                property.SetValueConverter(new UtcValueConverter());
            }
        }
    }
}