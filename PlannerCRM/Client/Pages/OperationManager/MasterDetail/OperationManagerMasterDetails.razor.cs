using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.OperationManager.MasterDetail;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerMasterDetails
{
    [Parameter] public int WorkOrderId { get; set; }

    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }

    private WorkOrderViewDto _WorkOrder = new();
    private List<ActivityEditFormDto> _Activities = new();
    public bool _IsEditActivityClicked { get; set; }
    public bool _IsDeleteActivityClicked { get; set; }
    public int _ActivityId { get; set; }
    public bool _Loaded { get; set; } = false;

    protected override async Task OnInitializedAsync() {
        _WorkOrder = await OperationManagerService.GetWorkOrderForViewAsync(WorkOrderId);
        _Activities = await OperationManagerService.GetActivityPerWorkOrderAsync(_WorkOrder.Id); 
    }
    
    void LoadSpinner() {
        Task.Delay(1500);
        _Loaded=true;
    }

    private void OnClickEdit(int activityId) {
        //NavManager.NavigateTo($"/operation-manager/edit/activity/{_WorkOrder.Id}/{activityId}");
        _IsEditActivityClicked = !_IsEditActivityClicked;
        _ActivityId = activityId;
    }

    private void OnClickDelete(int activityId) {
        //NavManager.NavigateTo($"/operation-manager/delete/activity/{_WorkOrder.Id}/{activityId}");
        _IsDeleteActivityClicked = !_IsDeleteActivityClicked;
        _ActivityId = activityId;
    }
}