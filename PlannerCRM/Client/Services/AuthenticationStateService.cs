using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Services;

public class AuthenticationStateService : AuthenticationStateProvider
{
    private readonly CurrentUserInfoService _authInfoService;
    private readonly ILogger _logger;
    private CurrentUser _currentUser;

    public AuthenticationStateService(
        CurrentUserInfoService api, 
        ILogger logger)
    {
        this._authInfoService = api;
        this._logger = logger;
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
}