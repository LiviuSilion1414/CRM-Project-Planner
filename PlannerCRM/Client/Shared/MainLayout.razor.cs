using static PlannerCRM.Shared.Constants.ConstantValues;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services;

namespace PlannerCRM.Client.Shared;

public partial class MainLayout
{
    [Inject] CurrentUserInfoService CurrentUserInfoService { get; set; }
    [Inject] AuthenticationStateService AuthStateService { get; set; }
    [Inject] NavigationManager NavManager { get; set; }
    [Inject] LoginService LoginService { get; set; }
    
    private CurrentEmployeeDto _CurrentEmployee { get; set; }
    private CurrentUser _CurrentUser { get; set; }
    private bool IsAuthenticated { get; set; }
    private string UserRole { get; set; }

    protected override async Task OnInitializedAsync() {
        var authState = await AuthStateService.GetAuthenticationStateAsync();
        IsAuthenticated = authState.User.Identity.IsAuthenticated;
        
        if (IsAuthenticated) {
            UserRole = (await CurrentUserInfoService.GetCurrentUserRoleAsync())
                .ToUpper()
                .Replace('_', ' ');
        }
        var currentPage = NavManager.Uri.Replace(NavManager.BaseUri, "/");
        if (currentPage == "/login" || currentPage == "/") {
            var state = await AuthStateService.GetAuthenticationStateAsync();
            var loggedInUserID = new CurrentEmployeeDto();
            var loggedInUserRole = string.Empty;
            if (state.User.Identity.IsAuthenticated) {
                loggedInUserRole = await CurrentUserInfoService.GetCurrentUserRoleAsync();
                if (state.User.Identity.Name != ADMIN_EMAIL) {
                    loggedInUserID = await CurrentUserInfoService.GetCurrentEmployeeIdAsync(state.User.Identity.Name);
                }
                foreach (var possibleRole in Enum.GetValues(typeof(Roles))) {
                    if (possibleRole.ToString() == loggedInUserRole) {
                        if (possibleRole.ToString() == nameof(Roles.SENIOR_DEVELOPER) ||
                            possibleRole.ToString() == nameof(Roles.JUNIOR_DEVELOPER)) {

                            NavManager.NavigateTo($"{loggedInUserRole.ToLower().Replace('_', '-')}/{loggedInUserID.Id}");
                        } else {
                            NavManager.NavigateTo($"{loggedInUserRole.ToLower().Replace('_', '-')}");
                        }
                    }
                }
            }
        }
    }

    public async Task NavigateBasedOnRole() {
        if (IsAuthenticated) {
            var role = await CurrentUserInfoService.GetCurrentUserRoleAsync();
            _CurrentUser = await CurrentUserInfoService.GetCurrentUserInfoAsync();

            if (_CurrentUser.UserName != ADMIN_EMAIL) {
                _CurrentEmployee = await CurrentUserInfoService.GetCurrentEmployeeIdAsync(_CurrentUser.UserName);
            }

            foreach (var possibleRole in Enum.GetValues(typeof(Roles))) {
                if (possibleRole.ToString() == role) {
                    if (possibleRole.ToString() == nameof(Roles.SENIOR_DEVELOPER) ||
                        possibleRole.ToString() == nameof(Roles.JUNIOR_DEVELOPER)) {
                        NavManager.NavigateTo($"{role.ToLower().Replace('_', '-')}/{_CurrentEmployee.Id}", true);
                    } else {
                        NavManager.NavigateTo($"{role.ToLower().Replace('_', '-')}", true);
                    }
                }
            }
        } else {
            NavManager.NavigateTo("/", true);
        }
    }

    public async Task OnClickLogout() {
        await LoginService.LogoutAsync();

        NavManager.NavigateTo("/login", true);
    }

}