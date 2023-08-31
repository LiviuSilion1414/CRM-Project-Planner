namespace PlannerCRM.Shared.Attributes;

public class StringIsNotNullOrEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object value) {
        return !string.IsNullOrEmpty(value as string);
    }
}