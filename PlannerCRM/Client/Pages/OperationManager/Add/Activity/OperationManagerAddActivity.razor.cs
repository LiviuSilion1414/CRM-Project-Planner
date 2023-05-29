using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.Models;

using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using Microsoft.AspNetCore.Components.Forms;

namespace PlannerCRM.Client.Pages.OperationManager.Add.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerAddActivity
{
    private readonly Logger<ActivityFormDto> _logger;

    public OperationManagerAddActivity(Logger<ActivityFormDto> logger) {
        _logger = logger;
    }

    public OperationManagerAddActivity() { }

    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; }

    private ActivityFormDto _Model = new();
    private EditContext _EditContext { get; set; }
    private ActivitySelectHelperDto _SelectModel = new();
    private List<WorkOrderSelectDto> _WorkOrders = new();
    private List<EmployeeSelectDto> _Employees = new();
    private bool _IsError { get; set; }
    private string _Message { get; set; }

    protected override void OnInitialized() {
       _Model.EmployeesActivities = new();
       _EditContext = new(_Model);
    } 

    private async Task OnClickSearchWorkOrder(string workOrder) {
        if (!string.IsNullOrEmpty(workOrder)) {
            _WorkOrders = await OperationManagerService.SearchWorkOrderAsync(workOrder);
        }
    }
    private void OnClickSetWorkOrder(WorkOrderSelectDto workOrderSelect) {
        _Model.WorkOrderId = workOrderSelect.Id;
        _SelectModel.SelectedWorkorder = workOrderSelect.Name;
    }

    private async Task OnClickSearchEmployee(string employee) {
        _Employees = await OperationManagerService.SearchEmployeeAsync(employee);
    }

    private void OnClickAddAsSelected(EmployeeSelectDto employee) {
        try {
            var isNotContained = true;

            foreach (var ea in _Model.EmployeesActivities) {
                if (ea.Employee.Email == employee.Email) {
                    isNotContained = false;
                }
            }

            if (isNotContained) {
                _Model.EmployeesActivities.Add(
                    new EmployeeActivityDto {
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
                });
            }
        } catch (NullReferenceException nullRefExc) {
            _logger.Log(LogLevel.Error, nullRefExc.Message);
            _Message = nullRefExc.Message;
            _IsError = true;
        }
    }

    private void OnClickRemoveAsSelected(EmployeeSelectDto employee) {
        foreach (var ea in _Model.EmployeesActivities.ToList()) {
            if (ea.Employee.Email == employee.Email) {
                _Model.EmployeesActivities.Remove(ea);
            }
        }
    }
    
    private void RedirectToPage() {
        NavManager.NavigateTo("/operation-manager");
    }

    private void OnClickCancel() {
        RedirectToPage();
    }

    private async Task OnClickAddActivity() {
        var response = await OperationManagerService.AddActivityAsync(_Model);
        
        if (!response.IsSuccessStatusCode) {
            _IsError = true;
            _Message = await response.Content.ReadAsStringAsync();
        } else {
            RedirectToPage();
        }
        
    }
}