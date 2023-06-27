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
using static PlannerCRM.Shared.Constants.ExceptionsMessages;
namespace PlannerCRM.Client.Pages.OperationManager.Add.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerAddActivity
{
    private readonly Logger<ActivityAddFormDto> _logger;

    public OperationManagerAddActivity(Logger<ActivityAddFormDto> logger) {
        _logger = logger;
    }

    public OperationManagerAddActivity() { }

    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; }

    private ActivityAddFormDto _Model = new();
    private EditContext _EditContext { get; set; }
    private ActivitySelectHelperDto _SelectModel = new();
    private List<WorkOrderSelectDto> _WorkOrders = new();
    private List<EmployeeSelectDto> _Employees = new();
    private bool _IsError { get; set; }
    private string _Message { get; set; }
    private string _CurrentPage { get; set; }
    private bool _IsCancelClicked { get; set; }
    private bool _WorkOrdersHasElements { get; set; } 
    private bool _EmployeeHasElements { get; set; } 
    private bool _HideWorkOrdersList { get; set; }
    private bool _HideEmployeesList { get; set; }

    protected override void OnInitialized() {
        _EditContext = new(_Model);
        _Model.EmployeeActivity = new();
        _HideWorkOrdersList = true;
        _HideEmployeesList = true;
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    } 

    private async Task OnClickSearchWorkOrder(string workOrder) {
        if (!string.IsNullOrEmpty(workOrder)) {
            _WorkOrders = await OperationManagerService.SearchWorkOrderAsync(workOrder);
            _WorkOrdersHasElements = _WorkOrders.Any();
            if (!_WorkOrdersHasElements) {
                _Message = WORKORDER_NOT_FOUND;
            }
            ToggleWorkOrderListView();
        }
    }
    private void OnClickSetWorkOrder(WorkOrderSelectDto workOrderSelect) {
        _Model.WorkOrderId = workOrderSelect.Id;
        _SelectModel.SelectedWorkorder = workOrderSelect.Name;
        ToggleWorkOrderListView();
    }

    public void ToggleWorkOrderListView() {
        _HideWorkOrdersList = !_HideWorkOrdersList;
    }
    
    public void ToggleEmployeesListView() {
        _HideEmployeesList = !_HideEmployeesList;
    }

    private async Task OnClickSearchEmployee(string employee) {
        _Employees = await OperationManagerService.SearchEmployeeAsync(employee);
        _EmployeeHasElements = _Employees.Any();
        if (!_EmployeeHasElements) {
            _Message = EMPLOYEE_NOT_FOUND;
        } 
        ToggleEmployeesListView();
    }

    private void OnClickAddAsSelected(EmployeeSelectDto employee) {
        try {
            var contains = _Model.EmployeeActivity.Any(ea => ea.Employee.Email == employee.Email);
            
            if (!contains) {
                _Model.EmployeeActivity.Add(
                    new EmployeeActivityDto {
                        EmployeeId = employee.Id,
                        Employee = new EmployeeSelectDto {
                            Id = employee.Id,
                            Email = employee.Email,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            FullName = employee.FullName,
                            Role = employee.Role,
                            EmployeeActivities = new List<EmployeeActivityDto>() {
                                new EmployeeActivityDto() {
                                    EmployeeId = employee.Id,
                                    ActivityId = _Model.Id,
                                }
                            }
                        },
                        ActivityId = _Model.Id,
                        Activity = new ActivitySelectDto {
                            Id = _Model.Id,
                            Name = _Model.Name,
                            StartDate = _Model.StartDate ?? DateTime.Now,
                            FinishDate = _Model.FinishDate ?? DateTime.Now.AddMonths(4),
                            WorkOrderId = _Model.WorkOrderId ?? throw new NullReferenceException(NULL_PROP)
                        }
                    }
                );
            }
            ToggleEmployeesListView();
        } catch (NullReferenceException nullRefExc) {
            _logger.LogError(nullRefExc, nullRefExc.Message);
            _Message = nullRefExc.Message;
            _IsError = true;
        } catch (Exception exc) {
            _logger.Log(LogLevel.Error, exc, exc.Message);
            _Message = exc.Message;
            _IsError = true;
        }
    }

    private void OnClickRemoveAsSelected(EmployeeSelectDto employee) {
        _Model.EmployeeActivity
            .ToList()
            .ForEach(ea => {
                if (ea.Employee.Id == employee.Id) 
                    _Model.EmployeeActivity.Remove(ea);
            }
        );
    }

    private void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
    }

    private async Task OnClickModalConfirm() {
        try {
            Console.WriteLine($"EmployeeActivity is null ? { _Model.EmployeeActivity is null}");
            Console.WriteLine($"EmployeeActivity is empty ? {!_Model.EmployeeActivity.Any()}");
            Console.WriteLine("EmployeeActivity items count {0}", _Model.EmployeeActivity.Count());

            foreach (var item in _Model.EmployeeActivity) {
                Console.WriteLine("Item is {0}", item.Employee.FullName);
            }
            if (_EditContext.IsModified() && _EditContext.Validate()) {

                var response = await OperationManagerService.AddActivityAsync(_Model);
                
                if (!response.IsSuccessStatusCode) {
                    _IsError = true;
                    _Message = await response.Content.ReadAsStringAsync();
                } else {
                    _IsCancelClicked = !_IsCancelClicked;
                    NavManager.NavigateTo(_CurrentPage, true);
                }
            } else {
                _IsCancelClicked = !_IsCancelClicked;
                NavManager.NavigateTo(_CurrentPage);
            }
        } catch (Exception exc) {
            _IsError = true;
            _Message = exc.Message;
        }
    }
}