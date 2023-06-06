using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.AccountManager.Details;

public partial class AccountManagerShowUserDetails
{
    [Parameter] public int Id { get; set; }

    [Inject] NavigationManager NavManager { get; set; }
    [Inject] AccountManagerCrudService AccountManagerService { get; set; }

    private EmployeeViewDto _Model = new();
    private string _TypeField { get; set; } = InputType.PASSWORD.ToString().ToLower();
    private bool _IsCheckboxClicked { get; set; }

    protected override async Task OnInitializedAsync() {
        _Model = await AccountManagerService.GetEmployeeForViewAsync(Id);
    }

    public void OnClickCancel() {
        NavManager.NavigateTo("/account-manager");
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
}