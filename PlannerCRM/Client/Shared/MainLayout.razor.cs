using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Client.Shared;

public partial class MainLayout
{
    [Inject] public CurrentUserInfoService CurrentUserInfoService { get; set; }
    [Inject] public AuthenticationStateService AuthStateService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public LoginService LoginService { get; set; }
    
    private string _userRole;
    private CurrentEmployeeDto _currentEmployee;
    private CurrentUser _currentUser;
    private bool _isAuthenticated;

    protected override async Task OnInitializedAsync()
        => await HandleAuthenticationAndNavigation();

    public async Task NavigateBasedOnRole()
        => await HandleAuthenticationAndNavigation();

    protected override void OnInitialized() {
        _currentEmployee = new();
        _currentUser = new();
    }

    private async Task HandleAuthenticationAndNavigation() {
        var authState = await AuthStateService.GetAuthenticationStateAsync();
        _currentUser = await AuthStateService.GetCurrentUserAsync();
        _isAuthenticated = authState.User.Identity.IsAuthenticated;

        if (_isAuthenticated) {
            _userRole = await CurrentUserInfoService.GetCurrentUserRoleAsync();

            if (_currentUser.UserName != ADMIN_EMAIL) {
                _currentEmployee = await CurrentUserInfoService.GetCurrentEmployeeIdAsync(_currentUser.UserName);
            }

            var navigationUrl = NavigationUtil.BuildNavigationUrl(_userRole, _currentEmployee.Id);
            NavManager.NavigateTo(navigationUrl);
        }
    }

    public async Task OnClickLogout() {
        await LoginService.LogoutAsync();
        NavManager.NavigateTo(ConstantValues.LOGIN_PAGE_LONG, true);
    }
}
