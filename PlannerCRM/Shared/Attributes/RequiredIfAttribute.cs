namespace PlannerCRM.Shared.Attributes;

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _propertyName;
    private readonly object _targetValue;

    public RequiredIfAttribute(string propertyName, object targetValue)
    {
        _propertyName = propertyName;
        _targetValue = targetValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_propertyName);

        if (property == null)
            return new ValidationResult($"Unknown property {_propertyName}");

        var propertyValue = property.GetValue(validationContext.ObjectInstance);

        if (Equals(propertyValue, _targetValue))
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(ErrorMessage ??
                    $"{validationContext.DisplayName} is required.");
            }
        }

        return ValidationResult.Success;
    }
}
