namespace PlannerCRM.Client.Pages.ProjectManager.Home;

public partial class ProjectManager : ComponentBase 
{
    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private List<WorkOrderCostDto> _workOrders;
    private bool _isCancelClicked;
    private string _currentPage;
    private int _collectionSize;
    
    protected override async Task OnInitializedAsync() {
        _workOrders = await ProjectManagerService.GetPaginatedAsync();
    }

    protected override void OnInitialized() {
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    private void OnClickHideBanner() => _isCancelClicked = !_isCancelClicked;

    
}