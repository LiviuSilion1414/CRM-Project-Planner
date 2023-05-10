using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;
using System.Net.Http.Json;

namespace PlannerCRM.Client.Services;

public class AuthService //usare questo service per le chiamate http e suddividerlo in categorie
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<CurrentUser> CurrentUserInfo() {
        return await _httpClient.GetFromJsonAsync<CurrentUser>("http://localhost:5032/account/current/user/info");
    }

    public async Task<string> GetRole() {
        var responseMessage = await _httpClient.GetAsync("http://localhost:5032/account/user/role");
        return await responseMessage.Content.ReadAsStringAsync();
    }

    public async Task Login(EmployeeLoginDto loginRequest) {
        await _httpClient.PostAsJsonAsync("http://localhost:5032/account/login", loginRequest);
    }

    public async Task Logout() {
        var result = await _httpClient.GetAsync("http://localhost:5032/account/logout");
    }

    public async Task<CurrentEmployeeDto> GetCurrentEmployeeIdAsync(string email) {
        return await _httpClient.GetFromJsonAsync<CurrentEmployeeDto>($"http://localhost:5032/employee/get/id/{email}");
    }
}