using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Views;
using PlannerCRM.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using PlannerCRM.Client.Services.Crud;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;

namespace PlannerCRM.Client.Pages.Developer.MasterDetail;

[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
public partial class WorkOrderActivityMasterDetail
{
    [Parameter] public int EmployeeId { get; set; }

    [Inject] private DeveloperService DeveloperService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }

    private List<WorkOrderViewDto> _workorders = new();
    private List<ActivityViewDto> _activities = new();
    private List<WorkTimeRecordViewDto> _workTimeRecords = new();
    private bool _IsAddClicked { get; set; } = false;
    private int _ActivityId { get; set; }
    private int _WorkOrderId { get; set; }

    protected override async Task OnInitializedAsync() {
        _activities = await DeveloperService.GetActivitiesByEmployeeIdAsync(EmployeeId);
        
        foreach(var ac in _activities) {
            var workorder = await DeveloperService.GetWorkOrderByIdAsync(ac.WorkOrderId);
            _workorders.Add(workorder);
        }

        foreach (var activity in _activities) {
            var workTime = await DeveloperService.GetWorkTimeRecords(activity.Id, EmployeeId);
            _workTimeRecords.Add(workTime);
        }
    }

    public void OnClickAddWorkedHours(int activityId) {
        _IsAddClicked = !_IsAddClicked;
        _ActivityId = activityId;
    }
}