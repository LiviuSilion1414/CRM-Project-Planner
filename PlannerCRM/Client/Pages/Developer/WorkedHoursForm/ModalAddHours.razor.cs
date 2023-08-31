namespace PlannerCRM.Client.Pages.Developer.WorkedHoursForm;

[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
public partial class ModalAddHours : ComponentBase
{
    [Parameter] public int EmployeeId { get; set; }
    [Parameter] public int ActivityId { get; set; }
    
    [Inject] public CurrentUserInfoService CurrentUserInfoService  { get; set; }
    [Inject] public DeveloperService DeveloperService { get; set; }
    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public CustomDataAnnotationsValidator CustomValidator { get; set; }
    
    private readonly bool _disabled = true;
    
    private Dictionary<string, List<string>> _errors;
    
    private WorkTimeRecordFormDto _model;
    private WorkOrderViewDto _workOrder;
    private EditContext _editContext;
    private ActivityViewDto _activity;

    private string _employeeRole;

    private bool _isError;
    private string _message;

    public bool _isCancelClicked;
    private string _currentPage;

    protected override async Task OnInitializedAsync() {
        _employeeRole = await CurrentUserInfoService.GetCurrentUserRoleAsync();
        foreach (var possibleRole in Enum.GetValues(typeof(Roles))) {
            if ((possibleRole as string == nameof(Roles.SENIOR_DEVELOPER) && possibleRole as string == _employeeRole) || 
                (possibleRole as string == nameof(Roles.JUNIOR_DEVELOPER) && possibleRole as string == _employeeRole)) {
                _employeeRole = possibleRole as string;
            }
        }
        _model.Employee = await AccountManagerService.GetEmployeeForViewByIdAsync(EmployeeId);
        _activity = await DeveloperService.GetActivityByIdAsync(ActivityId);
        _workOrder = await DeveloperService.GetWorkOrderByIdAsync(_activity.WorkOrderId);
    }
    
    protected override void OnInitialized() {
        _model = new();
        _workOrder = new();
        _activity = new();
        _errors = new();
        _editContext = new(_model);
        _currentPage = _currentPage = NavigationUtil.GetCurrentPage();
    }

    public void RedirectToPage() {
        foreach (var possibleRole in Enum.GetValues(typeof(Roles))) {
            if ((possibleRole as string == _employeeRole) && (possibleRole as string == _employeeRole) && 
                (possibleRole as string == nameof(Roles.SENIOR_DEVELOPER))) {
                
                NavigationManager.NavigateTo($"/senior-developer/{EmployeeId}");
            }
            if ((possibleRole as string == _employeeRole) && (possibleRole as string == _employeeRole) && 
                (possibleRole as string == nameof(Roles.JUNIOR_DEVELOPER))) {
                
                NavigationManager.NavigateTo($"/junior-developer/{EmployeeId}");
            }
        }
    }

    private void Toggle() =>  _isCancelClicked = !_isCancelClicked;
    
    public void OnClickModalCancel() {
       Toggle();
    }

    public async Task OnClickModalConfirm() {
        try {
            var isValid = ValidatorService.Validate(_model, out _errors);

            if (isValid) {
                Console.WriteLine("isValid");
                _model.Date = DateTime.Now;
                _model.ActivityId = _activity.Id;
                _model.EmployeeId = EmployeeId;
                _model.WorkOrderId = _workOrder.Id;
        
                var response = await DeveloperService.AddWorkedHoursAsync(_model);
                
                if (!response.IsSuccessStatusCode) {
                    _isError = true;
                    _message = await response.Content.ReadAsStringAsync();
                } else {
                    Toggle();
                    NavigationManager.NavigateTo(_currentPage, true);
                }
            } else {
                CustomValidator.DisplayErrors(_errors);
                _isError = true;
                _message = ExceptionsMessages.EMPTY_FIELDS;
            }
        } catch (Exception exc) {
            _isError = true;
            _message = exc.Message;
        }
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;
}