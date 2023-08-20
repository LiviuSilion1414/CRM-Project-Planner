namespace PlannerCRM.Client.Pages.OperationManager.Delete.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalDeleteWorkOrder : ComponentBase
{
    [Parameter] public int Id { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    
    private bool _isCancelClicked;
    private string _message;

    private string _currentPage;
    private bool _isError;

    private WorkOrderDeleteDto _model;

    protected override async Task OnInitializedAsync() =>
        _model = await OperationManagerService.GetWorkOrderForDeleteAsync(Id);

    protected override void OnInitialized() {
        _model = new();
        _currentPage = _currentPage = NavigationUtil.GetCurrentPage();
    }

    public void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
        NavigationManager.NavigateTo(_currentPage);
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    private async Task OnClickModalConfirm() {
        try {
            var responseDelete = await OperationManagerService.DeleteWorkOrderAsync(Id);

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