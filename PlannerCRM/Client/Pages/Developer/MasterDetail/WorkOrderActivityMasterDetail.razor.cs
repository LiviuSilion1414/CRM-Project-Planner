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
    private List<List<WorkTimeRecordViewDto>> _workTimeRecords = new();

    protected override async Task OnInitializedAsync() {
        _activities = await DeveloperService.GetActivitiesByEmployeeIdAsync(EmployeeId);
        
        foreach(var ac in _activities) {
            var workorder = await DeveloperService.GetWorkOrderByIdAsync(ac.WorkOrderId);
            _workorders.Add(workorder);
        }

        foreach (var workOrder in _workorders) {
            foreach (var activity in _activities) {
                var workTimes = await DeveloperService.GetWorkTimeRecordsByActivityId(activity.Id);
                
                Console.WriteLine("*******************Start***********************");
                if (workOrder.Id == activity.WorkOrderId) {
                    Console.WriteLine("WorkOrder: {0}", workOrder.Name);
                }

                Console.WriteLine("Activity: {0}", activity.Name);
                Console.WriteLine("{0} is populated? {1}", nameof(workTimes).ToUpper(), workTimes.Count != 0);

                foreach (var item in workTimes) {
                    if (item.ActivityId == activity.Id) {
                        Console.WriteLine("WorkTimeRecord: {0}", item.Hours);
                    }
                }

                _workTimeRecords.Add(workTimes);
                Console.WriteLine("*******************End***********************");
            }
        }
    }

    public void OnClickAddWorkedHours(int activityId) {
        NavManager.NavigateTo($"/developer/add/worked-hours/{EmployeeId}/{activityId}");
    }
}