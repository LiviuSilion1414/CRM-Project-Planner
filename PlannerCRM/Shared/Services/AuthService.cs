using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Client.Services;
using PlannerCRM.Shared.Dtos;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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

            await _localStorage.SetItemAsync(CustomClaimTypes.Token, result.data.ToString());

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

            if (token == null) return anonymous;

            var claims = ParseClaimsFromJwt(token.ToString());

            if (claims == null)
            {
                return anonymous;
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "Bearer", CustomClaimTypes.Name, CustomClaimTypes.Role)));
        } 
        catch 
        {
            throw;
        }
    }

    public async Task<Guid> GetCurrentUserIdAsync()
    {
        return Guid.Parse((await GetAuthenticationStateAsync()).User.Claims.First(x => x.Type == CustomClaimTypes.Guid).Value);
    }

    public async Task<CurrentUserDto> GetCurrentUserDataAsync(FetchService fetchService)
    {
        var currentUser = new CurrentUserDto();

        var authState = await GetAuthenticationStateAsync();
        var user = authState.User;

        if (user == null || !user.Identity.IsAuthenticated)
        {
            fetchService.currentUser = new CurrentUserDto();
            return currentUser;
        }

        currentUser.email = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Email)?.Value ?? string.Empty;
        currentUser.name = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Name)?.Value ?? string.Empty;
        currentUser.isAuthenticated = user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.IsAuthenticated)?.Value != null
                                                        ? bool.Parse(user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.IsAuthenticated)?.Value)
                                                        : false;
        currentUser.id = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Guid)?.Value != null
                                           ? Guid.Parse(user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Guid)?.Value)
                                           : Guid.Empty;
        currentUser.roles = user.Claims
                                   .Where(c => c.Type == CustomClaimTypes.Role)
                                   .Select(c => c.Value)
                                   .ToList();

        fetchService.currentUser = currentUser;

        return currentUser;
    }

    private IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
    {
        try
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(CustomClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(CustomClaimTypes.Role, parsedRole));
                    }
                } else
                {
                    claims.Add(new Claim(CustomClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(CustomClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            claims.Add(new Claim(CustomClaimTypes.Token, jwt));

            return claims;
        } 
        catch (Exception)
        {
            throw;
        }
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        try
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}