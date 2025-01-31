namespace PlannerCRM.Client.Services;

public class LoginService(HttpClient http)
{
    private readonly HttpClient _http = http;

    public async Task<HttpResponseMessage> LoginAsync(EmployeeLoginDto dto)
    {
        return await _http.PostAsJsonAsync("api/account/login", dto);
    }

    public async Task<HttpResponseMessage> LogoutAsync()
    {
        return await _http.GetAsync("api/account/logout");
    }
}
