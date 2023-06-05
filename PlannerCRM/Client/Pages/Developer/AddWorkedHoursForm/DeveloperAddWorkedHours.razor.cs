using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Form;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.Developer.AddWorkedHoursForm;

// [Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
// [Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
public partial class DeveloperAddWorkedHours
{
    [Parameter] public int EmployeeId { get; set; }
    [Parameter] public int ActivityId { get; set; }
    [Parameter] public int WorkorderId { get; set; }
    
    [Inject] private CurrentUserInfoService CurrentUserInfoService  { get; set; }
    [Inject] private DeveloperService DeveloperService { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }
    
    private ActivityViewDto _Model = new();
    private EditContext _EditContext { get; set; }
    private WorkTimeRecordFormDto _WorkTimeRecord = new();
    private WorkOrderViewDto _WorkOrder = new();
    private string _EmployeeRole { get; set; }  
    private int _WorkedHours { get; set; }
    private bool _IsError { get; set; }
    private string _Message { get; set; }

    protected override async Task OnInitializedAsync() {
        _EmployeeRole = await CurrentUserInfoService.GetCurrentUserRoleAsync();
        foreach (var possibleRole in Enum.GetValues(typeof(Roles))) {
            if ((possibleRole.ToString() == nameof(Roles.SENIOR_DEVELOPER) && possibleRole.ToString() == _EmployeeRole) || 
                (possibleRole.ToString() == nameof(Roles.JUNIOR_DEVELOPER) && possibleRole.ToString() == _EmployeeRole)) {
                _EmployeeRole = _EmployeeRole;
            }
        }
            
        _Model = await DeveloperService.GetActivityByIdAsync(ActivityId);
        _WorkOrder = await DeveloperService.GetWorkOrderByIdAsync(_Model.WorkOrderId);
    }
    
    protected override void OnInitialized() {
        _EditContext = new(_Model);
    }

    public void RedirectToPage() {
        foreach (var possibleRole in Enum.GetValues(typeof(Roles))) {
            if (((possibleRole.ToString() == _EmployeeRole) && (possibleRole.ToString() == _EmployeeRole)) && 
                ((possibleRole.ToString() == nameof(Roles.SENIOR_DEVELOPER)))) {
                
                NavManager.NavigateTo($"/senior-developer/{EmployeeId}");
            }
            if (((possibleRole.ToString() == _EmployeeRole) && (possibleRole.ToString() == _EmployeeRole)) && 
                ((possibleRole.ToString() == nameof(Roles.JUNIOR_DEVELOPER)))) {
                
                NavManager.NavigateTo($"/junior-developer/{EmployeeId}");
            }
        }
    }
    
    public void OnClickCancel() {
        RedirectToPage();
    }

    public async Task OnClickAddWorkedHours() {
        _WorkTimeRecord.Date = DateTime.Now;
        _WorkTimeRecord.Hours = _WorkedHours;
        _WorkTimeRecord.ActivityId = _Model.Id;
        _WorkTimeRecord.EmployeeId = EmployeeId;
        _WorkTimeRecord.WorkOrderId = _WorkOrder.Id;
        _WorkTimeRecord.Hours = _WorkedHours;

        var response = await DeveloperService.AddWorkedHoursAsync(_WorkTimeRecord);
        
        if (!response.IsSuccessStatusCode) {
            _IsError = true;
            _Message = await response.Content.ReadAsStringAsync();
        }

        RedirectToPage();
    }
}