using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.OperationManager.Delete.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerDeleteActivity
{
    [Parameter] public int WorkOrderId { get; set; }
    [Parameter] public int ActivityId { get; set; }

    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }    
    [Inject] private NavigationManager NavManager { get; set; }
    
    private WorkOrderViewDto _CurrentWorkorder = new();
    private ActivityDeleteDto _CurrentActivity = new();
    private bool _IsError { get; set; }
    private string _Message { get; set; }

    protected override async Task OnInitializedAsync() {
        _CurrentWorkorder = await OperationManagerService.GetWorkOrderForViewAsync(WorkOrderId);
        _CurrentActivity = await OperationManagerService.GetActivityForDeleteAsync(ActivityId);
    }

    public void RedirectToPage() {
        NavManager.NavigateTo("/operation-manager");
    }


    public void OnClickCancel() {
        RedirectToPage();
    }

    public async Task OnClickDeleteActivity() {
        var responseDelete = await OperationManagerService.DeleteActivityAsync(ActivityId);

        if (!responseDelete.IsSuccessStatusCode) {
            _IsError = true;
            _Message = await responseDelete.Content.ReadAsStringAsync();
        } else {
            RedirectToPage();
        }
    }
}