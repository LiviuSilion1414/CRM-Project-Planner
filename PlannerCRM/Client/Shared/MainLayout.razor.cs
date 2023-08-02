using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Client.Shared;

public partial class MainLayout
{
    [Inject] public CurrentUserInfoService CurrentUserInfoService { get; set; }
    [Inject] public AuthenticationStateService AuthStateService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public LoginService LoginService { get; set; }
    
    private CurrentEmployeeDto _currentEmployee;
    private CurrentUser _currentUser;
    private bool _isAuthenticated;
    private string _userRole;

    protected override async Task OnInitializedAsync() {
        var authState = await AuthStateService.GetAuthenticationStateAsync();
        _isAuthenticated = authState.User.Identity.IsAuthenticated;
        
        if (_isAuthenticated) {
            _userRole = (await CurrentUserInfoService.GetCurrentUserRoleAsync())
                .ToUpper()
                .Replace('_', ' ');
        }
        var currentPage = NavigationManager.Uri.Replace(NavigationManager.BaseUri, "/");
        if (currentPage == "/login" || currentPage == "/") {
            var state = await AuthStateService.GetAuthenticationStateAsync();
            var loggedInUserID = new CurrentEmployeeDto();
            if (state.User.Identity.IsAuthenticated) {
                var loggedInUserRole = await CurrentUserInfoService.GetCurrentUserRoleAsync();
                if (state.User.Identity.Name != ADMIN_EMAIL) {
                    loggedInUserID = await CurrentUserInfoService.GetCurrentEmployeeIdAsync(state.User.Identity.Name);
                }
                foreach (var possibleRole in Enum.GetValues(typeof(Roles))) {
                    if (possibleRole.ToString() == loggedInUserRole) {
                        if (possibleRole.ToString() == nameof(Roles.SENIOR_DEVELOPER) ||
                            possibleRole.ToString() == nameof(Roles.JUNIOR_DEVELOPER)) {

                            NavigationManager.NavigateTo($"{loggedInUserRole.ToLower().Replace('_', '-')}/{loggedInUserID.Id}");
                        } else {
                            NavigationManager.NavigateTo($"{loggedInUserRole.ToLower().Replace('_', '-')}");
                        }
                    }
                }
            }
        }
    }

    public async Task NavigateBasedOnRole() {
        if (_isAuthenticated) {
            var role = await CurrentUserInfoService.GetCurrentUserRoleAsync();
            _currentUser = await CurrentUserInfoService.GetCurrentUserInfoAsync();

            if (_currentUser.UserName != ADMIN_EMAIL) {
                _currentEmployee = await CurrentUserInfoService.GetCurrentEmployeeIdAsync(_currentUser.UserName);
            }

            foreach (var possibleRole in Enum.GetValues(typeof(Roles))) {
                if (possibleRole.ToString() == role) {
                    if (possibleRole.ToString() == nameof(Roles.SENIOR_DEVELOPER) ||
                        possibleRole.ToString() == nameof(Roles.JUNIOR_DEVELOPER)) {
                        NavigationManager.NavigateTo($"{role.ToLower().Replace('_', '-')}/{_currentEmployee.Id}", true);
                    } else {
                        NavigationManager.NavigateTo($"{role.ToLower().Replace('_', '-')}", true);
                    }
                }
            }
        } else {
            NavigationManager.NavigateTo("/", true);
        }
    }

    public async Task OnClickLogout() {
        await LoginService.LogoutAsync();

        NavigationManager.NavigateTo("/login", true);
    }

}