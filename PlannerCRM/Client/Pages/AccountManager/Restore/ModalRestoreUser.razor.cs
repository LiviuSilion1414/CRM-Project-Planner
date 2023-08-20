namespace PlannerCRM.Client.Pages.AccountManager.Restore;

public partial class ModalRestoreUser : ComponentBase 
{
    [Parameter] public int Id { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string Message { get; set; }

    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private EmployeeSelectDto _employee;

    private bool _isCancelClicked;
    private string _currentPage;

    protected override async Task OnInitializedAsync() {
        _employee = await AccountManagerService.GetEmployeeForRestoreAsync(Id);
    }

    protected override void OnInitialized() {
        _employee = new();
        _currentPage = _currentPage = NavigationUtil.GetCurrentPage();
    }

    private void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
        NavigationManager.NavigateTo(_currentPage);
    }

    private async Task OnClickConfirm() {
        await AccountManagerService.RestoreEmployeeAsync(Id);
        NavigationManager.NavigateTo(_currentPage, true);
    }
}