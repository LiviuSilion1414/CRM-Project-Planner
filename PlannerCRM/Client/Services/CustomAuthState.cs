using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Services;

public class CustomAuthState : AuthenticationStateProvider
{
    private readonly AuthService _api;
    private CurrentUser _currentUser;

    public CustomAuthState(AuthService api) {
        this._api = api;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var identity = new ClaimsIdentity();
        try {
            var userInfo = await GetCurrentUser();
            if (userInfo.IsAuthenticated) {
                var claims = new List<Claim> { 
                    new Claim(ClaimTypes.Name, _currentUser.UserName) 
                }.Concat(_currentUser.Claims
                    .Select(c => new Claim(c.Key, c.Value)));
               
                identity = new ClaimsIdentity(claims, "Server authentication");
            } else {
                identity = new ClaimsIdentity(new List<Claim> {
                    new Claim(ClaimTypes.Name, "Anonymous")
                });
            }
        } catch (HttpRequestException ex) {
            Console.WriteLine("Request failed:" + ex.ToString());
        }
        
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private async Task<CurrentUser> GetCurrentUser() {
        _currentUser = await _api.CurrentUserInfo();

        if (_currentUser != null && _currentUser.IsAuthenticated) {
            return _currentUser;
        } else {
            return _currentUser;
        }
    }
    public async Task<string> GetRole() {
        return await _api.GetRole();
    }

    public async Task Logout() {
        await _api.Logout();
        _currentUser = null;

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    
    public async Task Login(EmployeeLoginDTO loginParameters) {
        await _api.Login(loginParameters);
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<CurrentEmployee> GetCurrentEmployeeId(string email) {
       return await _api.GetCurrentEmployeeIdAsync(email);
    }

    public async Task<CurrentUser> GetCurrentUserInfoAsync() {
        return await _api.CurrentUserInfo();
    }
}