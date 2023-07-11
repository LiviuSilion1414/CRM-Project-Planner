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

    private string _CurrentPage { get; set; }
    private bool _IsCancelClicked { get; set; } = false;
    public string Title { get; set; }    
    public string Message { get; set; } 
    
    protected override void OnInitialized() {
        _EditContext = new(_Model);
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    }

    public void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
        NavManager.NavigateTo(_CurrentPage);
    }
    
    public async Task OnClickModalConfirm() {
        try {
            if (_EditContext.Validate()) {
                var response = await OperationManagerService.AddWorkOrderAsync(_Model);

                if (!response.IsSuccessStatusCode) {
                    _Message = await response.Content.ReadAsStringAsync();
                    _IsError = true;
                } else {
                    _IsCancelClicked = !_IsCancelClicked;
                    NavManager.NavigateTo("/operation-manager", true);
                }
            } else {
                _IsCancelClicked = !_IsCancelClicked;
                NavManager.NavigateTo(_CurrentPage, true);
            }    
        } catch (Exception ex) {
            _IsError = true;
            _Message = ex.Message;
        }
    }
}