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

    private int _workOrderId; 
    private int _clientId; 

    private int _collectionSize;

    protected override async Task OnInitializedAsync() {
        _workOrders = await OperationManagerService.GetCollectionPaginated();
        _collectionSize = await OperationManagerService.GetCollectionSize();
    }
    
    public async Task HandlePaginate(int limit, int offset) =>
        _workOrders = await OperationManagerService.GetCollectionPaginated(limit, offset);

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