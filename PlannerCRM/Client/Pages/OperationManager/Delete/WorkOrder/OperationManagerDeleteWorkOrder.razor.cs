using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.OperationManager.Delete.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerDeleteWorkOrder
{
    [Parameter] public int Id { get; set; }

    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }
    
    private bool _IsCancelClicked { get; set; } = false;
    public string Title { get; set; }    
    public string Message { get; set; } 

    private string _CurrentPage { get; set; }
    private bool _IsError { get; set; }
    private string _Message { get; set; }
    private WorkOrderDeleteDto _Model = new();

    protected override async Task OnInitializedAsync() {
        _Model = await OperationManagerService.GetWorkOrderForDeleteAsync(Id);
    }

    protected override void OnInitialized() {
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    }

    public void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
        NavManager.NavigateTo("/operation-manager");
    }

    private async Task OnClickModalConfirm() {
        try {
            var responseDelete = await OperationManagerService.DeleteWorkOrderAsync(Id);

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