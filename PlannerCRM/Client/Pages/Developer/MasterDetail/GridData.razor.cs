namespace PlannerCRM.Client.Pages.Developer.MasterDetail;

[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
public partial class GridData : ComponentBase
{
    [Parameter] public int EmployeeId { get; set; }

    [Inject] public DeveloperService DeveloperService { get; set; }

    private List<WorkTimeRecordViewDto> _workTimeRecords;
    private List<WorkOrderViewDto> _workOrders;

    private List<ActivityViewDto> _activities;
    private bool _isAddClicked;
    private bool _hasMoreActivities;
    private int _activityId;
    private int _collectionSize;

    protected override async Task OnInitializedAsync() {
        _collectionSize = await DeveloperService.GetCollectionSizeByEmployeeIdAsync(EmployeeId);

        await FetchDataAsync();
    }

    protected override void OnInitialized() {
        _workTimeRecords = new();
        _workOrders = new();
        _activities = new();
    }

    private async Task FetchDataAsync(int limit = 0, int offset= 5) {
        _activities = await DeveloperService.GetActivitiesByEmployeeIdAsync(EmployeeId, limit, offset);
        
        foreach (var ac in _activities) {
            _workOrders.Add(await DeveloperService.GetWorkOrderByIdAsync(ac.WorkOrderId));
            _workTimeRecords.Add(await DeveloperService.GetWorkTimeRecordsAsync(ac.WorkOrderId ,ac.Id, EmployeeId));
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