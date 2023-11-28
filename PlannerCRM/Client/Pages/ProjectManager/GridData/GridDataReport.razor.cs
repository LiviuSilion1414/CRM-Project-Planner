using Append.Blazor.WebShare;
using PlannerCRM.Client.Services.Utilities.Navigation.Lock;

namespace PlannerCRM.Client.Pages.ProjectManager.GridData;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
public partial class GridDataReport : ComponentBase
{
    [Parameter] public List<WorkOrderViewDto> WorkOrders { get; set;}

    [Inject] IWebShareService WebShareService { get; set; }
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

    protected override void OnInitialized() {
        WorkOrders = new();
        _currentPage = NavigationUtil.GetCurrentPage();
        _orderTitles = new() {
            { "Cliente", OnClickOrderByClient },
            { "Commessa", OnClickOrderByWorkOrder },
            { "Data inizio", OnClickOrderByStartDate },
            { "Data fine", OnClickOrderByFinishDate },
            { "Stato report", OnClickOrderByStatus }
        };
    }

    private void OnClickOrderByStatus() {
        WorkOrders = WorkOrders
            .OrderBy(wo => wo.IsInvoiceCreated)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByFinishDate() {
        WorkOrders = WorkOrders
            .OrderBy(wo => wo.FinishDate)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByStartDate() {
        WorkOrders = WorkOrders
            .OrderBy(wo => wo.StartDate)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByWorkOrder() {
        WorkOrders = WorkOrders
            .OrderBy(wo => wo.Name)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByClient() {
        WorkOrders = WorkOrders
            .OrderBy(wo => wo.ClientName)
            .ToList();

        StateHasChanged();
    }

    private void HandleOrdering(string key) {
        if (_orderTitles.ContainsKey(key)) {
            _orderTitles[key].Invoke();
            _orderKey = key;
        }
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
        //_isViewInvoiceClicked = !_isViewInvoiceClicked;
        _workOrderId = workOrderId;
        _isViewReportInvoiceClicked = !_isViewReportInvoiceClicked;
        NavManager.NavigateTo($"/invoices/{_workOrderId}");
    }
    
    private async Task ShareReportAsync(int invoiceId) {
        await WebShareService.ShareAsync(new ShareData(title: "La tua fattura.", text: "Nuova fattura", url: $"http://localhost:5432/invoices/{invoiceId}"));
    }
    
    private void HandleFeedbackCancel(bool value) =>
        _isError = value;
}