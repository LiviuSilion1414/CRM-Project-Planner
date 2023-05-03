using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.Attributes;

public class BirthDayRangeAttribute : ValidationAttribute
{
    private int _Minimum { get; set; }
    private int _Maximum { get; set; }

    public BirthDayRangeAttribute(int minimum, int maximum) {
       _Minimum = minimum;
       _Maximum = maximum;
    }

    public override bool IsValid(object value) {
        if (value.GetType() == typeof(DateTime)) { 
            var birthDay = Convert.ToDateTime(value).Year;
        
            return ((birthDay >= _Minimum) && (birthDay <= _Maximum)) 
                ? true 
                : false;
        } else {
            return false;
        }
    }
}