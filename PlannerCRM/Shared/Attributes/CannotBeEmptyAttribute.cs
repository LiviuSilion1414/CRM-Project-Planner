namespace PlannerCRM.Shared.Attributes;

public class CannotBeEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object value) =>
        value is not null && (value as IEnumerable<object>)
            .Any();
}