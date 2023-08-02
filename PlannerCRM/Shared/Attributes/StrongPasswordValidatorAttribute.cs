namespace PlannerCRM.Shared.Attributes;

public class StrongPasswordValidatorAttribute : ValidationAttribute
{   
    private int Minimum { get; set; }
    private int Maximum { get; set; }

    public StrongPasswordValidatorAttribute(int minimum, int maximum) {
        Minimum = minimum;
        Maximum = maximum;
    }

    public StrongPasswordValidatorAttribute()
    { }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(string)) {
            var pattern = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])");
            var stringContent = value.ToString();
            var stringLength = stringContent.Length;

            return (((stringLength >= Minimum) && (stringLength <= Maximum)) && (pattern.IsMatch(stringContent)));
        } else {
            return false;        
        }
    }
}