using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PlannerCRM.Shared.Attributes;

public class StrongPasswordValidatorAttribute : ValidationAttribute
{   
    private int _Minimum { get; set; }
    private int _Maximum { get; set; }

    public StrongPasswordValidatorAttribute(int minimum, int maximum) {
        _Minimum = minimum;
        _Maximum = maximum;
    }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(string)) {
            var pattern = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])");
            var stringContent = value.ToString();
            var stringLength = stringContent.Length;

            return (((stringLength >= _Minimum) && (stringLength <= _Maximum)) && (pattern.IsMatch(stringContent)));
        } else {
            return false;        
        }
    }
}