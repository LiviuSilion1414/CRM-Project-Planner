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
    private bool _IsCheckboxClicked { get; set; }
    public string _CurrentPage { get; set; }
    public bool _IsCancelClicked { get; set; }
    protected bool _IS_DISABLED { get => true;}

    protected override async Task OnInitializedAsync() {
        _Model = await AccountManagerService.GetEmployeeForViewAsync(Id);
    }

    protected override void OnInitialized() {
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    }

    public void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
        NavManager.NavigateTo(_CurrentPage);
    }

    public void SwitchShowPassword() => _IsCheckboxClicked = !_IsCheckboxClicked;
}