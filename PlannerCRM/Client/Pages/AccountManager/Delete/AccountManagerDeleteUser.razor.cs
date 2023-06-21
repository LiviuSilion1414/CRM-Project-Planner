using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.AccountManager.Delete;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManagerDeleteUser
{
    [Parameter] public int Id { get; set; }
    
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private AccountManagerCrudService AccountManagerService { get; set; }
    
    private bool _IsCancelClicked { get; set; } = false;
    public string Title { get; set; }    
    public string Message { get; set; } 

    private string _CurrentPage { get; set; }
    private bool _IsError { get; set; }
    private string _Message { get; set; }
    private EmployeeDeleteDto _Model = new();
    private List<string> _ErrorMessages = new();

    protected override async Task OnInitializedAsync() {
        _Model = await AccountManagerService.GetEmployeeForDeleteAsync(Id);
    }

    protected override void OnInitialized() {
        _CurrentPage = NavigationManager.Uri.Replace(NavigationManager.BaseUri, "/");
    }

    public void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
        NavigationManager.NavigateTo(_CurrentPage);
    }

    public async Task OnClickModalConfirm() {
        var responseEmployee = await AccountManagerService.DeleteEmployeeAsync(Id);
        var responseUser = await AccountManagerService.DeleteUserAsync(_Model.Email);
        
        if (!responseEmployee.IsSuccessStatusCode || !responseUser.IsSuccessStatusCode) {
            _Message = await responseEmployee.Content.ReadAsStringAsync();
            _IsError = true;
        } 
        
        _IsCancelClicked = !_IsCancelClicked;
        NavigationManager.NavigateTo("/account-manager", true);
    }
}
