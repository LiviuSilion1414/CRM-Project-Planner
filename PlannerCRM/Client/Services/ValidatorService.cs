using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.VisualBasic;

namespace PlannerCRM.Client.Services;

public static class ValidatorService
{
    public static bool ValidateModel(object model, out Dictionary<string, List<string>> errors) {
        errors = new();

        if (model is null) return false;

        var memberNames = new List<string>();

        var properties = model 
            .GetType()
            .GetProperties()
            .Select(prop => prop)
            .ToList();
        
        foreach (var property in properties) {
            var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();
            foreach (var attribute in validationAttributes) {
                var propertyValue = property.GetValue(model);

                var isValid = attribute.IsValid(propertyValue);
                var validationContext = new ValidationContext(property);
                var memberName = validationContext.MemberName = property.Name;
                
                if (!isValid) {
                    memberNames.Add(validationContext.MemberName);
                    errors.Add(memberName, new() { attribute.ErrorMessage });
                } 
            }               
        }
        return !errors.Any();
    }

    public static bool ValidateProperty(object model, PropertyInfo property, out Dictionary<string, List<string>> errors) {
        errors = new();

        var propertyName = property.Name;
        System.Console.WriteLine("Property name: {0}", propertyName);

        var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();
        var propertyValue = property.GetValue(model, null);
        
        foreach(var attribute in validationAttributes) {
            if (!attribute.IsValid(propertyValue)) {
                errors.Add(propertyName, new() { attribute.ErrorMessage });
            }
        }
        System.Console.WriteLine("is valid: {0}", !errors.Any());
        return !errors.Any();
    }
}
