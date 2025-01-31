using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.Models;
using System.Security.Claims;

namespace PlannerCRM.Client.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string LocalStorageKey = "currentUser";

    private readonly LocalStorageService _localStorageService;

    public CustomAuthenticationStateProvider(LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await GetCurrentUserAsync();

        if (currentUser == null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, currentUser.Id!.ToString()!),
                new Claim(ClaimTypes.Email, currentUser.Email),
                new Claim(ClaimTypes.Name, currentUser.Name),
                new Claim(ClaimTypes.Role, currentUser.Role)

        ];

        var authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: nameof(CustomAuthenticationStateProvider))));

        return authenticationState;
    }

    public async Task SetCurrentUserAsync(CurrentUser? currentUser)
    {
        await _localStorageService.SetItem(LocalStorageKey, currentUser);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public Task<CurrentUser?> GetCurrentUserAsync() => _localStorageService.GetItemAsync<CurrentUser>(LocalStorageKey);
}