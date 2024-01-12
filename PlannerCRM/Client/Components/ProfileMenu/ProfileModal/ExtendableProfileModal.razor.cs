namespace PlannerCRM.Client.Components.ProfileMenu.ProfileModal;

public partial class ExtendableProfileModal : ComponentBase
{
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public AuthenticationStateService AuthStateService { get; set; }
    [Inject] public LoginService LoginService { get; set; }

    private CurrentUser _currentUser;
    private bool _isMenuSwitched = false;

    protected override async Task OnInitializedAsync()
        => _currentUser = await AuthStateService.GetCurrentUserAsync();

    protected override void OnInitialized() {
        _currentUser= new() {
            ProfilePicture = new(),
            Claims = new()
        };
    }

    private void SwitchMenu() {
        _isMenuSwitched = !_isMenuSwitched;
    }

    private void NavigateToProfileSettings() {
        NavManager.NavigateTo($"/profile-settings/{_currentUser.Id}");
    }

    public async Task OnClickLogout() {
        await LoginService.LogoutAsync();
        NavManager.NavigateTo(ConstantValues.LOGIN_PAGE_LONG, true);
    }    
}