using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Views;
using PlannerCRM.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using PlannerCRM.Client.Services.Crud;
using Microsoft.AspNetCore.Components;

namespace PlannerCRM.Client.Pages.Developer.MasterDetail;

[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
public partial class WorkOrderActivityMasterDetail
{
    [Parameter] public int EmployeeId { get; set; }

    [Inject] private DeveloperService DeveloperService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }

    private List<WorkOrderViewDto> _workorders = new();
    private List<ActivityEditFormDto> _activities = new();
    private List<WorkTimeRecordViewDto> _workTimeRecords = new();

    protected override async Task OnInitializedAsync() {
        _activities = await DeveloperService.GetActivitiesByEmployeeIdAsync(EmployeeId);
        var listSizeByEmployeeId = await DeveloperService.GetWorkTimesSizeByEmployeeIdAsync(EmployeeId);
    
        if (listSizeByEmployeeId != 0) {
            _workTimeRecords = await DeveloperService.GetAllWorkTimeRecordsByEmployeeId(EmployeeId);
        }

        foreach(var ac in _activities) {
            var workorder = await DeveloperService.GetWorkOrderByIdAsync(ac.WorkOrderId ?? 0);
            _workorders.Add(workorder);
        }
    }

    public void OnClickAddWorkedHours(int activityId) {
        NavManager.NavigateTo($"/developer/add/worked-hours/{EmployeeId}/{activityId}");
    }
}