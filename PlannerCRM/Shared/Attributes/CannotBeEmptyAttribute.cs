namespace PlannerCRM.Shared.Attributes;

public class CannotBeEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object value) => 
        value != null && (value as IEnumerable<object>)
        .GetEnumerator()
        .MoveNext();
}