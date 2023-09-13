namespace PlannerCRM.Client.Pages.AccountManager.Home;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManager : ComponentBase
{
    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }

    private Dictionary<string, Action> _actions = new();
    private int _userId;

    private bool _isViewClicked;
    private bool _isAddClicked;
    private bool _isEditClicked;
    private bool _isDeleteClicked;
    private bool _isRestoreClicked;

    private List<EmployeeViewDto> _users;
    private List<EmployeeViewDto> _filteredList;
    private IEnumerable<IDtoComparer> _outcomingList;
    private IEnumerable<IDtoComparer> _incomingList;
    private List<string> _usersNames;
    private int _collectionSize;
    private string _input;

    protected override async Task OnInitializedAsync() {
        await FetchDataAsync();
        _collectionSize = await AccountManagerService.GetEmployeesSizeAsync();
    }

    protected override void OnInitialized() {
        _users = new();
        _outcomingList = new List<EmployeeViewDto>();
        _incomingList = new List<EmployeeViewDto>();
        _filteredList = new(_users);
        _usersNames = new();
        _actions = new() {
            { "Tutti",  OnClickAll },
            { "Nome",  OnClickFilterByName },
            { "Dal più recente",  OnClickFilterByLatest },
            { "Dal meno recente",  OnClickFilterByOldest }
        };
        _outcomingList = _users
            .Select(us => 
                new EmployeeViewDto {
                    Name = us.Name,
                    Id = us.Id,
                    FullName = us.FullName,
                    IsDeleted = us.IsDeleted
                }
            )
            .ToList();
    } 

    private void HandleSearchedElements(string query) {
        _filteredList = _users
            .Where(us => us.FullName
                .Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        StateHasChanged();
    } 
    
    private void OnClickAll() {
        _filteredList = new(_users);

        StateHasChanged();
    }

    private void OnClickFilterByName() {
        _filteredList = _users
            .OrderBy(us => us.FullName)
            .ToList();

        StateHasChanged();
    }

    private void OnClickFilterByLatest() {
        _filteredList = _users
            .OrderByDescending(us => us.Id)
            .ToList();

        StateHasChanged();
    }

    private void OnClickFilterByOldest() {
        _filteredList = _users
            .OrderBy(us => us.Id)
            .ToList();

        StateHasChanged();
    }

    private async Task FetchDataAsync(int limit = 0, int offset = 5) {
        _users = await AccountManagerService.GetPaginatedEmployeesAsync(limit, offset);
        _filteredList = new(_users);
    } 

    private async Task HandlePaginate(int limit, int offset) {
        await FetchDataAsync(limit, offset);
    }
    
    private void OnClickAddUser() => 
        _isAddClicked = !_isAddClicked;
}