namespace PlannerCRM.Client.Pages.ProjectManager.Home;

public partial class ProjectManager : ComponentBase 
{
    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private List<WorkOrderViewDto> _workOrders = new();
    private List<ClientViewDto> _clients = new();
    private bool _isCancelClicked; 
    private bool _isInvoiceClicked; 
    private string _currentPage;
    private int _collectionSize;
    private int _workOrderId;
    
    protected override async Task OnInitializedAsync() {
       await FetchDataAsync();
    }

    private async Task FetchDataAsync(int limit = 0, int offset = 5) {
        _workOrders = await ProjectManagerService.GetPaginatedAsync(limit, offset);

        foreach (var wo in _workOrders) {
            var client = await ProjectManagerService.GetClientForViewAsync(wo.ClientId);
            _clients.Add(client);
        }
    }

    protected override void OnInitialized() {
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    private void ViewReport(int workOrderId) {
        _isInvoiceClicked = !_isInvoiceClicked;
        _workOrderId = workOrderId;
    }

    private async Task HandlePaginate(int limit, int offset) {
       await FetchDataAsync(limit, offset);
    }        
}