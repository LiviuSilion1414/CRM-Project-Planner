namespace PlannerCRM.Client.Pages.OperationManager.Delete.FirmClient;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalDeleteClient : ComponentBase
{
    [Parameter] public int Id { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string ConfirmationMessage { get; set; }
    
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    
    private WorkOrderViewDto _workOrder = new();
    private ClientDeleteDto _model = new();
    private string _currentPage;
    private bool _isCancelClicked = false;
    
    private string _message; 
    private bool _isError;

    protected override async Task OnInitializedAsync() {
        _model = await OperationManagerService.GetClientForDeleteByIdAsync(Id);
        _workOrder = await OperationManagerService.GetWorkOrderForViewByIdAsync(_model.WorkOrderId);
    }

    protected override void OnInitialized()
        => _currentPage = NavigationUtil.GetCurrentPage();

    private void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
        NavigationManager.NavigateTo(_currentPage);
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    private async Task OnClickDelete() {
        var responseEmployee = await OperationManagerService.DeleteClientAsync(Id);
        
        if (!responseEmployee.IsSuccessStatusCode) {
            _message = await responseEmployee.Content.ReadAsStringAsync();
            _isError = true;
        } 
        
        _isCancelClicked = !_isCancelClicked;
        NavigationManager.NavigateTo(_currentPage, true);
    }
}
