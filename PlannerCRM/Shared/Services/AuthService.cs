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

            var objId = await _localStorage.GetItemAsync(CustomClaimTypes.Guid);
            var objIsAuthenticated = await _localStorage.GetItemAsync(CustomClaimTypes.IsAuthenticated);
            var objEmail = await _localStorage.GetItemAsync(CustomClaimTypes.Email);
            var objName = await _localStorage.GetItemAsync(CustomClaimTypes.Name);
            var objToken = await _localStorage.GetItemAsync(CustomClaimTypes.Token);
            var objMenuList = await _localStorage.GetItemAsync(CustomClaimTypes.Menu);
            var objRoleList = await _localStorage.GetItemAsync(CustomClaimTypes.Role);
            var objRoleListString = JsonSerializer.Deserialize<List<string>>((await _localStorage.GetItemAsync(CustomClaimTypes.RoleString)).ToString());

            List<Claim> claims = new List<Claim>()
            {
                new Claim(CustomClaimTypes.Token, token.ToString()),
                new Claim(CustomClaimTypes.Guid, objId.ToString()),
                new Claim(CustomClaimTypes.IsAuthenticated, objIsAuthenticated.ToString()),
                new Claim(CustomClaimTypes.Email, objEmail.ToString()),
                new Claim(CustomClaimTypes.Name, objName.ToString()),
                new Claim(CustomClaimTypes.Menu, objMenuList.ToString()),
                new Claim(CustomClaimTypes.Role, objRoleList.ToString())
            };

            foreach (var claim in objRoleListString)
            {
                claims.Add(new Claim(CustomClaimTypes.RoleString, claim));
            }

            if (claims == null || !claims.Any())
            {
                return anonymous;
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "Bearer", CustomClaimTypes.Name, CustomClaimTypes.RoleString)));
        } 
        catch 
        {
            throw;
        }
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