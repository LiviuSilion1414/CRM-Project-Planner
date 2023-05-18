using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Services;

public class AuthenticationStateService : AuthenticationStateProvider
{
    private readonly AuthenticationInfoService _authInfoService;
    private readonly LoginService _loginService;
    private CurrentUser _currentUser;

    public AuthenticationStateService(AuthenticationInfoService api) {
        this._authInfoService = api;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var identity = new ClaimsIdentity();
        try {
            var userInfo = await GetCurrentUserAsync();
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
        } catch (Exception exc) {
            Console.WriteLine("Exception:" + exc.ToString());
        }
        
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task<CurrentUser> GetCurrentUserAsync() {
        _currentUser = await _authInfoService.GetCurrentUserInfoAsync();

        if (_currentUser != null && _currentUser.IsAuthenticated) {
            return _currentUser;
        } else {
            return _currentUser;
        }
    }

    public async Task<string> LoginAsync(EmployeeLoginDto dto) {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return await _loginService.LoginAsync(dto);
    }
    
    public async Task LogoutAsync() {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        await _loginService.LogoutAsync();
    }

    public async Task<string> GetCurrentUserRoleAsync() {
        return await _authInfoService.GetCurrentUserRoleAsync(); 
    }    

    public async Task<CurrentEmployeeDto> GetCurrentEmployeeIdAsync(string email) {
       return await _authInfoService.GetCurrentEmployeeIdAsync(email);
    }

    public async Task<CurrentUser> GetCurrentUserInfoAsync() {
        return await _authInfoService.GetCurrentUserInfoAsync();
    }
}