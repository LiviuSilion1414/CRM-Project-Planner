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
    

    private bool _IsError { get; set; }
    private string _Message { get; set; }

    private WorkOrderDeleteDto _Model = new();

    protected override async Task OnInitializedAsync() {
        _Model = await OperationManagerService.GetWorkOrderForDeleteAsync(Id);
    }

    private void RedirectToPage() {
        NavManager.NavigateTo("/operation-manager");
    }

    private void OnClickCancel() {
        RedirectToPage();
    }

    private async Task OnClickDeleteUser() {
        var responseDelete = await OperationManagerService.DeleteWorkOrderAsync(Id);

        if (!responseDelete.IsSuccessStatusCode) {
            _Message = await responseDelete.Content.ReadAsStringAsync();
            _IsError = true;
        } else {
            RedirectToPage();
        }
    }
}