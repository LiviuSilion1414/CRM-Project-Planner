namespace PlannerCRM.Client.Pages.AccountManager.Home;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManager : ComponentBase
{
    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }

    private int _userId;

    private bool _isViewClicked;
    private bool _isAddClicked;
    private bool _isEditClicked;
    private bool _isDeleteClicked;
    private bool _isRestoreClicked;

    private bool _hasMoreUsers = true;

    private List<EmployeeViewDto> _users;
    private int _collectionSize;


    protected override async Task OnInitializedAsync() {
        await FetchDataAsync();
        _collectionSize = await AccountManagerService.GetEmployeesSizeAsync();
    }

    protected override void OnInitialized() => _users = new();
    
    private async Task FetchDataAsync(int limit = 0, int offset = 5) 
        => _users = await AccountManagerService.GetPaginatedEmployeesAsync();

    private async Task HandlePaginate(int limit, int offset) {
        await FetchDataAsync(limit, offset);
        _hasMoreUsers = _users.Any();
    }
    
    private void ShowDetails(int id) {
        _isViewClicked = !_isViewClicked;
        _userId = id;
    }

    private void OnClickAddUser() => _isAddClicked = !_isAddClicked;

    private void OnClickEdit(int id) {
        _isEditClicked = !_isEditClicked;
        _userId = id;
    }

    private void OnClickDelete(int id) {
        _isDeleteClicked = !_isDeleteClicked;
        _userId = id;
    }

    private void OnClickRestore(int id) {
        _isRestoreClicked = !_isRestoreClicked;
        _userId = id;
    }
}