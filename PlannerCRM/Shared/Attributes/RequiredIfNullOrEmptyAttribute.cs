namespace PlannerCRM.Shared.Attributes;

public class RequiredIfNullOrEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object value) {
        return value.GetType().GetProperties().Any(p => p.GetValue(value) is not null); 
    }
}