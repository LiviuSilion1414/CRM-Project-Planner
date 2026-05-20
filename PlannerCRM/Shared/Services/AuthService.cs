using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.Dtos;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace PlannerCRM.Shared.Services;

public class AuthService(HttpClient http, LocalStorageService localStorage) : AuthenticationStateProvider
{
    private readonly HttpClient _http = http;
    private readonly LocalStorageService _localStorage = localStorage;

    public async Task<ResultDto> LoginAsync(LoginDto dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/account/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                return new ResultDto() 
                { 
                    statusCode = response.StatusCode,
                    data = null,
                    hasCompleted = false,
                    message = "Login failed",
                    messageType = MessageType.Warning,
                    id = null
                };
            }

            var result = await response.Content.ReadFromJsonAsync<ResultDto>();
            var currentUser = JsonSerializer.Deserialize<CurrentUserDto>(result.data.ToString());

            await _localStorage.SetItemAsync(CustomClaimTypes.Guid, currentUser.id);
            await _localStorage.SetItemAsync(CustomClaimTypes.IsAuthenticated, currentUser.isAuthenticated);
            await _localStorage.SetItemAsync(CustomClaimTypes.Email, currentUser.email);
            await _localStorage.SetItemAsync(CustomClaimTypes.Name, currentUser.name);
            await _localStorage.SetItemAsync(CustomClaimTypes.Token, currentUser.token);
            await _localStorage.SetItemAsync(CustomClaimTypes.Menu, currentUser.menuList);
            await _localStorage.SetItemAsync(CustomClaimTypes.Role, currentUser.roleList);
            await _localStorage.SetItemAsync(CustomClaimTypes.RoleString, currentUser.roleList.Select(x => x.name).ToList());
            await _localStorage.SetItemAsync(CustomClaimTypes.Setting, currentUser.settingsList);

            return new ResultDto()
            {
                statusCode = HttpStatusCode.OK,
                data = result.data,
                hasCompleted = true,
                message = result.message,
                messageType = MessageType.Success,
                id = result.id
            };
        } 
        catch 
        {
            throw;
        }
    }

    public async Task<ResultDto> LogoutAsync()
    {
        try
        {
            await _localStorage.ClearAsync();

            return new ResultDto()
            {
                statusCode = HttpStatusCode.OK,
                data = null,
                hasCompleted = true,
                message = string.Empty,
                messageType = MessageType.Success,
                id = null
            };
        } 
        catch 
        {
            throw;
        }
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            object token = await _localStorage.GetItemAsync(CustomClaimTypes.Token);

            if (string.IsNullOrEmpty(token?.ToString()))
            {
                return anonymous;
            }

            var id = await _localStorage.GetItemAsync(CustomClaimTypes.Guid);
            var isAuthenticated = await _localStorage.GetItemAsync(CustomClaimTypes.IsAuthenticated);
            var email = await _localStorage.GetItemAsync(CustomClaimTypes.Email);
            var name = await _localStorage.GetItemAsync(CustomClaimTypes.Name);
            var menuList = await _localStorage.GetItemAsync(CustomClaimTypes.Menu);
            var roleList = await _localStorage.GetItemAsync(CustomClaimTypes.Role);
            var settingList = await _localStorage.GetItemAsync(CustomClaimTypes.Setting);
            var roleListString = JsonSerializer.Deserialize<List<string>>((await _localStorage.GetItemAsync(CustomClaimTypes.RoleString)).ToString());

            List<Claim> claims = new List<Claim>()
            {
                new Claim(CustomClaimTypes.Token, token.ToString()),
                new Claim(CustomClaimTypes.Guid, id.ToString()),
                new Claim(CustomClaimTypes.IsAuthenticated, isAuthenticated.ToString()),
                new Claim(CustomClaimTypes.Email, email.ToString()),
                new Claim(CustomClaimTypes.Name, name.ToString()),
                new Claim(CustomClaimTypes.Menu, menuList.ToString()),
                new Claim(CustomClaimTypes.Role, roleList.ToString()),
                new Claim(CustomClaimTypes.Setting, settingList.ToString())
            };

            foreach (var claim in roleListString)
            {
                claims.Add(new Claim(CustomClaimTypes.RoleString, claim));
            }

            if (claims == null || !claims.Any())
            {
                return anonymous;
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "Bearer")));
        } 
        catch 
        {
            throw;
        }
    }
}