namespace PlannerCRM.Shared.Attributes;

public class StartDateRangeHourlyRateAttribute : ValidationAttribute
{
    private int MinimumMonth { get; set; }
    private int MaximumMonth { get; set; }

    public StartDateRangeHourlyRateAttribute(int minimumMonth, int maximumMonth) 
    {
       MinimumMonth = minimumMonth;
       MaximumMonth = maximumMonth;
    }
    
    public StartDateRangeHourlyRateAttribute()
    { }

    public override bool IsValid(object value) {
        if (value is null) return false;
        
        if (value.GetType() == typeof(DateTime)) {
            var startDate = Convert.ToDateTime(value);
            
            return startDate <= CURRENT_DATE;
        } else {
            return false;
        }
    }
}