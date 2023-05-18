using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;
using System.Net.Http.Json;

namespace PlannerCRM.Client.Services;

public class AuthenticationInfoService
{
    private readonly HttpClient _http;

    public AuthenticationInfoService(HttpClient http) {
        _http = http;
    }

    public async Task<CurrentUser> GetCurrentUserInfoAsync() {
        return await _http.GetFromJsonAsync<CurrentUser>("http://localhost:5032/account/current/user/info");
    }

    public async Task<string> GetCurrentUserRoleAsync() {
        return await _http.GetFromJsonAsync<string>("http://localhost:5032/account/user/role");
    }

    public async Task<CurrentEmployeeDto> GetCurrentEmployeeIdAsync(string email) {
        return await _http.GetFromJsonAsync<CurrentEmployeeDto>($"http://localhost:5032/employee/get/id/{email}");
    }
}