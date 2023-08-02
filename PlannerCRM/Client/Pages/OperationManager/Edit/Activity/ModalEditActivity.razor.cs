namespace PlannerCRM.Client.Pages.OperationManager.Edit.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalEditActivity : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public int WorkOrderId { get; set; }
    [Parameter] public int ActivityId { get; set; }
    
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }

    private ActivityFormDto _model;
    private WorkOrderViewDto _currentWorkOrder;
    private string _currentPage;

    protected override async Task OnInitializedAsync() {
        _model = await OperationManagerService.GetActivityForEditAsync(ActivityId);
        _currentWorkOrder = await OperationManagerService.GetWorkOrderForViewAsync(WorkOrderId);
        _model.SelectedWorkorder = _currentWorkOrder.Name;
        _model.SelectedEmployee = string.Empty;
    }

    protected override void OnInitialized() {
        _currentWorkOrder = new();
        _model = new() {
            ViewEmployeeActivity = new(),
            EmployeeActivity = new(),
        };

        _currentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
    }

    private async Task OnClickModalConfirm(ActivityFormDto returnedModel) {
        returnedModel.SelectedEmployee = string.Empty;

        await OperationManagerService.EditActivityAsync(returnedModel);

        NavManager.NavigateTo(_currentPage, true);
    }
}