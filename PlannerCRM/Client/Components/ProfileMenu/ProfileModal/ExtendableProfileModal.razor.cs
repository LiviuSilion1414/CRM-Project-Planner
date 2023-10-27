namespace PlannerCRM.Client.Components.ProfileMenu.ProfileModal;

public partial class ExtendableProfileModal : ComponentBase
{
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public AuthenticationStateService AuthStateService { get; set; }
    [Inject] public LoginService LoginService { get; set; }

    private CurrentUser _currentUser = new();
    private bool _isMenuSwitched = false;

    protected override async Task OnInitializedAsync()
        => _currentUser = await AuthStateService.GetCurrentUserAsync();

    private void SwitchMenu() {
        _isMenuSwitched = !_isMenuSwitched;
    }

    public async Task OnClickLogout() {
        await LoginService.LogoutAsync();
        NavManager.NavigateTo(ConstantValues.LOGIN_PAGE_LONG, true);
    }    
}