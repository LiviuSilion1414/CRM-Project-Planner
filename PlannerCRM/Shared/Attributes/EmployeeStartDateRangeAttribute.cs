using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.Attributes;

public class EmployeeStartDateRangeAttribute : ValidationAttribute
{
    private int _Minimum { get; set; }
    private int _Maximum { get; set; }

    public EmployeeStartDateRangeAttribute(int minimum, int maximum) {
       _Minimum = minimum;
       _Maximum = maximum;
    }

    public override bool IsValid(object value) {
        if (value.GetType() == typeof(DateTime)) {
            var startDate = Convert.ToDateTime(value);
            
            return ((startDate.Month >= _Minimum) 
                || (startDate.Month <= _Maximum)) 
                ? true 
                : false;
        } else {
            return false;
        }
    }
}