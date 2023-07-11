using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Services;

namespace PlannerCRM.Client.Pages.ValidatorComponent;

public partial class CustomDataAnnotationsValidator : ComponentBase
{
    [CascadingParameter(Name = "Errors")] Dictionary<string, List<string>> Errors { get; set; }
    [CascadingParameter] EditContext? CurrentEditContext { get; set; }

    private ValidationMessageStore? messageStore;

    protected override void OnInitialized() {
        if (CurrentEditContext is null) {
            throw new InvalidOperationException(
                $"{nameof(CustomDataAnnotationsValidator)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. " +
                $"For example, you can use {nameof(CustomDataAnnotationsValidator)} " +
                $"inside an {nameof(EditForm)}.");
        }
        
        Errors = new();
        messageStore = new(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += OnValidationRequested;
            
        CurrentEditContext.OnFieldChanged += OnFieldChanged;  ///prova
    }

    void OnFieldChanged(object? sender, FieldChangedEventArgs e) {
        messageStore?.Clear(e.FieldIdentifier);
        Errors.Remove(e.FieldIdentifier.FieldName);
    }

    void OnFieldChanged2(object? sender, FieldChangedEventArgs e) {
        var property = CurrentEditContext.Model
            .GetType()
            .GetProperty(e.FieldIdentifier.FieldName);

        var isValid = ValidatorService
            .ValidateProperty(
                model: CurrentEditContext.Model, 
                property: property, 
                errors: out var errors);

        if (isValid) {
            messageStore?.Clear(e.FieldIdentifier);
            Errors.Remove(e.FieldIdentifier.FieldName);
        } else {
            DisplayErrors(errors);
        }
    }

    void OnValidationRequested(object? sender, ValidationRequestedEventArgs e) => messageStore?.Clear();
    
    public void DisplayErrors(Dictionary<string, List<string>> errors = null) {
        if (CurrentEditContext is not null) {
            if (errors is not null) {
                AddIntoMessageStore(errors);
            } else {
                AddIntoMessageStore(Errors);
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }
    }

    private void AddIntoMessageStore(Dictionary<string, List<string>> errors) {
        errors
            .ToList()
            .ForEach(err => messageStore?
                .Add(CurrentEditContext.Field(err.Key), err.Value));
    }


    public void ClearErrors() {
        messageStore?.Clear();
        CurrentEditContext?.NotifyValidationStateChanged();
    }
}