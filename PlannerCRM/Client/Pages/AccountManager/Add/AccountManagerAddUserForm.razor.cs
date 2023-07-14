using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.AccountManager.Add;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManagerAddUserForm
{   
    private readonly Logger<EmployeeFormDto> _logger;
    
    public AccountManagerAddUserForm(
            Logger<EmployeeFormDto> logger) 
    {
        _logger = logger;
    }

    public AccountManagerAddUserForm() { }

    [Inject] private AccountManagerCrudService AccountManagerService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; }

    public EmployeeFormDto _Model = new();
    public EditContext _EditContext { get; set; }
    public bool _IsError { get; set; }
    public string _Message { get; set; }
    public bool _IsCheckboxClicked { get; set; }
    public string _CurrentPage { get; set; }
    public bool _IsCancelClicked { get; set; }
    public EventCallback _CancelCallback { get; set; }


    protected override void OnInitialized() {
        _EditContext = new(_Model);
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    }

    public void SwitchShowPassword() => _IsCheckboxClicked = !_IsCheckboxClicked;

    public void OnClickModalCancel() {
        NavManager.NavigateTo(_CurrentPage);
        _IsCancelClicked = !_IsCancelClicked;
    }

    public void OnClickInvalidSubmit() {
        System.Console.WriteLine("Clicked");
        System.Console.WriteLine("_IsError: {0}", _IsError);
        System.Console.WriteLine("Is invalid: {0}", _EditContext.Validate());
        _IsError = true;
        _Message = "Tutti i campi sono obbligatori, si prega di ricontrollare.";
        System.Console.WriteLine("_IsError: {0}", _IsError);
    }


    public async Task OnClickModalConfirm() {
        try {
            if (_EditContext.IsModified() && _EditContext.Validate()) {
                _Model.OldEmail = _Model.Email;
                _Model.EmployeeSalaries = new();
                _Model.EmployeeSalaries
                    .Add(new EmployeeSalaryDto {
                        Id = _Model.Id,
                        EmployeeId = _Model.Id,
                        Salary = _Model.CurrentHourlyRate,
                        StartDate = _Model.StartDateHourlyRate 
                            ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.StartDateHourlyRate)} non può essere null."""),
                        FinishDate = _Model.FinishDateHourlyRate 
                            ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.FinishDateHourlyRate)} non può essere null."""),
                    });

                var responseEmployee = await AccountManagerService.AddUserAsync(_Model);
                var responseUser = await AccountManagerService.AddEmployeeAsync(_Model);

                if (!responseEmployee.IsSuccessStatusCode || !responseUser.IsSuccessStatusCode) {
                    _Message = await responseEmployee.Content.ReadAsStringAsync();
                    _IsError = true;
                } else {
                    _IsCancelClicked = !_IsCancelClicked;
                    NavManager.NavigateTo(_CurrentPage, true);
                }
            } else {
                _IsCancelClicked = !_IsCancelClicked;
                NavManager.NavigateTo(_CurrentPage);
            }
        } catch (NullReferenceException exc) {
            _logger.Log(LogLevel.Error, exc, exc.Message);
            _Message = exc.Message;
            _IsError = true;
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc,  exc.Message);
            _Message = exc.Message;
            _IsError = true;
        }
    }
}
