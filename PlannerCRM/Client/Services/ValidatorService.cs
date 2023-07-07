using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Services;

public class ValidatorService
{
    private object _Model { get; set; }

    public ValidatorService(object Model) {
        _Model = Model;
    }

    public ValidatorService()
    { }

    public bool Validate(object Model, out List<ValidationResult> validationResults) {
        validationResults = new();

        var properties = Model 
            .GetType()
            .GetProperties()
            .Select(prop => prop)
            .ToList();
        
        foreach (var property in properties) {
            var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();
            foreach (var attribute in validationAttributes) {
                var propertyValue = property.GetValue(Model) ?? false;
                var isValid = attribute.IsValid(propertyValue);
                var validationResult = new ValidationResult(attribute.ErrorMessage);
                
                if (!isValid) {
                    validationResults.Add(validationResult);
                } 
            }               
        }
        return !validationResults.Any();
    }
}
