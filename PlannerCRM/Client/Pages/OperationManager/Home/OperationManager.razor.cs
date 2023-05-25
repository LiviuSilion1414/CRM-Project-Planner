using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.Models;
using PlannerCRM.Shared.DTOs.Workorder.Views;

namespace PlannerCRM.Client.Pages.OperationManager.Home;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManager
{
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }

    private List<WorkOrderViewDto> _WorkOrders = new();
    private bool _TrIsClicked = false;

    protected override async Task OnInitializedAsync() {
        _WorkOrders = await OperationManagerService.GetAllWorkOrdersAsync();
    }
    
    private void OnClickTableRow(int workorderId) {
        if (_TrIsClicked) {
            _TrIsClicked = false;
        } else {
            _TrIsClicked = true;
        }
    }

    private void OnClickAddWorkOrder() {
        NavManager.NavigateTo("/operation-manager/add/workorder");
    }

    private void OnClickAddActivity() {
        NavManager.NavigateTo("/operation-manager/add/activity");
    }

    private void OnClickEdit(int id) {
        NavManager.NavigateTo($"/operation-manager/edit/workorder/{id}");
    }

    private void OnClickDelete(int id) {
        NavManager.NavigateTo($"/operation-manager/delete/workorder/{id}");
    }
}