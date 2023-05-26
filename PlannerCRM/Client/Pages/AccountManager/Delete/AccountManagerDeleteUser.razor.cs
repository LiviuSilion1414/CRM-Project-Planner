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
    
    private bool _IsError { get; set; }
    private string _Message { get; set; }
    private EmployeeDeleteDto _Model = new();
    private List<string> _ErrorMessages = new();

    protected override async Task OnInitializedAsync() {
        _Model = await AccountManagerService.GetEmployeeForDeleteAsync(Id);
    }

    public void RedirectToPage() {
        NavigationManager.NavigateTo("/account-manager");
    }

    public void OnClickCancel() {
        RedirectToPage();
    }

    public async Task OnClickDeleteUser() {
        var responseEmployee = await AccountManagerService.DeleteEmployeeAsync(Id);
        var responseUser = await AccountManagerService.DeleteUserAsync(_Model.Email);
        
        if (!responseEmployee.IsSuccessStatusCode && !responseUser.IsSuccessStatusCode) {
            _IsError = true;
            _Message = await responseEmployee.Content.ReadAsStringAsync();
            _ErrorMessages.Add(_Message);
            _Message = await responseUser.Content.ReadAsStringAsync();
            _ErrorMessages.Add(_Message);
        } else {
            RedirectToPage();
        }
    }
}
