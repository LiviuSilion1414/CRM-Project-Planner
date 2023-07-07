using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Services;

namespace PlannerCRM.Client.Pages.ObjectValidator;

public class AnnotationsValidator : ComponentBase, IDisposable
{
    private IDisposable _subscriptions;

    [Parameter] public object Model { get; set; }
    [CascadingParameter] EditContext CurrentEditContext { get; set; }
    [Inject] private IServiceProvider _ServiceProvider { get; set; }
    [Inject] private ValidatorService _ValidatorService { get; set; }
    
    private ValidationMessageStore _messages { get; set; }


    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(DataAnnotationsValidator)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(DataAnnotationsValidator)} " +
                $"inside an EditForm.");
        }
        CurrentEditContext = new(Model);
        _ValidatorService = new(Model);

        _messages= new(CurrentEditContext);
        
        CurrentEditContext.OnFieldChanged += OnFieldChanged;
        CurrentEditContext.OnValidationRequested += OnValidationRequested;

        _subscriptions = CurrentEditContext.EnableDataAnnotationsValidation(_ServiceProvider);
    }
    // working pretty good, except by it's not showing errors...
    private void OnFieldChanged(object? sender, FieldChangedEventArgs eventArgs)
    {
        var fieldIdentifier = eventArgs.FieldIdentifier;
        if (_ValidatorService.Validate(fieldIdentifier, out var validationResults)) { //or Model
            _messages.Clear(fieldIdentifier);
            
        } else {
            foreach (var result in ValidatorService.ValidationErrors) {
                _messages.Add(new FieldIdentifier(Model, result.PropertyName), result.ErrorMessage);
            }
        }
    
        CurrentEditContext.NotifyValidationStateChanged();
    }
    //private void OnFieldChanged(object? sender, FieldChangedEventArgs eventArgs)
    //{
    //    var fieldIdentifier = eventArgs.FieldIdentifier;
    //    if (_ValidatorService.Validate(fieldIdentifier, out var validationResults))
    //    {
    //        _messages.Clear(fieldIdentifier);
    //        foreach (var result in validationResults) {
    //            _messages.Add(fieldIdentifier, result.ErrorMessage!);
    //        }
    //
    //        CurrentEditContext.NotifyValidationStateChanged();
    //    }
    //}

    // working pretty good, except by it's not showing errors...
    //private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
    //{
    //    if (ValidatorService.Validate(Model, out List<ValidationResult> validationResults)) {
    //        _messages.Clear();
    //    }
    //
    //    foreach (var validationResult in ValidatorService.ValidationErrors) {
    //        _messages.Add(CurrentEditContext.Field(validationResult.PropertyName), validationResult.ErrorMessage);
    //    }
    //
    //    CurrentEditContext.NotifyValidationStateChanged();
    //}

    ///<summary>
    /// Improve event handler for validation event.
    ///
    ///<summary/>
    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        _ValidatorService.Validate(Model, out var validationResults);

        _messages.Clear();
        foreach (var result in validationResults)
        {
            if (result == null) {
                continue;
            }

            var hasMemberNames = false;
            foreach (var memberName in result.MemberNames) {
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