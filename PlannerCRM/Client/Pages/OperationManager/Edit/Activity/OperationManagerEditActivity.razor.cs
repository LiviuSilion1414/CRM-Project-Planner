using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Pages.ValidatorComponent;
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
    private readonly Logger<ActivityFormDto> _logger;

    public OperationManagerEditActivity(Logger<ActivityFormDto> logger) {
        _logger = logger;
    }

    public OperationManagerEditActivity() { }

    [Parameter] public RenderFragment Content { get; set; }
    [CascadingParameter(Name = "WorkOrderId")] public int WorkOrderId { get; set; }
    [CascadingParameter(Name = "ActivityId")] public int ActivityId { get; set; }
    
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; }

    private CustomDataAnnotationsValidator _CustomValidator { get; set; }
    private Dictionary<string, List<string>> Errors;

    private ActivityFormDto _Model = new();
    private EditContext _EditContext { get; set; }
    private ActivitySelectHelperDto _SelectModel = new();
    private List<WorkOrderSelectDto> _WorkOrders = new();
    private WorkOrderViewDto _CurrentWorkOrder = new();
    private List<EmployeeSelectDto> _Employees = new();
    private bool _IsError { get; set; }
    private string _Message { get; set; }    

    private string _CurrentPage { get; set; }
    private bool _HasElements { get; set; }
    private bool _EmployeeHasElements { get; set; } 
    private bool _HideEmployeesList { get; set; }
    private bool _IsCancelClicked { get; set; }
    private bool _IS_DISABLED { get => true; }
    
    protected override async Task OnInitializedAsync() {
        _Model = await OperationManagerService.GetActivityForEditAsync(ActivityId);
        _CurrentWorkOrder = await OperationManagerService.GetWorkOrderForViewAsync(WorkOrderId);
    }

    protected override void OnInitialized() {
        _EditContext = new(_Model);
        _Model.ViewEmployeeActivity = new();
        _Model.EmployeeActivity = new();
        _HideEmployeesList = true;
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    }
    
    public void ToggleEmployeesListView() {
        _HideEmployeesList = !_HideEmployeesList;
    }

    private void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
    }

    public async Task OnClickSearchEmployee(string employee) {
        _Employees = await OperationManagerService.SearchEmployeeAsync(employee);
        _EmployeeHasElements = _Employees.Any();
        if (!_EmployeeHasElements) {
            _HasElements = true;
            _Message = EMPLOYEE_NOT_FOUND;
        }
        ToggleEmployeesListView();
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
                        StartDate = _Model.StartDate ?? throw new NullReferenceException(NULL_PROP),
                        FinishDate = _Model.FinishDate ?? throw new NullReferenceException(NULL_PROP),
                        WorkOrderId = _Model.WorkOrderId ?? throw new NullReferenceException(NULL_PROP), 
                    }
                };
                _Model.EmployeeActivity.Add(item);
                _Model.ViewEmployeeActivity.Add(item);
            }
            ToggleEmployeesListView();
        } catch (NullReferenceException exc) {
            _logger.Log(LogLevel.Error, exc, exc.Message);
            _Message = exc.Message;
            _IsError = true;
        } catch (Exception exc) {
            _logger.Log(LogLevel.Error, exc, exc.Message);
            _Message = exc.Message;
            _IsError = true;
        }
    }

    private void OnClickRemoveAsSelected(EmployeeSelectDto employee) {
        _Model.ViewEmployeeActivity
            .ToList()
            .ForEach(ea => {
                if (ea.Employee.Id == employee.Id) {
                    _Model.ViewEmployeeActivity.Remove(ea);
                    _Model.EmployeeActivity.Remove(ea);
                }
            }
        );
    }
    
    public async Task OnClickModalConfirm() {
        try
        {
            var isValid = ValidatorService.ValidateModel(_Model, out Errors);
            if (isValid) {
                Console.WriteLine("is valid");
                var response = await OperationManagerService.EditActivityAsync(_Model);
                if (!response.IsSuccessStatusCode) {
                    _IsError = true;
                    _Message = await response.Content.ReadAsStringAsync();
                } else {
                    _IsCancelClicked = !_IsCancelClicked;
                    NavManager.NavigateTo(_CurrentPage, true);
                }
            } else {
                _CustomValidator.DisplayErrors(Errors);
            }
        } catch (Exception exc) {
            _IsError = true;
            _Message = exc.Message;          
        }
    }
}