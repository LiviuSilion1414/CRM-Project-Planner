using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json.Linq;
using PlannerCRM.Shared.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
namespace PlannerCRM.Client.Services;

public class AuthService(HttpClient http, LocalStorageService localStorage) : AuthenticationStateProvider
{
    private readonly HttpClient _http = http;
    private readonly LocalStorageService _localStorage = localStorage;

    public async Task<HttpResponseMessage> LoginAsync(EmployeeLoginDto dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/account/login", dto);

            if (!res.IsSuccessStatusCode)
            {
                return new HttpResponseMessage() { StatusCode = res.StatusCode };
            }

            var token = await res.Content.ReadAsStringAsync();

            await _localStorage.SetItem(CustomClaimTypes.Token, token);

            return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK };
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<HttpResponseMessage> LogoutAsync()
    {
        try
        {
            await _localStorage.ClearAsync();

            return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK };
        } catch (Exception ex)
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
        catch (Exception ex)
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