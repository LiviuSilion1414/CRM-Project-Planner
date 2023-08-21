namespace PlannerCRM.Client.Pages.OperationManager.Add.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalAddWorkOrder : ComponentBase
{
    [Parameter] public string Title { get; set; }    

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private WorkOrderFormDto _model;
    private string _currentPage;
    
    protected override void OnInitialized() {
        _model = new();
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    public async Task OnClickModalConfirm(WorkOrderFormDto returnedModel) {
        await OperationManagerService.AddWorkOrderAsync(returnedModel);

        NavManager.NavigateTo(_currentPage, true);
    }
}