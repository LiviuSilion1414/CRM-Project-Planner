using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.OperationManager.Edit.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerEditActivity
{
    private readonly Logger<ActivityFormDto> _logger;

    public OperationManagerEditActivity(Logger<ActivityFormDto> logger) {
        _logger = logger;
    }

    [Parameter] public int WorkOrderId { get; set; }
    [Parameter] public int ActivityId { get; set; }
    
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; }

    private ActivityFormDto _Model = new();
    private EditContext _EditContext { get; set; }
    private ActivitySelectHelperDto _SelectModel = new();
    private List<WorkOrderSelectDto> _WorkOrders = new();
    private WorkOrderViewDto _CurrentWorkOrder = new();
    private List<EmployeeSelectDto> _Employees = new();
    private bool _IsError { get; set; }
    private string _Message { get; set; }    
    
    protected override async Task OnInitializedAsync() {
        _Model.EmployeesActivities = new();

        _Model = await OperationManagerService.GetActivityForEditAsync(ActivityId);
        _CurrentWorkOrder = await OperationManagerService.GetWorkOrderForViewAsync(WorkOrderId);
        _EditContext = new(_Model);
    } 

    public async Task OnClickSearchWorkorder(string workorder) {
        _WorkOrders = await OperationManagerService.SearchWorkOrderAsync(workorder);
    }

    public void OnClickSetWorkorder(WorkOrderSelectDto workorderSelect) {
        _Model.WorkOrderId = workorderSelect.Id;
        _SelectModel.SelectedWorkorder = workorderSelect.Name;
    }

    public async Task OnClickSearchEmployee(string employee) {
        _Employees = await OperationManagerService.SearchEmployeeAsync(employee);
    }

    public void OnClickAddAsSelected(EmployeeSelectDto employee) {
        try {
            var employeeActivity = new EmployeeActivityDto {
                EmployeeId = employee.Id,
                Employee = employee,
                ActivityId = _Model.Id,
                Activity = new ActivitySelectDto {
                    Id = _Model.Id,
                    Name = _Model.Name,
                    StartDate = _Model.StartDate 
                        ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.StartDate)} non può essere null."""),
                    FinishDate = _Model.FinishDate 
                        ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.FinishDate)} non può essere null."""),
                    WorkOrderId = _Model.WorkOrderId 
                        ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.WorkOrderId)} non può essere null."""),
                },
            };

            if (!_Model.EmployeesActivities.Contains(employeeActivity)) {
                _Model.EmployeesActivities.Add(employeeActivity);
            }
        } catch (NullReferenceException nullRefExc) {
            _logger.Log(LogLevel.Error, nullRefExc.Message);
            _Message = nullRefExc.Message;
            _IsError = true;
        }
    }

    public void OnClickRemoveAsSelected(EmployeeSelectDto employee) {
        try {
            var employeeActivity = new EmployeeActivityDto {
                EmployeeId = employee.Id,
                Employee = employee,
                ActivityId = _Model.Id,
                Activity = new ActivitySelectDto {
                    Id = _Model.Id,
                    Name = _Model.Name,
                    StartDate = _Model.StartDate 
                        ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.StartDate)} non può essere null."""),
                    FinishDate = _Model.FinishDate 
                        ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.FinishDate)} non può essere null."""),
                    WorkOrderId = _Model.WorkOrderId 
                        ?? throw new NullReferenceException($"""Proprietà {nameof(_Model.WorkOrderId)} non può essere null."""),
                }
            };
    
            if (_Model.EmployeesActivities.Contains(employeeActivity)) {
                _Model.EmployeesActivities.Remove(employeeActivity);
            }
        } catch (NullReferenceException nullRefExc) {
            _logger.Log(LogLevel.Error, nullRefExc.Message);
            _Message = nullRefExc.Message;
            _IsError = true;
        }
    }
    
    public void RedirectToPage() {
        NavManager.NavigateTo("/operation-manager");
    }

    public async Task OnClickEditActivity() {
        var response = await OperationManagerService.EditActivityAsync(_Model);

        if (!response.IsSuccessStatusCode) {
            _IsError = true;
            _Message = await response.Content.ReadAsStringAsync();
        } else {
            RedirectToPage();
        }
    }

    public void OnClickCancel() {
        RedirectToPage();
    }
}