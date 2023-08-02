namespace PlannerCRM.Client.Pages.OperationManager.Home;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManager : ComponentBase
{
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private List<WorkOrderViewDto> _workOrders = new();
    private WorkOrderViewDto _currentWorkOrder = new();
    
    private bool _trIsClicked;

    private bool _isCreateWorkOrderClicked;
    private bool _isEditWorkOrderClicked;
    private bool _isDeleteWorkOrderClicked;
    
    private bool _isCreateActivityClicked;

    private int _workOrderId; 

    private int _collectionSize;

    protected override async Task OnInitializedAsync() {
        _workOrders = await OperationManagerService.GetCollectionPaginated();
        _collectionSize = await OperationManagerService.GetCollectionSize();
    }
    
    public async Task HandlePaginate(int limit, int offset) =>
        _workOrders = await OperationManagerService.GetCollectionPaginated(limit, offset);

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
}