namespace PlannerCRM.Client.Pages.AccountManager.GridData;

public partial class GridDataUsers : ComponentBase
{
    [Parameter] public List<EmployeeViewDto> Users { get; set;  }

    private int _userId;
    
    private bool _isViewClicked;
    private bool _isAddClicked;
    private bool _isEditClicked;
    private bool _isDeleteClicked;
    private bool _isRestoreClicked;

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
}