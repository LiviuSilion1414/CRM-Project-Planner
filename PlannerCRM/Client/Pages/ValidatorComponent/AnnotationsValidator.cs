/*using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Services;

namespace PlannerCRM.Client.Pages.ValidatorComponent;

public class AnnotationsValidator : ComponentBase, IDisposable
{
    private IDisposable _subscriptions;

    [CascadingParameter] EditContext CurrentEditContext { get; set; }
    [Parameter] public object Model { get; set; }
    
    [Inject] private IServiceProvider _ServiceProvider { get; set; }
    
    private ValidationMessageStore _messages { get; set; }


    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(DataAnnotationsValidator)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}." +
                $"For example, you can use {nameof(DataAnnotationsValidator)} " +
                $"inside an EditForm.");
        }

        CurrentEditContext = new(Model);

        _messages= new(CurrentEditContext);
        
        CurrentEditContext.OnFieldChanged += OnFieldChanged;
        CurrentEditContext.OnValidationRequested += OnValidationRequested;

        _subscriptions = CurrentEditContext.EnableDataAnnotationsValidation(_ServiceProvider);
    }

    public void DisplayErrors(Dictionary<string, List<string>> errors)
    {
        foreach (var err in errors)
        {
            _messages.Add(CurrentEditContext.Field(err.Key), err.Value);
        }        
        CurrentEditContext.NotifyValidationStateChanged();
    }


    private void OnFieldChanged(object? sender, FieldChangedEventArgs eventArgs)
    {
        var fieldIdentifier = eventArgs.FieldIdentifier;
        if (!ValidatorService.ValidateProperty(fieldIdentifier.FieldName, Model, out var validationResults)) { 
            foreach (var result in validationResults) {
                _messages.Add(new FieldIdentifier(Model, eventArgs.FieldIdentifier.FieldName), result.ErrorMessage);
                CurrentEditContext.GetValidationMessages(fieldIdentifier);
            }
        } else {
            _messages.Clear(fieldIdentifier);
        }
    
        CurrentEditContext.NotifyValidationStateChanged();
    }

    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        ValidatorService.ValidateModel(Model, out var validationResults);

        foreach (var result in validationResults)
        {
            if (result.Key == null) {
                continue;
            }

            var hasMemberNames = false;
            foreach (var memberName in result.Value) {
                hasMemberNames = true;
                _messages.Add(CurrentEditContext.Field(memberName), result.ErrorMessage);
                System.Console.WriteLine("membername {0}", memberName);
            }

            if (!hasMemberNames) {
                _messages.Add(new FieldIdentifier(CurrentEditContext.Model, fieldName: string.Empty), result.ErrorMessage);
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }


    protected virtual void Dispose(bool disposing)
    { }

    void IDisposable.Dispose()
    {
        _subscriptions?.Dispose();
        _subscriptions = null;

        Dispose(disposing: true);
    }
}
*/