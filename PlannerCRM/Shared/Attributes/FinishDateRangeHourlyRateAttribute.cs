namespace PlannerCRM.Shared.Attributes;

public class FinishDateRangeHourlyRateAttribute : ValidationAttribute
{
    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(DateTime)) {
            return (DateTime) value >= CURRENT_DATE;
        } 
        
        return false;
    }
}