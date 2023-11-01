
using PlannerCRM.Client.Services.Utilities.Navigation.Lock;

namespace PlannerCRM.Client.Components.ProfileMenu.ProfileSettings;

public partial class ProfileInfoSettings : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public CustomDataAnnotationsValidator CustomValidator { get; set; }

    private Dictionary<string, List<string>> _errors;
    private EmployeeFormDto _model;
    private EditContext _editContext;

    private bool _isModifyModeEnabled = false;
    private bool _operationDone = false;
    private string _editButtonText = ConstantValues.MODIFY;

    protected override async Task OnInitializedAsync()
    {
        _model = new();
        _editContext = new(_model);
        _model = await AccountManagerService.GetEmployeeForEditByIdAsync(2);
    }

    private string AddEditCssClass() {
        return !_isModifyModeEnabled
            ? CssClass.Span
            : CssClass.FormControl;
    }
        
    private void Modify() {
        _isModifyModeEnabled = !_isModifyModeEnabled;
        _editButtonText = _isModifyModeEnabled switch
        {
            true => ConstantValues.CANCEL,
            false => ConstantValues.MODIFY
        };
    }

    private string SetColonToLabel() {
        return !_isModifyModeEnabled
            ? ConstantValues.COLON
            : string.Empty;
    }

    private void SetBanner() {
        _operationDone = true;
    }

    public async Task AutoHideBanner() {
        await Task.Delay(5000);
        _operationDone = false;
    } 

    private async Task EditUserAsync() {
        var isValid = ValidatorService.Validate(_model, out _errors);

        if (isValid) {
            await AccountManagerService.UpdateUserAsync(_model);
            await AccountManagerService.UpdateEmployeeAsync(_model);

            _operationDone = true;
            Console.WriteLine("fATTO");
        } else {
            CustomValidator.DisplayErrors(_errors);
        }

        StateHasChanged();
    }
}