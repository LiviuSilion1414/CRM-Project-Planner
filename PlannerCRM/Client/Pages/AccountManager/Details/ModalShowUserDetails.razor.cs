namespace PlannerCRM.Client.Pages.AccountManager.Details;

public partial class ModalShowUserDetails : ComponentBase
{
    [Parameter] public int Id { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }
    
    private readonly bool _isDisabled = true;
    
    private EmployeeViewDto _model;
    private bool _isCancelClicked;

    private string _currentPage;
    private string _input;

    protected override async Task OnInitializedAsync() =>
        _model = await AccountManagerService.GetEmployeeForViewAsync(Id);
    
    protected override void OnInitialized() {
        _model = new();
        _currentPage = NavigationManager.Uri.Replace(NavigationManager.BaseUri, "/");
    }

    private void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
        NavigationManager.NavigateTo(_currentPage);
    }

    private void SwitchPassword(string type) => _input = type;
}