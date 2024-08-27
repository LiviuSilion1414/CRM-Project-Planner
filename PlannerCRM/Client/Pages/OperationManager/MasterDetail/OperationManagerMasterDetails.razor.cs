namespace PlannerCRM.Client.Pages.OperationManager.MasterDetail;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerMasterDetails : ComponentBase
{
    [Parameter] public int WorkOrderId { get; set; }
    [Parameter] public WorkOrderViewDto WorkOrder { get; set; }
    [Parameter] public bool HasBlockedActions { get; init; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private WorkOrderViewDto _workOrder;
    private List<ActivityViewDto> _activities;

    private bool _isEditActivityClicked;
    private bool _isDeleteActivityClicked;
    private bool _isShowActivityClicked;
    private int _activityId;
    private ActivityViewDto _activityView;

    protected override async Task OnInitializedAsync() {
        _workOrder = await OperationManagerService.GetWorkOrderForViewByIdAsync(WorkOrderId);
        _activities = await OperationManagerService.GetActivityByWorkOrderAsync(_workOrder.Id); 
    }

    protected override void OnInitialized() {
        _workOrder = new();
        _activities = new();
    }

    private void OnClickEdit(int activityId) {
        _isEditActivityClicked = !_isEditActivityClicked;
        _activityId = activityId;
    }

    private void OnClickShowDetails(int activityId) {
        _isShowActivityClicked = !_isShowActivityClicked;
        _activityId = activityId;
    }

    private void OnClickDelete(int activityId) {
        _isDeleteActivityClicked = !_isDeleteActivityClicked;
        _activityId = activityId;
    }
}