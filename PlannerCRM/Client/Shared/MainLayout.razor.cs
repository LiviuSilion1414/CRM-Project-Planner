using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Client.Shared;

public partial class MainLayout
{
    [Inject] public CurrentUserInfoService CurrentUserInfoService { get; set; }
    [Inject] public AuthenticationStateService AuthStateService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public LoginService LoginService { get; set; }
    
    private string _userRole;
    private CurrentEmployeeDto _currentEmployee;
    private CurrentUser _currentUser;
    private bool _isAuthenticated;

    protected override async Task OnInitializedAsync()
        => await HandleAuthenticationAndNavigation();

    public async Task NavigateBasedOnRole()
        => await HandleAuthenticationAndNavigation();

    private async Task HandleAuthenticationAndNavigation() {
        var authState = await AuthStateService.GetAuthenticationStateAsync();
        _currentUser = await AuthStateService.GetCurrentUserAsync();
        _isAuthenticated = authState.User.Identity.IsAuthenticated;

        if (_isAuthenticated) {
            _userRole = await CurrentUserInfoService.GetCurrentUserRoleAsync();

            if (_currentUser.UserName != ADMIN_EMAIL) {
                _currentEmployee = await CurrentUserInfoService.GetCurrentEmployeeIdAsync(_currentUser.UserName);
            }

            if (Enum.TryParse(_userRole, out Roles parsedRole)) {
                var navigationUrl = BuildNavigationUrl(parsedRole);
                NavigationManager.NavigateTo(navigationUrl);
            }
        } else {
            NavigationManager.NavigateTo(ConstantValues.LOGIN_PAGE_LONG);
        }
    }

    private string BuildNavigationUrl(Roles role) {
        var url = $"{role.ToString().ToLower().Replace('_', '-')}";
        
        if (role == Roles.SENIOR_DEVELOPER || role == Roles.JUNIOR_DEVELOPER) {
            url += $"/{_currentEmployee.Id}";
        }
        
        return url;
    }

    public async Task OnClickLogout() {
        await LoginService.LogoutAsync();
        NavigationManager.NavigateTo(ConstantValues.LOGIN_PAGE_LONG, true);
    }
}
