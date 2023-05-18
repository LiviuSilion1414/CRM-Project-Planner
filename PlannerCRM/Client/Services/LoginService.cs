using System.Net.Http.Json;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

namespace PlannerCRM.Client.Services;

public class LoginService
{
    private readonly HttpClient _http;

    public LoginService(HttpClient http) {
        _http = http;
    }

    public async Task<string> LoginAsync(EmployeeLoginDto dto) {
        var response = await _http.PostAsJsonAsync("http://localhost:5032/account/login", dto);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task LogoutAsync() {
        await _http.GetFromJsonAsync<HttpResponseMessage>("http://localhost:5032/account/logout");
    }
}