namespace PlannerCRM.Shared.Attributes;

public sealed class PresentOrFutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (Convert.ToDateTime(value) >= DateTime.Today)
        {
            return ValidationResult.Success;
        } 
        else
        {
            return new ValidationResult("Past date not allowed.");
        }
    }
}