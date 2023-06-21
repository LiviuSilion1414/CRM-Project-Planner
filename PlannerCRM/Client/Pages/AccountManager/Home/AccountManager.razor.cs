using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Client.Pages.AccountManager.Home;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManager
{
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private AccountManagerCrudService AccountManagerService { get; set; }

    private bool _IsError { get; set; }
    private string _Message { get; set; }
    private int _CollectionSize { get; set; }
    private int _TotalPageNumbers { get; set; }
    private int _PageNumber { get; set; } = ONE;
    private int _Skip { get; set; } = ZERO;
    private int _Take { get => PAGINATION_LIMIT; }

    private bool _IsViewClicked { get; set; }
    private bool _IsAddClicked { get; set; }
    private bool _IsEditClicked { get; set; }
    private bool _IsDeleteClicked { get; set; }

    private int _UserId { get; set; }

    public List<EmployeeViewDto> _users { get; set; } = new();

    protected override async Task OnInitializedAsync() {
        _users = await AccountManagerService.GetPaginatedEmployees(_Skip, _Take);
        _CollectionSize = await AccountManagerService.GetEmployeesSize();
        _TotalPageNumbers = (_CollectionSize / PAGINATION_LIMIT);
    }
    
    public async Task Previous(int pageNumber) {
        if (_Skip <= PAGINATION_LIMIT) {
            _Skip = ZERO;
            _PageNumber = ONE;
        } else {
            _Skip -= (_Skip - PAGINATION_LIMIT);
            _PageNumber--;
        }
        _users = await AccountManagerService.GetPaginatedEmployees(_Skip, _Take);
    }

    public async Task Next(int pageNumber) {
        if (_Skip < _TotalPageNumbers + PAGINATION_LIMIT) {
            _Skip += PAGINATION_LIMIT;
            _PageNumber++; 
        } else {
            _Skip = _TotalPageNumbers;
            _PageNumber = _TotalPageNumbers + ONE;
        }
        _users = await AccountManagerService.GetPaginatedEmployees(_Skip, _Take);
    }

    public void ShowDetails(int id) {
        _IsViewClicked = !_IsViewClicked;
        _UserId = id;
    }

    public void OnClickAddUser() {
        _IsAddClicked = !_IsAddClicked;
    }

    public void OnClickEdit(int id) {
        _IsEditClicked = !_IsEditClicked;
        _UserId = id;
    }

    public void OnClickDelete(int id) {
        _IsDeleteClicked = !_IsDeleteClicked;
        _UserId = id;
    }
}