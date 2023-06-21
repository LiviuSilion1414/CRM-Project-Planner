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
    private readonly Logger<EmployeeEditFormDto> _logger;

    public AccountManagerEditUserForm(Logger<EmployeeEditFormDto> logger) {
        _logger = logger;
    }

    public AccountManagerEditUserForm() { }

    [Parameter] public int Id { get; set; }
    [Inject] private AccountManagerCrudService AccountManagerService { get; set;}
    [Inject] private NavigationLockService NavLockService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }
    
    private EmployeeEditFormDto _Model;
    private EditContext _EditContext { get; set; }
    private string _TypeField { get; set;} = InputType.PASSWORD.ToString().ToLower();
    private bool _IsCheckboxClicked { get; set; }
    private bool _IsError { get; set; }
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
        if (_IsCheckboxClicked) {
            _IsCheckboxClicked = false;
            _TypeField = InputType.TEXT.ToString().ToLower();
        } else {
            _IsCheckboxClicked = true;
            _TypeField = InputType.PASSWORD.ToString().ToLower();
        }
    }

    public void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
        NavManager.NavigateTo(_CurrentPage);
    }

    public async void OnClickModalConfirm() {
        try {
            if (_EditContext.IsModified() && _EditContext.Validate()) {       
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
                    _Message = await responseUser.Content.ReadAsStringAsync();
                    _IsError = true;
                } else {
                    _IsCancelClicked = !_IsCancelClicked;
                    NavManager.NavigateTo(_CurrentPage, true);
                }
                _IsError = true;
            } else {
                _IsCancelClicked = !_IsCancelClicked;
                NavManager.NavigateTo(_CurrentPage);
            }
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