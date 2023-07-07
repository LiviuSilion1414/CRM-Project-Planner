using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;
using static PlannerCRM.Shared.Constants.ExceptionsMessages;

namespace PlannerCRM.Client.Pages.AccountManager.Edit;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManagerEditUserForm
{   
    private readonly Logger<EmployeeFormDto> _logger;

    public AccountManagerEditUserForm(Logger<EmployeeFormDto> logger) {
        _logger = logger;
    }

    public AccountManagerEditUserForm() { }

    [Parameter] public int Id { get; set; }
    [Inject] private AccountManagerCrudService AccountManagerService { get; set;}
    [Inject] private NavigationLockService NavLockService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private ValidatorService CustomValidatorService { get; set; }
    ValidationMessage<string> ErrorMessage { get; set; }
    List<ValidationMessage<string>> ErrorMessages { get; set; }

    private EmployeeFormDto _Model;
    private EditContext _EditContext { get; set; }
    private ValidationMessageStore _MessageStore { get; set; }
    private bool _IsCheckboxClicked { get; set; }
    private bool _IsError { get; set; } = false;
    private string _Message { get; set; }
    private string _CurrentPage { get; set; }
    private bool _IsCancelClicked { get; set; }

    protected override async Task OnInitializedAsync() {
        _Model = await AccountManagerService.GetEmployeeForEditAsync(Id);
    }

    protected override void OnInitialized() {
        _Model = new();
        _EditContext = new(_Model);
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    }

    public void SwitchShowPassword() {
        _IsCheckboxClicked = !_IsCheckboxClicked;
    }

    public void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
        NavManager.NavigateTo(_CurrentPage);
    }

    public void OnClickInvalidSubmit() {
        _IsError = true;
        _Message = "Tutti i campi sono obbligatori, si prega di ricontrollare.";
    }

    public async void OnClickModalConfirm() {
        try {
            var r = CustomValidatorService.Validate(_Model, out var validationResults);
            var isValid = !validationResults.Any();
            if (isValid) {
                System.Console.WriteLine("is valid");
                _Model.EmployeeSalaries = new();
                _Model.EmployeeSalaries
                    .Add(new EmployeeSalaryDto {
                        Id = _Model.Id,
                        EmployeeId = _Model.Id,
                        Salary = _Model.CurrentHourlyRate ?? throw new NullReferenceException(NULL_PROP),
                        StartDate = _Model.StartDateHourlyRate 
                            ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.StartDateHourlyRate)} non può essere null."""),
                        FinishDate = _Model.FinishDateHourlyRate 
                            ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.FinishDateHourlyRate)} non può essere null."""),
                    });
                var responseUser = await AccountManagerService.UpdateUserAsync(_Model);
                var responseEmployee = await AccountManagerService.UpdateEmployeeAsync(_Model);

                if (!responseUser.IsSuccessStatusCode || !responseUser.IsSuccessStatusCode) {
                    _IsError = true;
                    _Message = "Tutti i campi sono obbligatori, si prega di ricontrollare.";
                } else {
                    _IsCancelClicked = !_IsCancelClicked;
                    NavManager.NavigateTo(_CurrentPage, true);
                }
                _IsError = true;
            } else {
                OnClickInvalidSubmit();
                _IsError = false;
            }
        } catch (NullReferenceException nullRefExc) {
            //_logger.Log(LogLevel.Error, nullRefExc.Message);
            System.Console.WriteLine("hit: {0}, innerException: {1}", nullRefExc.Message, nullRefExc.InnerException);
            System.Console.WriteLine("err: {0}", nullRefExc.StackTrace, nullRefExc.Source);
            _Message = nullRefExc.Message;
            _IsError = true;
            
        } catch (Exception exc) {
            System.Console.WriteLine("Exception: {0}, Name: {1}", exc.StackTrace, exc.GetType().ToString());
           // _logger.LogError(new EventId(), exc, exc.Message, new string[] { exc.Message });
            _Message = exc.Message;
            _IsError = true;
        }
    }
}