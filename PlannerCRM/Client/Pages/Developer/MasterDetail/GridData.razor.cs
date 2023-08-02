namespace PlannerCRM.Client.Pages.Developer.MasterDetail;

[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
public partial class GridData : ComponentBase
{
    [Parameter] public int EmployeeId { get; set; }

    [Inject] public DeveloperService DeveloperService { get; set; }

    private List<WorkTimeRecordViewDto> WorkTimeRecords { get; set;}
    private List<WorkOrderViewDto> WorkOrders { get; set;}

    
    private List<ActivityViewDto> _activities;
    private bool _isAddClicked = false;
    private int _activityId;

    protected override async Task OnInitializedAsync() {
        _activities = await DeveloperService.GetActivitiesByEmployeeIdAsync(EmployeeId);
        
        foreach(var ac in _activities) {
            var workorder = await DeveloperService.GetWorkOrderByIdAsync(ac.WorkOrderId);
            WorkOrders.Add(workorder);
        }

        foreach (var activity in _activities) {
            var workTime = await DeveloperService.GetWorkTimeRecords(activity.WorkOrderId ,activity.Id, EmployeeId);
            if (workTime is not null) {
                WorkTimeRecords.Add(workTime);
            }
        }
    }

    public void OnClickAddWorkedHours(int activityId) {
        _isAddClicked = !_isAddClicked;
        _activityId = activityId;
    }
}