namespace PlannerCRM.Client.Pages.ProjectManager.Details;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
public partial class GridDataReport : ComponentBase
{
    [Parameter] public List<WorkOrderViewDto> WorkOrders { get; set;}
    [Parameter] public List<ClientViewDto> Clients { get; set;}

    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private int _workOrderId;
    private bool _isViewInvoiceClicked;
    private bool _isError;
    private string _message;
    private string _currentPage;


    protected override void OnInitialized() {
        WorkOrders = new();
        Clients = new();
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    private async Task CreateReport(int workOrderId) {
        var response = await ProjectManagerService.AddInvoiceAsync(workOrderId);
        _isError = !response.IsSuccessStatusCode;

        if(!_isError) {
            NavManager.NavigateTo(_currentPage, true);
        } else {
            _isError = true;
            _message = await response.Content.ReadAsStringAsync();
        }
    }

    private void ViewReport(int workOrderId) {
        _isViewInvoiceClicked = !_isViewInvoiceClicked;
        _workOrderId = workOrderId;
    }
    
    private void HandleFeedbackCancel(bool value) =>
        _isError = value;
}