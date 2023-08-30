namespace PlannerCRM.Client.Pages.ProjectManager.CostPreview;

public partial class ModalWorkOrderCostPreview : ComponentBase
{
    [Parameter] public int WorkOrderId { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    private WorkOrderCostDto _invoice = new();
    private WorkOrderViewDto _workOrder = new();
    private ClientViewDto _client = new();
    private bool _isCancelClicked = false;
    private bool _isInvoiceClicked = false; 
    private string _currentPage;
    private int _collectionSize;

    private readonly bool _isDisabled = true;

    protected override async Task OnInitializedAsync() {
        await ProjectManagerService.AddInvoiceAsync(WorkOrderId);
        
        _invoice = await ProjectManagerService.GetInvoiceAsync(WorkOrderId);
        _client = await OperationManagerService.GetClientForViewAsync(_invoice.ClientId);
        _workOrder = await OperationManagerService.GetWorkOrderForViewAsync(WorkOrderId);
    }

    protected override void OnInitialized()
        => _currentPage = NavigationUtil.GetCurrentPage();

    private void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
        NavigationManager.NavigateTo(_currentPage);
    }

    private void OnClickIssueInvoice() {
        _isInvoiceClicked = !_isInvoiceClicked;
    }
}