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
    [Parameter] public int Id { get; set; }

    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }

    private WorkOrderViewDto _WorkOrder = new();
    private List<ActivityEditFormDto> _Activities = new();

    protected override async Task OnInitializedAsync() {
        _WorkOrder = await OperationManagerService.GetWorkOrderForViewAsync(Id);
        _Activities = await OperationManagerService.GetActivityPerWorkOrderAsync(_WorkOrder.Id); 
    }

    private void OnClickEdit(int activityId) {
        NavManager.NavigateTo($"/operation-manager/edit/activity/{_WorkOrder.Id}/{activityId}");
    }

    private void OnClickDelete(int activityId) {
        NavManager.NavigateTo($"/operation-manager/delete/activity/{_WorkOrder.Id}/{activityId}");
    }
}