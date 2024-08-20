using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Delete.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalDeleteWorkOrder : ComponentBase
{
    [Parameter] public int Id { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    
    private WorkOrderDeleteDto _model = new() { Client = new() };
    
    private string _message;
    private bool _isCancelClicked;
    private bool _isError;

    protected override async Task OnInitializedAsync() =>
        _model = await OperationManagerService.GetWorkOrderForDeleteByIdAsync(Id);

    public void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
    }

    private void OnClickHideBanner(bool hidden) 
        => _isError = hidden;

    private async Task OnClickModalConfirm() {
        try {
            var responseDelete = await OperationManagerService.DeleteWorkOrderAsync(Id);

            if (!responseDelete.IsSuccessStatusCode) {
                _message = await responseDelete.Content.ReadAsStringAsync();
                _isError = true;
            } else {
                _isCancelClicked = !_isCancelClicked;
            }
        } catch (Exception exc) {
            _isError = true;
            _message = exc.Message;
        }
    }
}