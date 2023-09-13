namespace PlannerCRM.Client.Pages.OperationManager.GridData;

public partial class GridDataWorkOrders : ComponentBase
{
    [Parameter] public List<WorkOrderViewDto> WorkOrders { get; set; }
    [Parameter] public List<ClientViewDto> Clients { get; set; }

    private WorkOrderViewDto _currentWorkOrder;

    private bool _trIsClicked;

    private bool _isEditWorkOrderClicked;
    private bool _isDeleteWorkOrderClicked;
    
    private int _workOrderId;

    protected override void OnInitialized()
        => _currentWorkOrder = new();

    private void OnClickEditWorkOrder(int id) {
        _isEditWorkOrderClicked = !_isEditWorkOrderClicked;
        _workOrderId = id;
    }

    public void OnClickDeleteWorkOrder(int id) {
        _isDeleteWorkOrderClicked = !_isDeleteWorkOrderClicked;
        _workOrderId = id;
    }

    private void OnClickTableRow(int workOrderId) {
        _trIsClicked = !_trIsClicked;
        _currentWorkOrder = WorkOrders
            .Single(wo => wo.Id == workOrderId);
    }
}