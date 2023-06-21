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
using static PlannerCRM.Shared.Constants.ExceptionsMessages;

namespace PlannerCRM.Client.Pages.OperationManager.Edit.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerEditActivity
{
    private readonly Logger<ActivityEditFormDto> _logger;

    public OperationManagerEditActivity(Logger<ActivityEditFormDto> logger) {
        _logger = logger;
    }

    public OperationManagerEditActivity() { }

    [Parameter] public RenderFragment Content { get; set; }
    [CascadingParameter(Name = "WorkOrderId")] public int WorkOrderId { get; set; }
    [CascadingParameter(Name = "ActivityId")] public int ActivityId { get; set; }
    
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; }

    private ActivityEditFormDto _Model = new();
    private EditContext _EditContext { get; set; }
    private ActivitySelectHelperDto _SelectModel = new();
    private List<WorkOrderSelectDto> _WorkOrders = new();
    private WorkOrderViewDto _CurrentWorkOrder = new();
    private List<EmployeeSelectDto> _Employees = new();
    private bool _IsError { get; set; }
    private string _Message { get; set; }    

    private string _CurrentPage { get; set; }
    private bool _HasElements { get; set; }
    private bool _IsCancelClicked { get; set; }
    private bool _HideWorkOrdersList { get; set; } = false;
    
    protected override async Task OnInitializedAsync() {
        _Model = await OperationManagerService.GetActivityForEditAsync(ActivityId);
        _CurrentWorkOrder = await OperationManagerService.GetWorkOrderForViewAsync(WorkOrderId);
    }

    protected override void OnInitialized() {
        _EditContext = new(_Model);
        _Model.ViewEmployeeActivity = new();
        _Model.EmployeeActivity = new();
        _HideWorkOrdersList = true;
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    }

    public async Task OnClickSearchWorkorder(string workorder) { 
        if (!string.IsNullOrEmpty(workorder)) {
            _WorkOrders = await OperationManagerService.SearchWorkOrderAsync(workorder);
            _HasElements = _WorkOrders.Any();
            if (_HasElements) {
                Toggle();
            } else {
                _Message = WORKORDER_NOT_FOUND;
            }
        }
    }

    public void Toggle() {
        _HideWorkOrdersList = !_HideWorkOrdersList;
    }

    private void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
    }

    public async Task OnClickSearchEmployee(string employee) {
        _Employees = await OperationManagerService.SearchEmployeeAsync(employee);
        if (_Employees.Any()) {
            _HasElements = true;
            _Message = EMPLOYEE_NOT_FOUND;
        }
    }

    private void OnClickAddAsSelected(EmployeeSelectDto employee) {
        try {
            var contains = _Model.ViewEmployeeActivity
                .Any(ea => ea.EmployeeId == employee.Id);

            if(!contains) {
                var item =  new EmployeeActivityDto {
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
                        StartDate = _Model.StartDate,
                        FinishDate = _Model.FinishDate,
                        WorkOrderId = _Model.WorkOrderId, 
                    }
                };
                _Model.EmployeeActivity.Add(item);
                _Model.ViewEmployeeActivity.Add(item);
            }
        } catch (NullReferenceException nullRefExc) {
            _logger.Log(LogLevel.Error, nullRefExc.Message);
            _Message = nullRefExc.Message;
            _IsError = true;
        }
        catch (Exception exc) {
            _logger.Log(LogLevel.Error, exc.Message);
            _Message = exc.Message;
            _IsError = true;
        } finally {
            Toggle();
        }
    }

    private void OnClickRemoveAsSelected(EmployeeSelectDto employee) {
        foreach (var ea in _Model.ViewEmployeeActivity.ToHashSet()) {
            if (ea.Employee.Email == employee.Email) {
                _Model.EmployeeActivity.Remove(ea);
                _Model.ViewEmployeeActivity.Remove(ea);
            }
        }
    }
    //trovare un modo per mandare la lista con gli impiegati che sono stati aggiunti e non anche con quelli vecchi
    
    public void RedirectToPage() {
        NavManager.NavigateTo("/operation-manager");
    }

    public async Task OnClickModalConfirm() {
        try
        {
            if (_EditContext.IsModified() && _EditContext.Validate()) {

                var response = await OperationManagerService.EditActivityAsync(_Model);
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