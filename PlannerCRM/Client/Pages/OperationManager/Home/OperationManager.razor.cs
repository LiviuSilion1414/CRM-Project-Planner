namespace PlannerCRM.Client.Pages.OperationManager.Home;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManager : ComponentBase
{
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private List<WorkOrderViewDto> _workOrders;
    private List<WorkOrderViewDto> _filteredList;
    private List<ClientViewDto> _clients;

    private Dictionary<string, Action> _actions;
    
    private bool _isCreateWorkOrderClicked;

    private bool _isCreateClientClicked;
    private bool _isDeleteClientClicked;
    
    private bool _isCreateActivityClicked;
    private bool _isShowClientsClicked;

    private bool _hasMoreWorkOrders;

    private int _clientId; 

    private int _collectionSize;

    protected override void OnInitialized() {
        _workOrders = new();
        _filteredList = new();
        _clients = new();
        _actions = new() {
            { "Tutti", OnClickAll },
            { "Nome", OnClickFilterByName },
            { "Dal più recente", OnClickFilterByLatest },
            { "Dal meno recente", OnClickFilterByOldest },
        };
    }

    protected override async Task OnInitializedAsync() {
        _collectionSize = await OperationManagerService.GetWorkOrdersCollectionSizeAsync();
        _workOrders = await OperationManagerService.GetPaginatedWorkOrdersAsync();

        foreach (var wo in _workOrders) {
            var clients = await OperationManagerService.SearchClientAsync(wo.ClientId);
            foreach (var client in clients) {
                if (!_clients.Any(cl => cl.Id == client.Id)) {
                    _clients.Add(client);
                }
            }
        }

        _filteredList = new(_workOrders);
    }

    private void HandleSearchedElements(string query) {
        _filteredList = _workOrders
            .Where(wo => wo.Name
                .Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        StateHasChanged();
    } 

    private void OnClickAll() {
        _filteredList = new(_workOrders);

        StateHasChanged();
    }

    private void OnClickFilterByName() {
        _filteredList = _workOrders
            .OrderBy(wo => wo.Name)
            .ToList();

        StateHasChanged();
    }

    private void OnClickFilterByLatest() {
        _filteredList = _workOrders
            .OrderByDescending(wo => wo.Id)
            .ToList();
        
        StateHasChanged();
    }

    private void OnClickFilterByOldest() {
        _filteredList = _workOrders
            .OrderBy(wo => wo.Id)
            .ToList();

        StateHasChanged();
    }
    
    public async Task HandlePaginate(int limit, int offset) {
        _workOrders = await OperationManagerService.GetPaginatedWorkOrdersAsync(limit, offset);
        _hasMoreWorkOrders = _workOrders.Any();
    }

    public async Task OnClickShowClients() {
        _isShowClientsClicked = !_isShowClientsClicked;
        _clients = await OperationManagerService.GetClientsPaginatedAsync();
    }

    private void OnClickAddWorkOrder() =>
       _isCreateWorkOrderClicked = !_isCreateWorkOrderClicked;

    private void OnClickAddActivity() =>
       _isCreateActivityClicked = !_isCreateActivityClicked;

    private void OnClickAddClient() =>
       _isCreateClientClicked = !_isCreateClientClicked;

    public void OnClickDeleteClient(int id) {
        _isDeleteClientClicked = !_isDeleteClientClicked;
        _clientId = id;
    }
}