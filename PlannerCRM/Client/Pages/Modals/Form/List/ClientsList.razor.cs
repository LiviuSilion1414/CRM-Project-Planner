namespace PlannerCRM.Client.Pages.Modals.Form.List;

public partial class ClientsList : ComponentBase
{
    [Parameter] public string Title { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private List<ClientViewDto> _clients = new();
    private List<WorkOrderViewDto> _workOrders = new();

    private bool _isCreateClientClicked;
    private bool _isEditClientClicked;
    private bool _isDeleteClientClicked;
    private bool _isShowClientsClicked;

    private bool _isCancelClicked;

    private int _clientId;

    protected override async Task OnInitializedAsync() {
        _clients = await OperationManagerService.GetClientsPaginated(0, 5);
        foreach (var client in _clients) {
            var workOrder = await OperationManagerService.GetWorkOrderForViewAsync(client.Id);

            _workOrders.Add(workOrder);
        }
    } 

    private void OnClickModalCancel() => _isCancelClicked = !_isCancelClicked;

        private void OnClickAddClient() =>
       _isCreateClientClicked = !_isCreateClientClicked;

    private void OnClickEditClient(int id) {
        _isEditClientClicked = !_isEditClientClicked;
        _clientId = id;
    }

    public void OnClickDeleteClient(int id) {
        _isDeleteClientClicked = !_isDeleteClientClicked;
        _clientId = id;
    }
}