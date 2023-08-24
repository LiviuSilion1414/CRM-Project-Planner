namespace PlannerCRM.Client.Pages.OperationManager.Home;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManager : ComponentBase
{
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private List<WorkOrderViewDto> _workOrders = new();
    private List<ClientViewDto> _clients = new();
    private WorkOrderViewDto _currentWorkOrder = new();
    
    private bool _trIsClicked;

    private bool _isCreateWorkOrderClicked;
    private bool _isEditWorkOrderClicked;
    private bool _isDeleteWorkOrderClicked;

    private bool _isCreateClientClicked;
    private bool _isEditClientClicked;
    private bool _isDeleteClientClicked;
    
    private bool _isCreateActivityClicked;
    private bool _isShowClientsClicked;

    private bool _hasMoreWorkOrders;

    private int _workOrderId; 
    private int _clientId; 

    private int _collectionSize;

    protected override async Task OnInitializedAsync() {
        _collectionSize = await OperationManagerService.GetCollectionSize();
        _workOrders = await OperationManagerService.GetCollectionPaginated();
        foreach (var wo in _workOrders) {
            var clients = await OperationManagerService.SearchClientAsync(wo.Client.Id);
            foreach (var client in clients) {
                if (!_clients.Contains(client)) {
                    _clients.Add(client);
                }
            }
        }
    }
    
    public async Task HandlePaginate(int limit, int offset) {
        _workOrders = await OperationManagerService.GetCollectionPaginated(limit, offset);
        _hasMoreWorkOrders = _workOrders.Any();
    }

    public async Task OnClickShowClients() {
        _isShowClientsClicked = !_isShowClientsClicked;
        _clients = await OperationManagerService.GetClientsPaginated();
    }

    private void OnClickTableRow(int workorderId) {
        _trIsClicked = !_trIsClicked;
        _currentWorkOrder = _workOrders.Find(wo => wo.Id == workorderId);
    }

    private void OnClickAddWorkOrder() =>
       _isCreateWorkOrderClicked = !_isCreateWorkOrderClicked;

    private void OnClickAddActivity() =>
       _isCreateActivityClicked = !_isCreateActivityClicked;

    private void OnClickEdit(int id) {
        _isEditWorkOrderClicked = !_isEditWorkOrderClicked;
        _workOrderId = id;
    }

    public void OnClickDelete(int id) {
        _isDeleteWorkOrderClicked = !_isDeleteWorkOrderClicked;
        _workOrderId = id;
    }

    private void OnClickAddClient() =>
       _isCreateClientClicked = !_isCreateClientClicked;

    private void OnClickEditClient(int id) {
        _isEditWorkOrderClicked = !_isEditWorkOrderClicked;
        _clientId = id;
    }

    public void OnClickDeleteClient(int id) {
        _isDeleteClientClicked = !_isDeleteClientClicked;
        _clientId = id;
    }
}