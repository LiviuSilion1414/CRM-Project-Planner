namespace PlannerCRM.Client.Pages.OperationManager.Details;

public partial class ModalActivityDetails : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public ActivityViewDto Activity { get; set; }
    [Parameter] public string WorkOrderName { get; set; }

    [Inject] public NavigationManager NavManager { get; set; }

    private readonly bool _isDisabled = true;

    private string _currentPage;
    private bool _isCancelClicked;

    protected override void OnInitialized() => _currentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");

    private void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage);        
    }

}