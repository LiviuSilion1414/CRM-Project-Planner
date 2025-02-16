using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.Models;
using System.Security.Claims;
using System.Text.Json;
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
            var str = await res.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<CurrentUser>(str, 
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (!res.IsSuccessStatusCode)
            {
                return new HttpResponseMessage() { StatusCode = res.StatusCode };
            }

            await _localStorage.SetItem("id", user.Guid);
            await _localStorage.SetItem("token", user.Token);
            await _localStorage.SetItem("name", user.Name);
            await _localStorage.SetItem("email", user.Email);
            await _localStorage.SetItem("roles", user.Roles);
            await _localStorage.SetItem("claims", user.Claims);
            await _localStorage.SetItem("claimsOk", user.ClaimsOk);
            await _localStorage.SetItem("isAuthenticated", user.IsAuthenticated);

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
        }
        catch (Exception ex) 
        {
            throw;
        }
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            Claim[] claims = [new Claim(ClaimTypes.Name, currentUser.Name)];

            claims = currentUser.Roles.Select(x => new Claim(ClaimTypes.Role, x)).ToArray();

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: nameof(AuthService))));
        }
        catch (Exception ex) 
        {
            throw;
        }
    }

    public async Task<CurrentUser?> GetCurrentUserAsync()
    {
        try
        {
            object? token = await _localStorage.GetItemAsync("token");

            if (token is not null)
            {   
                string tokenString = token.ToString();
                Guid guid = Guid.Parse((await _localStorage.GetItemAsync("id")).ToString());
                string name = (await _localStorage.GetItemAsync("name")).ToString();
                string email = (await _localStorage.GetItemAsync("email")).ToString();
                bool isAuthenticated =bool.Parse((await _localStorage.GetItemAsync("isAuthenticated")).ToString());

                List<string> roles = (await _localStorage.GetItemAsync("roles")).ToString().Split(',').ToList();
                List<Claim> claimsOk = (await _localStorage.GetItemAsync("claimsOk")).ToString()
                                                                                     .Split(',')
                                                                                     .Select(x => new Claim(ClaimTypes.Role, x))
                                                                                     .ToList();
                Dictionary<string, string> claims = (await _localStorage.GetItemAsync("claims")).ToString()
                                                                                                .Split(',')
                                                                                                .ToDictionary(k => k, v => v);

                var user = new CurrentUser()
                {
                    Guid = guid,
                    Name = name,
                    Email = email,
                    Token = tokenString,
                    IsAuthenticated = isAuthenticated,
                    Roles = roles,
                    Claims = claims,
                    ClaimsOk = claimsOk
                };

                return user;
            }
            return null;
        } 
        catch (Exception ex)
        {
            throw;
        }
    }
}