namespace PlannerCRM.Shared.Attributes;

using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class StartsWithSlashAttribute : ValidationAttribute
{
    public StartsWithSlashAttribute()
    {
        ErrorMessage = "The Path must start with '/'";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Se null o stringa vuota, lascia la responsabilità a [Required]
        if (value == null)
            return ValidationResult.Success;

        if (value is not string path)
            return new ValidationResult("The Path must be a string value");

        if (!path.StartsWith("/"))
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}