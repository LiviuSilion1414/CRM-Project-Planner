using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.ProjectManager.Home;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
public partial class ProjectManager : ComponentBase
{
    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public ILogger<ProjectManager> Logger { get; set; }

    private List<WorkOrderCostDto> _workOrdersCosts;
    private List<WorkOrderViewDto> _completedWorkOrders;
    private List<WorkOrderCostDto> _filteredList;

    private Dictionary<string, Action> _filters => new() { 
        { "Tutti", OnClickAll },
        { "Creati", OnClickSortCreated },
        { "Inesistenti", OnClickSortNotCreated }
    };

    private int _collectionSize;

    protected override void OnInitialized() {
        _filteredList = new();
        _workOrdersCosts = new();
        _completedWorkOrders = new();
    }

    protected override async Task OnInitializedAsync()
        => await FetchDataAsync();

    private async Task FetchDataAsync(int limit = 5, int offset = 0) {
        _collectionSize = await ProjectManagerService.GetWorkOrderCostsCollectionSizeAsync();
        _workOrdersCosts = await ProjectManagerService.GetWorkOrdersCostsPaginatedAsync(limit, offset);
        _completedWorkOrders = await ProjectManagerService.GetCompletedWorkOrdersAsync(limit, offset);
        _filteredList = new(_workOrdersCosts);
    }

    private void OnClickAll() {
        _filteredList = new(_workOrdersCosts);

        StateHasChanged();
    }

    private void HandleSearchedElements(string query) {
        _filteredList = _workOrdersCosts
            .Where(wo => wo.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        StateHasChanged();
    }

    private void OnClickSortCreated() {
        try {
            _filteredList = _workOrdersCosts
                .Where(wo => wo.IsInvoiceCreated)
                .ToList();

            StateHasChanged();

        } catch (Exception exc) {
            Logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);
        }
    }

    private void OnClickSortNotCreated() {
        try {
            _filteredList = _workOrdersCosts
                .Where(wo => !wo.IsInvoiceCreated)
                .ToList();

            StateHasChanged();
        } catch (Exception exc) {
            Logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);
        }
    }

    private async Task HandlePaginate(int limit, int offset) =>
       await FetchDataAsync(limit, offset);
}