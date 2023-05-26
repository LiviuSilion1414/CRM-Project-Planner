using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.AccountManager.Edit;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManagerEditUserForm
{   
    private readonly Logger<EmployeeEditFormDto> _logger;

    public AccountManagerEditUserForm(Logger<EmployeeEditFormDto> logger) {
        _logger = logger;
    }

    public AccountManagerEditUserForm() { }

    [Parameter] public int Id { get; set; }
    [Inject] private AccountManagerCrudService AccountManagerService { get; set;}
    [Inject] private NavigationLockService NavLockService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }
    
    private EmployeeEditFormDto _Model = new();
    private EditContext _EditContext { get; set; }
    private string _TypeField = InputType.PASSWORD.ToString().ToLower();
    private string _CurrentEmail { get; set; }
    private bool _IsCheckboxClicked { get; set; }
    private bool _IsError { get; set; }
    private string _Message { get; set; }

    protected override async Task OnInitializedAsync() {
        _Model = await AccountManagerService.GetEmployeeForEditAsync(Id);
        _CurrentEmail = _Model.Email;   
    }

    protected override void OnInitialized() {
        _EditContext = new(_Model);
    }

    public void SwitchShowPassword() {
        if (_IsCheckboxClicked) {
            _IsCheckboxClicked = false;
            _TypeField = InputType.TEXT.ToString().ToLower();
        } else {
            _IsCheckboxClicked = true;
            _TypeField = InputType.PASSWORD.ToString().ToLower();
        }
    }

    public void RedirectToPage() {
        NavManager.NavigateTo("/account-manager");
    }

    public void OnClickCancel() {
        RedirectToPage();
    }

    public async void OnClickConfirm() {
        try {
            _Model.EmployeeSalaries = new();
            _Model.EmployeeSalaries
                .Add(new EmployeeSalaryDto {
                    Id = _Model.Id,
                    EmployeeId = _Model.Id,
                    Salary = _Model.HourlyRate,
                    StartDate = _Model.StartDateHourlyRate 
                        ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.StartDateHourlyRate)} non può essere null."""),
                    FinishDate = _Model.FinishDateHourlyRate 
                        ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.FinishDateHourlyRate)} non può essere null."""),
                });
    
            var responseUser = await AccountManagerService.UpdateUserAsync(_Model, _CurrentEmail);
            var responseEmployee = await AccountManagerService.UpdateEmployeeAsync(_Model);
    
            if (!responseUser.IsSuccessStatusCode) {
                _Message = await responseUser.Content.ReadAsStringAsync();
            } else if (!responseUser.IsSuccessStatusCode) {
                _Message = await responseEmployee.Content.ReadAsStringAsync();
            } else {
                RedirectToPage();
            }
            _IsError = true;
        } catch (NullReferenceException nullRefExc) {
            _logger.Log(LogLevel.Error, nullRefExc.Message);
            _Message = nullRefExc.Message;
            _IsError = true;
        } catch (Exception exc) {
            _logger.Log(LogLevel.Error, exc.Message);
            _Message = exc.Message;
            _IsError = true;
        }
    }
}