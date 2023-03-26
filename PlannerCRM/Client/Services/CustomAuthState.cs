using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Services;

public class CustomAuthState : AuthenticationStateProvider
{
    private readonly AuthService api;
    private CurrentUser _currentUser;
    public CustomAuthState(AuthService api) {
        this.api = api;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var identity = new ClaimsIdentity();
        try {
            var userInfo = await GetCurrentUser();
            if (userInfo.IsAuthenticated) {
                var claims = new[] { new Claim(ClaimTypes.Name, _currentUser.UserName) }.Concat(_currentUser.Claims.Select(c => new Claim(c.Key, c.Value)));
                identity = new ClaimsIdentity(claims, "Server authentication");
            }
        } catch (HttpRequestException ex) {
            Console.WriteLine("Request failed:" + ex.ToString());
        }

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private async Task<CurrentUser> GetCurrentUser() {
        if (_currentUser != null && _currentUser.IsAuthenticated) return _currentUser;
        _currentUser = await api.CurrentUserInfo();
        return _currentUser;
    }
    public async Task Logout() {
        await api.Logout();
        _currentUser = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    public async Task Login(EmployeeLoginDTO loginParameters) {
        await api.Login(loginParameters);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}