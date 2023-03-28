using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;
using System.Net.Http.Json;

namespace PlannerCRM.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<CurrentUser> CurrentUserInfo() {
        return await _httpClient.GetFromJsonAsync<CurrentUser>("http://localhost:5032/account/current/user/info");
    }

    public async Task<IEnumerable<string>> GetRoles() {
        return await _httpClient.GetFromJsonAsync<IEnumerable<string>>("http://localhost:5032/account/user/role");
    }

    public async Task Login(EmployeeLoginDTO loginRequest) {
        await _httpClient.PostAsJsonAsync("http://localhost:5032/account/login", loginRequest);
    }

    public async Task Logout() {
        var result = await _httpClient.GetAsync("http://localhost:5032/account/logout");
    }
}