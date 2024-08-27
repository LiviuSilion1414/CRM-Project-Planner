namespace PlannerCRM.Client.Pages.OperationManager.MasterDetail;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerMasterDetails : ComponentBase
{
    [Parameter] public WorkOrderViewDto WorkOrder { get; set; }
    [Parameter] public bool HasBlockedActions { get; init; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private readonly ActivityViewDto _activityView = new();
    private List<ActivityViewDto> _activities = [];

    private bool _isEditActivityClicked;
    private bool _isDeleteActivityClicked;
    private bool _isShowActivityClicked;
    private int _activityId;

    protected override async Task OnInitializedAsync() {
        _activities = await OperationManagerService.GetActivityByWorkOrderAsync(WorkOrder.Id); 
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