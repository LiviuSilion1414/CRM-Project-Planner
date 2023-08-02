namespace PlannerCRM.Client.Pages.Modals.Form.Activity;

public partial class ModalFormActivity : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public ActivityFormDto Model { get; set; }
    [Parameter] public EventCallback<ActivityFormDto> GetValidatedModel { get; set; }

    [Inject] public Logger<ActivityFormDto> Logger { get; set; }
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public NavigationLockService NavLockService { get; set; }
    [Inject] public CustomDataAnnotationsValidator CustomValidator { get; set; }

    private Dictionary<string, List<string>> _errors;

    private EditContext _editContext;

    private List<WorkOrderSelectDto> _workOrders;
    private List<EmployeeSelectDto> _employees;

    private bool _isError;
    private string _message;
    private bool _isCancelClicked;
    
    private bool _workOrdersHasElements;
    private bool _hideWorkOrdersList;

    private bool _employeeHasElements; 
    private bool _hideEmployeesList;

    protected override void OnInitialized() {
        Model = new() {
            EmployeeActivity = new(),
            ViewEmployeeActivity = new(),
            DeleteEmployeeActivity = new(),
            SelectedEmployee = ""
        };
        _editContext = new(Model);
        _workOrders = new();
        _employees = new();
        _hideWorkOrdersList = true;
        _hideEmployeesList = true;
    } 

    private async Task OnClickSearchWorkOrder(string workOrder) {
        if (!string.IsNullOrEmpty(workOrder)) {
            _workOrders = await OperationManagerService.SearchWorkOrderAsync(workOrder);
            _workOrdersHasElements = _workOrders.Any();
            if (!_workOrdersHasElements) {
                _message = ExceptionsMessages.WORKORDER_NOT_FOUND;
            }
            ToggleWorkOrderListView();
        }
    }


    private void OnClickSetWorkOrder(WorkOrderSelectDto workOrderSelect) {
        Model.WorkOrderId = workOrderSelect.Id;
        Model.SelectedWorkorder = workOrderSelect.Name;
        ToggleWorkOrderListView();
    }
    
    private void OnClickModalCancel() => _isCancelClicked = !_isCancelClicked;

    private void ToggleWorkOrderListView() => _hideWorkOrdersList = !_hideWorkOrdersList;
    
    private void ToggleEmployeesListView() => _hideEmployeesList = !_hideEmployeesList;

    private async Task OnClickSearchEmployee(string employee) {
        _employees = await OperationManagerService.SearchEmployeeAsync(employee);
        _employeeHasElements = _employees.Any();
        if (!_employeeHasElements) {
            _message = ExceptionsMessages.EMPLOYEE_NOT_FOUND;
        } 
        ToggleEmployeesListView();
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    private void OnClickAddAsSelected(EmployeeSelectDto employee) {
        try { 
            var contains = Model.EmployeeActivity
                .Any(ea => ea.Employee.Email == employee.Email);
            
            if (!contains) {
                var activity = 
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
                                    ActivityId = Model.Id,
                                }
                            }
                        },
                        ActivityId = Model.Id,
                        Activity = new ActivitySelectDto {
                            Id = Model.Id,
                            Name = Model.Name,
                            StartDate = Model.StartDate 
                                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                            FinishDate = Model.FinishDate
                                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                            WorkOrderId = Model.WorkOrderId 
                                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG)
                        },
                        
                    };

                Model.EmployeeActivity.Add(activity);
                Model.ViewEmployeeActivity.Add(activity);
                Model.DeleteEmployeeActivity = new();
            }
            ToggleEmployeesListView();
        } catch (NullReferenceException exc) {
            Logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            _message = exc.Message;
            _isError = true;
        } catch (Exception exc) {
            Logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            _message = exc.Message;
            _isError = true;
        }
    }

    private void OnClickRemoveAsSelected(EmployeeSelectDto employee) {
        Model.DeleteEmployeeActivity = AddToDeleteEnumerable(Model.ViewEmployeeActivity, employee);
        
        RemoveFromEnumerable(Model.EmployeeActivity, employee);
        RemoveFromEnumerable(Model.ViewEmployeeActivity, employee);
    }    

    private static void RemoveFromEnumerable(HashSet<EmployeeActivityDto> employeeActivities, EmployeeSelectDto employee) {
        employeeActivities
            .Where(ea => ea.EmployeeId == employee.Id)
            .ToList()
            .ForEach(ea => employeeActivities.Remove(ea));
    }

    private static HashSet<EmployeeActivityDto> AddToDeleteEnumerable(HashSet<EmployeeActivityDto> lookupList, EmployeeSelectDto employee) {
        return lookupList
            .Where(ea => ea.EmployeeId == employee.Id)
            .ToHashSet();
    }

    private void OnClickInvalidSubmit() {
        _isError = true;
        _message = "Tutti i campi sono obbligatori, si prega di ricontrollare.";
    }

    private async Task OnClickModalConfirm() {
        var isValid = ValidatorService.Validate(Model, out _errors);

        if (isValid) {
            await GetValidatedModel.InvokeAsync(Model);
        } else {
            CustomValidator.DisplayErrors(_errors);
            OnClickInvalidSubmit();
        }
    }
}