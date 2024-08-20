using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Details;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalActivityDetails : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public int ActivityId { get; set; }
    [Parameter] public string WorkOrderName { get; set; }

    private ActivityViewDto ActivityDto { get; set; } = new() { EmployeeActivity = [] };

    [Inject] public OperationManagerCrudService OperationManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private readonly bool _isDisabled = true;

    private bool _isCancelClicked;

    protected override async Task OnInitializedAsync() 
    {
        ActivityDto = await OperationManager.GetActivityForViewByIdAsync(ActivityId);
    }

    private void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
    }

}