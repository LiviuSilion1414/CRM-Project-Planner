namespace PlannerCRM.Shared.Attributes;

public class DateRangeValidationAttribute : ValidationAttribute
{
    private readonly string _startDateProperty;
    private readonly string _endDateProperty;

    public DateRangeValidationAttribute(string startDateProperty, string endDateProperty)
    {
        _startDateProperty = startDateProperty;
        _endDateProperty = endDateProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var startDate = validationContext.ObjectType.GetProperty(_startDateProperty)?.GetValue(validationContext.ObjectInstance, null) as DateTime?;
        var endDate = validationContext.ObjectType.GetProperty(_endDateProperty)?.GetValue(validationContext.ObjectInstance, null) as DateTime?;

        if (startDate.HasValue && endDate.HasValue)
        {
            if (endDate.Value < startDate.Value)
            {
                return new ValidationResult("End date cannot be earlier than start date.");
            }
        }

        return ValidationResult.Success;
    }
}
