using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.Attributes;

public class StartDateRangeAttribute : ValidationAttribute
{
    private int _Minimum { get; set; }
    private int _Maximum { get; set; }

    public StartDateRangeAttribute(int minimum, int maximum) {
       _Minimum = minimum;
       _Maximum = maximum;
    }

    public override bool IsValid(object value) {
        if (value.GetType() == typeof(DateTime)) {
            var startDate = Convert.ToDateTime(value).Year;
            
            return ((startDate >= _Minimum) && (startDate <= _Maximum)) 
                ? true 
                : false;
        } else {
            return false;
        }
    }
}