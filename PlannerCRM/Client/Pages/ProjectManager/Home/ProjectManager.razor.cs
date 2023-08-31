namespace PlannerCRM.Client.Pages.ProjectManager.Home;

public partial class ProjectManager : ComponentBase 
{
    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    private List<WorkOrderViewDto> _workOrders = new();
    private List<ClientViewDto> _clients = new();
    private bool _isCancelClicked; 
    private bool _isViewInvoiceClicked; 
    private string _currentPage;
    private int _collectionSize;
    private int _workOrderId;

    private bool _isError;
    private string _message;
    
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

    private async Task CreateReport(int workOrderId) {
        var response = await ProjectManagerService.AddInvoiceAsync(workOrderId);
        _isError = !response.IsSuccessStatusCode;
        _message = await response.Content.ReadAsStringAsync();

        if(!_isError) {
            NavigationManager.NavigateTo(_currentPage);//, forceload: true
        }
    }

    private void ViewReport(int workOrderId) {
        _isViewInvoiceClicked = !_isViewInvoiceClicked;
        _workOrderId = workOrderId;
    }

    private async Task HandlePaginate(int limit, int offset) {
       await FetchDataAsync(limit, offset);
    }
    
    private void HandleFeedbackCancel(bool value) {
        _isError = value;
    }        
}