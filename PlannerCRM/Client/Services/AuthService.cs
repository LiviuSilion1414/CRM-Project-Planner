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
        var result = await _httpClient.GetFromJsonAsync<CurrentUser>("http://localhost:5032/account/current/user/info");
        return result;
    }

    public async Task Login(EmployeeLoginDTO loginRequest) {
        await _httpClient.PostAsJsonAsync("http://localhost:5032/account/login", loginRequest);
    }

    public async Task Logout() {
        var result = await _httpClient.GetAsync("http://localhost:5032/account/logout");
    }
}