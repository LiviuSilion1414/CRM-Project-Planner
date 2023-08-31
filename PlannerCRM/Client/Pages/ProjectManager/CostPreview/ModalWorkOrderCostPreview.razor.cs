namespace PlannerCRM.Client.Pages.ProjectManager.CostPreview;

public partial class ModalWorkOrderCostPreview : ComponentBase
{
    [Parameter] public int WorkOrderId { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }

    private WorkOrderCostDto _invoice = new() { 
        Activities = new(),
        Employees = new(), 
        MonthlyActivityCosts = new() 
    };

    private ClientViewDto _client = new();
    private bool _isCancelClicked = false;
    private bool _isInvoiceClicked = false; 
    private string _currentPage;

    protected override async Task OnInitializedAsync() {
        _invoice = await ProjectManagerService.GetInvoiceAsync(WorkOrderId);
        _client = await OperationManagerService.GetClientForViewByIdAsync(_invoice.ClientId);
    }

    protected override void OnInitialized()
        => _currentPage = NavigationUtil.GetCurrentPage();

    private void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage);
    }

    private void OnClickIssueInvoice() {
        _isInvoiceClicked = !_isInvoiceClicked;
    }
}