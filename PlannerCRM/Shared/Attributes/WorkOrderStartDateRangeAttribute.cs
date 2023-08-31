namespace PlannerCRM.Shared.Attributes;

public class WorkOrderStartDateRangeAttribute : ValidationAttribute
{
    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(DateTime)) {
            var startDate = Convert.ToDateTime(value);

            return (startDate.Month >= MIN_WORKORDER_MONTH_CONTRACT) || 
                (startDate.Month <= MAX_WORKORDER_MONTH_CONTRACT);
        } 
        
        return false;
    }
}