using System.ComponentModel.DataAnnotations;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.Attributes;

public class StartDateRangeHourlyRateAttribute : ValidationAttribute
{
    private int _MinimumMonth { get; set; }
    private int _MaximumMonth { get; set; }

    public StartDateRangeHourlyRateAttribute(int minimumMonth, int maximumMonth) 
    {
       _MinimumMonth = minimumMonth;
       _MaximumMonth = maximumMonth;
    }

    public override bool IsValid(object value) {
        if (value == null) {
            return false;
        }
        if (value.GetType() == typeof(DateTime)) {
            var startDate = Convert.ToDateTime(value);
            
            return ((startDate.Year == CURRENT_YEAR) && ((startDate.Month >= _MinimumMonth) && (startDate.Month <= _MaximumMonth) 
            &&(startDate.Day <= CURRENT_DATE.Day))) 
                ? true 
                : false;
        } else {
            return false;
        }
    }
}