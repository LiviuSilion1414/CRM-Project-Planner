using System.ComponentModel.DataAnnotations;
using static PlannerCRM.Shared.Constants.ConstantValues;

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
        if (value is null) return false;
        
        if (value.GetType() == typeof(DateTime)) { 
            var date = Convert.ToDateTime(value);
            var totalYears = CURRENT_DATE.Year - date.Year;
        
            return (((totalYears >= _Minimum) && (totalYears <= _Maximum)) && (date <= CURRENT_DATE));
        } else {
            return false;
        }
    }
}