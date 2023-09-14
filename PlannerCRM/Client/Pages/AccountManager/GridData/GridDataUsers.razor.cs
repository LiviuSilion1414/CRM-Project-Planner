namespace PlannerCRM.Client.Pages.AccountManager.GridData;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class GridDataUsers : ComponentBase
{
    [Parameter] public List<EmployeeViewDto> Users { get; set;  }
    private Dictionary<string, Action> _orderTitles;
    
    private int _userId;
    private string _orderKey;

    private bool _isViewClicked;
    private bool _isAddClicked;
    private bool _isEditClicked;
    private bool _isDeleteClicked;
    private bool _isRestoreClicked;

    protected override void OnInitialized() {
        _orderTitles = new() {
            { " Stato ", OnClickOrderIfActive },
            { " Email ", OnClickOrderByEmail },
            { " Nome ", OnClickOrderByName },
            { " Data di nascita ", OnClickOrderByBirthDay },
            { " Data d'inizio ", OnClickOrderByStartDate },
            { " Ruolo ", OnClickOrderByRole },
            { " Tariffa oraria ", OnClickOrderByHourlyRate }
        };
    }

    private void HandleOrdering(string key) {
        if (_orderTitles.ContainsKey(key)) {
            _orderTitles[key].Invoke();
            _orderKey = key;
        }
    }

    private void ShowDetails(int id) {
        _isViewClicked = !_isViewClicked;
        _userId = id;
    }

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

    private void OnClickOrderIfActive() {
        Users = Users
            .OrderBy(us => !us.IsArchived)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByEmail() {
        Users = Users
            .OrderBy(us => us.Email)
            .ToList();

        StateHasChanged();
    }
    
    private void OnClickOrderByName() {
        Users = Users
            .OrderBy(us => us.FullName)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByBirthDay() {
        Users = Users
            .OrderBy(us => us.BirthDay)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByRole() {
        Users = Users
            .OrderBy(us => us.Role)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByHourlyRate() {
        Users = Users
            .OrderBy(us => us.CurrentHourlyRate)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByStartDate() {
        Users = Users
            .OrderBy(us => us.StartDate)
            .ToList();

        StateHasChanged();
    }
}