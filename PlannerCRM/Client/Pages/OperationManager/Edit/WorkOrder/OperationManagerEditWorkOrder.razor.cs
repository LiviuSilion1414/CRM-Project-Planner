using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.Models;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using Microsoft.AspNetCore.Components.Forms;
using PlannerCRM.Client.Pages.ValidatorComponent;

namespace PlannerCRM.Client.Pages.OperationManager.Edit.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerEditWorkOrder
{
    [Parameter] public int Id { get; set; }
    
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] private NavigationLockService NavLockService { get; set; } 
    [Inject] private NavigationManager NavManager { get; set; }

    private CustomDataAnnotationsValidator _CustomValidator { get; set; }
    private Dictionary<string, List<string>> Errors;

    private WorkOrderFormDto _Model = new();
    private EditContext _EditContext { get; set; }
    private bool _IsError { get; set; }
    private string _Message { get; set; }
    private string _CurrentPage { get; set; }
    private bool _IsCancelClicked { get; set; }

    protected override async Task OnInitializedAsync() {
        _Model = await OperationManagerService.GetWorkOrderForEditAsync(Id);
    }

    protected override void OnInitialized() {
        _EditContext = new(_Model);
        _CurrentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    }

    public void OnClickModalCancel() {
        _IsCancelClicked = !_IsCancelClicked;
        NavManager.NavigateTo(_CurrentPage);
    }

    private async Task OnClickModalConfirm() {
        try {
            var isValid = ValidatorService.ValidateModel(_Model, out Errors);

            if (isValid) {
                var response = await OperationManagerService.EditWorkOrderAsync(_Model);

                if (!response.IsSuccessStatusCode) {
                    _Message = await response.Content.ReadAsStringAsync();
                    _IsError = true;
                } else {
                    _IsCancelClicked = !_IsCancelClicked;
                    NavManager.NavigateTo(_CurrentPage, true);
                }
            } else {
                _CustomValidator.DisplayErrors(Errors);
            }
        } catch (Exception exc) {
            _IsError = true;
            _Message = exc.Message;
        }
    }  
}