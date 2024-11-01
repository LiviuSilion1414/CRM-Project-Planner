using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.ProjectManager.GridData;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
public partial class GridDataReport : ComponentBase
{
    [Parameter] public List<WorkOrderCostDto> WorkOrdersCosts { get; set; }

    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private Dictionary<string, Action> _orderTitles;

    private int _workOrderId;
    private bool _isViewInvoiceClicked;
    private bool _isError;
    private string _message;
    private string _currentPage;
    private string _orderKey;
    private bool _isViewReportInvoiceClicked;

    protected override void OnInitialized()
    {
        WorkOrdersCosts = new();
        _currentPage = NavigationUtil.GetCurrentPage();
        _orderTitles = new() {
            { "Cliente", OnClickOrderByClient },
            { "Commessa", OnClickOrderByWorkOrder },
            { "Data inizio", OnClickOrderByStartDate },
            { "Data fine", OnClickOrderByFinishDate },
            { "Stato report", OnClickOrderByStatus }
        };
    }

    private void OnClickOrderByStatus()
    {
        WorkOrdersCosts = WorkOrdersCosts
            .OrderBy(wo => wo.IsInvoiceCreated)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByFinishDate()
    {
        WorkOrdersCosts = WorkOrdersCosts
            .OrderBy(wo => wo.FinishDate)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByStartDate()
    {
        WorkOrdersCosts = WorkOrdersCosts
            .OrderBy(wo => wo.StartDate)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByWorkOrder()
    {
        WorkOrdersCosts = WorkOrdersCosts
            .OrderBy(wo => wo.Name)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByClient()
    {
        WorkOrdersCosts = WorkOrdersCosts
            .OrderBy(wo => wo.Client.Name)
            .ToList();

        StateHasChanged();
    }

    private void HandleOrdering(string key)
    {
        if (_orderTitles.ContainsKey(key))
        {
            _orderTitles[key].Invoke();
            _orderKey = key;
        }
    }

    private async Task CreateInvoice(int workOrderId)
    {
        var response = await ProjectManagerService.AddInvoiceAsync(workOrderId);
        _isError = !response.IsSuccessStatusCode;

        if (!_isError)
        {
            NavManager.NavigateTo(_currentPage, true);
        } else
        {
            _isError = true;
            _message = await response.Content.ReadAsStringAsync();
        }
    }

    private void ViewInvoice(int workOrderId)
    {
        _isViewInvoiceClicked = !_isViewInvoiceClicked;
        _workOrderId = workOrderId;
        _isViewReportInvoiceClicked = !_isViewReportInvoiceClicked;
        NavManager.NavigateTo($"/invoices/{_workOrderId}");
    }

    private async Task DeleteInvoice(int workOrderId)
    {
        await ProjectManagerService.DeleteInvoiceAsync(workOrderId);
    }

    private void HandleFeedbackCancel(bool value) =>
        _isError = value;
}