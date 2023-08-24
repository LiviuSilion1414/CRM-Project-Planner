namespace PlannerCRM.Shared.Attributes;

public class IsNotZeroAttribute : ValidationAttribute
{
    public override bool IsValid(object value) {
        return 
            value is not null && 
            int.TryParse(value.ToString(), out var res) && 
            res != 0;
    }
}