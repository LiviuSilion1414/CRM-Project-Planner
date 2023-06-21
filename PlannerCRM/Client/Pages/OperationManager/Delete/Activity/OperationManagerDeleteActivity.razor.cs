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
    [CascadingParameter(Name = "WorkOrderId")] public int WorkOrderId { get; set; }
    [CascadingParameter(Name = "ActivityId")] public int ActivityId { get; set; }

    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }    
    [Inject] private NavigationManager NavManager { get; set; }
    
    private WorkOrderViewDto _CurrentWorkorder = new();
    private ActivityDeleteDto _CurrentActivity = new();
    private bool _IsCancelClicked { get; set; } = false;
    public string Title { get; set; }    
    public string Message { get; set; } 

    private string _CurrentPage { get; set; }
    private bool _IsError { get; set; }
    private string _Message { get; set; }

    protected override async Task OnInitializedAsync() {
        _CurrentWorkorder = await OperationManagerService.GetWorkOrderForViewAsync(WorkOrderId);
        _CurrentActivity = await OperationManagerService.GetActivityForDeleteAsync(ActivityId);
    }

    protected override void OnInitialized() {
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
        _CurrentActivity.Employees = new();
    }

    public void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
        NavManager.NavigateTo("/operation-manager");
    }

    public async Task OnClickModalConfirm() {
        Console.Write("Clicked");
        try
        {
            var responseDelete = await OperationManagerService.DeleteActivityAsync(ActivityId);
    
            if (!responseDelete.IsSuccessStatusCode) {
                _Message = await responseDelete.Content.ReadAsStringAsync();
                _IsError = true;
            } else {
                _IsCancelClicked = !_IsCancelClicked;
                NavManager.NavigateTo("/operation-manager", true);
            }
        } catch (Exception exc) {
            _IsError = true;
            _Message = exc.Message;
        }
    }
}