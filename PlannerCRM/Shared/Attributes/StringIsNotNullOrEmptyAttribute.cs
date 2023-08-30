namespace PlannerCRM.Shared.Attributes;

public class StringIsNotNullOrEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object value) {
        return value is not null && !string.IsNullOrEmpty(value as string);
    }
}