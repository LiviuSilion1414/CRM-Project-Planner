using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.Models;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using Microsoft.AspNetCore.Components.Forms;

namespace PlannerCRM.Client.Pages.OperationManager.Edit.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerEditWorkOrder
{
    [Parameter] public int Id { get; set; }
    
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; } 
    [Inject] private NavigationManager NavManager { get; set; }

    private WorkOrderFormDto _Model = new WorkOrderFormDto();
    private EditContext _EditContext { get; set; }
    private bool _IsError { get; set; }
    private string _Message { get; set; }

    protected override async Task OnInitializedAsync() {
        _Model = await OperationManagerService.GetWorkOrderForEditAsync(Id);
    }

    protected override void OnInitialized() {
        _EditContext = new EditContext(_Model);
    }

    private void RedirectToPage() {
        NavManager.NavigateTo("/operation-manager");
    }

    private async Task OnClickConfirm() {
        var response = await OperationManagerService.EditWorkOrderAsync(_Model);

        if (!response.IsSuccessStatusCode) {
            _IsError = true;
            _Message = await response.Content.ReadAsStringAsync();
        } else {
            RedirectToPage();
        }
    }

    private void OnClickCancel() {
        RedirectToPage();
    }
}