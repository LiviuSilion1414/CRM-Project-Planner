namespace PlannerCRM.Client.Pages.AccountManager.Home;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManager : ComponentBase
{
    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }

    private int _userId;

    private string _title;
    private string _message;
    private string _confirmationMessage;

    private bool _isViewClicked;
    private bool _isAddClicked;
    private bool _isEditClicked;
    private bool _isDeleteClicked;
    private bool _isRestoreClicked;

    private List<EmployeeViewDto> _users;
    private int _collectionSize;


    protected override async Task OnInitializedAsync() {
        _users = await AccountManagerService.GetPaginatedEmployees();
        _collectionSize = await AccountManagerService.GetEmployeesSize();
    }

    protected override void OnInitialized() => _users = new();

    private async Task HandlePaginate(int limit, int offset) =>
        _users = await AccountManagerService.GetPaginatedEmployees(limit, offset);
    
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
        _confirmationMessage = Titles.CONFIRM_DELETE_USER;
        _title = Titles.DELETE_USER;
    }

    private void OnClickRestore(int id) {
        _isRestoreClicked = !_isRestoreClicked;
        _userId = id;
        _confirmationMessage = Titles.CONFIRM_RESTORE_USER;
        _title = Titles.RESTORE_USER;
    }
}