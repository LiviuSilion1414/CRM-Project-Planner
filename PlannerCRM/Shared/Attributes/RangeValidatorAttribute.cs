using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.Attributes;

public class RangeValidatorAttribute : ValidationAttribute
{
    private int _Minimum { get; set; }
    private int _Maximum { get; set; }

    public RangeValidatorAttribute(int minimum, int maximum) {
       _Minimum = minimum;
       _Maximum = maximum;
    }

    public override bool IsValid(object value) {
        if (value.GetType() == typeof(int)) {  //int Range check
            var length = (int) value;

            return ((length >= _Minimum) && (length <= _Maximum))
                ? true
                : false;
        } else if (value.GetType() == typeof(string)) { //Password
            var passLength = value.ToString().Length;

            return ((passLength >= _Minimum) && (passLength <= _Maximum))
                ? true 
                : false;
        } else {
            return false;
        }
    }
}