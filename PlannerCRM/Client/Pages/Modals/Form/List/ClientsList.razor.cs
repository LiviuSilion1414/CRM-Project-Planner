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
    private int _collectionSize;

    protected override async Task OnInitializedAsync() {
        _collectionSize = await OperationManagerService.GetCollectionSizeAsync();
        _clients = await OperationManagerService.GetClientsPaginated(0, 5);
        foreach (var client in _clients) {
            var workOrder = await OperationManagerService.GetWorkOrderForViewAsync(client.WorkOrderId);

            _workOrders.Add(workOrder);
        }
    }

    public async Task HandlePaginate(int limit, int offset) =>
        _clients = await OperationManagerService.GetClientsPaginated(limit, offset);


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