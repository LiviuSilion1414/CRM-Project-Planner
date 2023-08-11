namespace PlannerCRM.Client.Pages.Modals.Form.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalFormWorkOrder : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public WorkOrderFormDto Model { get; set; }
    [Parameter] public EventCallback<WorkOrderFormDto> GetValidatedModel { get; set; }

    [Inject] public NavigationLockService NavLockService { get; set; }
    [Inject] public CustomDataAnnotationsValidator CustomValidator { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public Logger<WorkOrderFormDto> Logger { get; set; }
    
    private Dictionary<string, List<string>> _errors;
    private EditContext _editContext;

    private string _currentPage;
    private string _errorMessage;
    private bool _isError;
    private bool _isCancelClicked;

    protected override void OnInitialized() {
        Model = new();
        _editContext = new(Model);
        CustomValidator = new();
        _isCancelClicked = false;
        _currentPage = NavigationManager.Uri.Replace(NavigationManager.BaseUri, "/");
    }

    private void OnClickModalCancel() {
        _isCancelClicked = !_isCancelClicked;
        NavigationManager.NavigateTo(_currentPage);
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    private void OnClickInvalidSubmit() {
        _isError = true;
        _errorMessage = ExceptionsMessages.EMPTY_FIELDS;
    }

    private async Task OnClickModalConfirm() {
        try {
            var isValid = ValidatorService.Validate(Model, out _errors);
            if (isValid) {
                await GetValidatedModel.InvokeAsync(Model);
            } else {
                CustomValidator.DisplayErrors(_errors);
                OnClickInvalidSubmit();
            }
        } catch (NullReferenceException exc) {
            Logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);
            _errorMessage = exc.Message;
            _isError = true;
        } catch (Exception exc) {
            Logger.LogError("Error: { } _message: { }", exc.StackTrace, exc.Message);
            _errorMessage = exc.Message;
            _isError = true;
        }
    }
}
