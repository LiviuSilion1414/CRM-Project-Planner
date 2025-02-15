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

            await _localStorage.SetItem("id", user.Id);
            await _localStorage.SetItem("token", user.Token);
            await _localStorage.SetItem("name", user.Name);
            await _localStorage.SetItem("email", user.Email);
            await _localStorage.SetItem("roles", user.Roles);
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

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, currentUser.Id!.ToString()!),
                    new Claim(ClaimTypes.Email, currentUser.Email),
                    new Claim(ClaimTypes.Name, currentUser.Name),
            ];

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
                object? id = await _localStorage.GetItemAsync("id");
                object? name = await _localStorage.GetItemAsync("name");
                object? email = await _localStorage.GetItemAsync("email");
                object? isAuthenticated = await _localStorage.GetItemAsync("isAuthenticated");

                return new CurrentUser()
                {
                    Id = int.Parse(id.ToString()),
                    Name = name.ToString(),
                    Email = email.ToString(),
                    Token = token.ToString(),
                    IsAuthenticated = bool.Parse(isAuthenticated.ToString()),
                    Roles = [],
                    Claims = []
                };
            }
            return null;
        } 
        catch (Exception ex)
        {
            throw;
        }
    }
}