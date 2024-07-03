using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PlannerCRM.Server.Converters;

public class UtcValueConverter : ValueConverter<DateTime, DateTime>
{
    public UtcValueConverter()
        : base(
            date => date.Kind == DateTimeKind.Utc ? date : date.ToUniversalTime(), // Converts to UTC before storing in DB
            date => DateTime.SpecifyKind(date, DateTimeKind.Utc)) // Ensures it's read as UTC from DB
    { }
}