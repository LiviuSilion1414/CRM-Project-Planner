namespace PlannerCRM.Client.Pages.OperationManager.Delete.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalDeleteActivity : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public int WorkOrderId { get; set; }
    [Parameter] public int ActivityId { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }    
    [Inject] public NavigationManager NavigationManager { get; set; }
    
    private WorkOrderViewDto _currentWorkOrder;
    private ActivityDeleteDto _currentActivity;

    private bool _isCancelClicked;
    public string _message; 

    private string _currentPage;
    private bool _isError;

    protected override async Task OnInitializedAsync() {
        _currentWorkOrder = await OperationManagerService.GetWorkOrderForViewAsync(WorkOrderId);
        _currentActivity = await OperationManagerService.GetActivityForDeleteAsync(ActivityId);
    }

    protected override void OnInitialized() {
        _currentPage = NavigationManager.Uri.Replace(NavigationManager.BaseUri, "/");
        _currentWorkOrder = new();
        _currentActivity = new() {
            Employees = new()
        };
    }

    public void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
        NavigationManager.NavigateTo(_currentPage);
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
                NavigationManager.NavigateTo(_currentPage, true);
            }
        } catch (Exception exc) {
            _isError = true;
            _message = exc.Message;
        }
    }
}