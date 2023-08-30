namespace PlannerCRM.Client.Pages.Developer.MasterDetail;

[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
public partial class GridData : ComponentBase
{
    [Parameter] public int EmployeeId { get; set; }

    [Inject] public DeveloperService DeveloperService { get; set; }

    private List<WorkTimeRecordViewDto> _workTimeRecords = new();
    private List<WorkOrderViewDto> _workOrders = new();

    
    private List<ActivityViewDto> _activities = new();
    private bool _isAddClicked = false;
    private bool _hasMoreActivities = true;
    private int _activityId;
    private int _collectionSize;

    protected override async Task OnInitializedAsync() {
        _workOrders = new();
        _workTimeRecords = new();

        _collectionSize = await DeveloperService.GetCollectionSizeByEmployeeId(EmployeeId);
        await FetchDataAsync();
    }

    private async Task FetchDataAsync(int limit = 0, int offset= 5) {
        _activities = await DeveloperService.GetActivitiesByEmployeeIdAsync(EmployeeId);
        
        foreach (var ac in _activities) {
            var workorder = await DeveloperService.GetWorkOrderByIdAsync(ac.WorkOrderId);
            
            _workOrders.Add(workorder);
        }

        foreach (var activity in _activities) {
            var workTime = await DeveloperService.GetWorkTimeRecords(activity.WorkOrderId ,activity.Id, EmployeeId);
            if (workTime is not null) {
                _workTimeRecords.Add(workTime);
            }
        }
    }

    public void OnClickAddWorkedHours(int activityId) {
        _isAddClicked = !_isAddClicked;
        _activityId = activityId;
    }

    private async Task HandlePaginate(int limit, int offset) {
        await FetchDataAsync(limit, offset);
        _hasMoreActivities = _activities.Any();
    }
}