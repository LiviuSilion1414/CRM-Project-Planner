using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.OperationManager.Add.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerAddWorkOrder
{
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }

    private WorkOrderFormDto _Model = new();
    private EditContext _EditContext;
    private bool _IsError { get; set; }
    private string _Message { get; set; }
    
    protected override void OnInitialized() {
        _EditContext = new(_Model);
    }

    public void RedirectToPage() {
        NavManager.NavigateTo("/operation-manager");
    }

    public async Task OnClickConfirm() {
        var response = await OperationManagerService.AddWorkOrderAsync(_Model);

        if (!response.IsSuccessStatusCode) {
            _IsError = true;
            _Message = await response.Content.ReadAsStringAsync();
        } else {
            RedirectToPage();
        } 
    }

    public void OnClickCancel() {
        RedirectToPage();
    }
}