using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Delete.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalDeleteActivity : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public int WorkOrderId { get; set; }
    [Parameter] public int ActivityId { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }    
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    
    private WorkOrderViewDto _currentWorkOrder = new();
    private ActivityDeleteDto _currentActivity = new() { Employees = [], Client = new() };

    private bool _isCancelClicked;
    public string _message; 

    private bool _isError;

    protected override async Task OnInitializedAsync() {
        Console.WriteLine($"{_currentActivity.Employees.Count == 0}: collection Employees of Activities entity has no elements");

        _currentWorkOrder = await OperationManagerService.GetWorkOrderForViewByIdAsync(WorkOrderId);
        _currentActivity = await OperationManagerService.GetActivityForDeleteByIdAsync(ActivityId);
    }

    public void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;    
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    public async Task OnClickModalConfirm() {
        try {
            var responseDelete = await OperationManagerService.DeleteActivityAsync(ActivityId);
    
            if (!responseDelete.IsSuccessStatusCode) {
                _message = await responseDelete.Content.ReadAsStringAsync();
                _isError = true;
            } else {
                _isCancelClicked = !_isCancelClicked;
            }
        } catch (Exception exc) {
            _isError = true;
            _message = exc.Message;
        }
    }
}