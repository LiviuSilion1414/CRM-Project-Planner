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

    private int _collectionSize;

    protected override void OnInitialized() {
        _workOrders = new();
        _filteredList = new();
        _clients = new();
        _actions = new() {
            { "Tutte", GetAll },
            { "Attive", GetActive },
            { "Archiviate", GetArchived },
            { "Completate", GetCompleted },
            { "Eliminate", GetDeleted },
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

    private void GetAll() {
        _filteredList = new(_workOrders);

        StateHasChanged();
    }

    private void GetArchived() {
        _filteredList = _workOrders
            .Where(wo => wo.IsArchived)
            .ToList();

        StateHasChanged();
    }

    private void GetActive() {
        _filteredList = _workOrders
            .Where(wo => !wo.IsDeleted || !wo.IsCompleted || !wo.IsArchived)
            .ToList();
        
        StateHasChanged();
    }

    private void GetCompleted() {
        _filteredList = _workOrders
            .Where(wo => wo.IsCompleted)
            .ToList();

        StateHasChanged();
    }

    private void GetDeleted() {
        _filteredList = _workOrders
            .Where(wo => wo.IsDeleted)
            .ToList();

        StateHasChanged();
    }

    public async Task HandlePaginate(int limit, int offset) {
        _workOrders = await OperationManagerService.GetPaginatedWorkOrdersAsync(limit, offset);
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
    }
}