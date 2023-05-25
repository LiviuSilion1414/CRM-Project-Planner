using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.AccountManager.Home;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManager
{
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private AccountManagerCrudService AccountManagerService { get; set; }

    private bool _IsError { get; set; }
    private string _Message { get; set; }

    public List<EmployeeViewDto> _users = new();  

    protected override async Task OnInitializedAsync() {
        _users = await AccountManagerService.GetAllEmployeesAsync();
    }
    
    public void OnClickAddUser() {
        NavManager.NavigateTo("/account-manager/add/user");
    }

    public void OnClickEdit(int id) {
        NavManager.NavigateTo($"/account-manager/edit/user/{id}");
    }

    public void OnClickDelete(int id) {
        NavManager.NavigateTo($"/account-manager/delete/user/{id}");
    }
}