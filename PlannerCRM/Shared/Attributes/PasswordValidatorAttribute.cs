namespace PlannerCRM.Shared.Attributes;

public partial class PasswordValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
        if (value is not null && value.GetType() == typeof(string) && EmailRegex().IsMatch(value as string)) 
        {
            return ValidationResult.Success;
        }
            
        return new ValidationResult("password should contain at least ONE lowercase letter, ONE uppercase letter and at least ONE digit");        
    }

    [GeneratedRegex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])")]
    private static partial Regex EmailRegex();
}