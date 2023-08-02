namespace PlannerCRM.Shared.Attributes;

public class PasswordValidatorAttribute : ValidationAttribute
{
    private int Minimum { get; set; }
    private int Maximum { get; set; }

    public PasswordValidatorAttribute(int minimum, int maximum) {
       Minimum = minimum;
       Maximum = maximum;
    }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(string)) { 
            var passLength = value.ToString().Count();

            return ((passLength >= Minimum) || (passLength <= Maximum));
        } else {
            return false;
        }
    }
}