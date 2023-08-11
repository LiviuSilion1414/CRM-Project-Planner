namespace PlannerCRM.Client.Services;

public class CurrentUserInfoService
{
    private readonly HttpClient _http;
    private readonly ILogger<CurrentUserInfoService> _logger;
    
    public CurrentUserInfoService(HttpClient http, Logger<CurrentUserInfoService> logger) {
        _http = http;
        _logger = logger;
    }

    public async Task<CurrentUser> GetCurrentUserInfoAsync() {
        try {
            return await _http
                .GetFromJsonAsync<CurrentUser>("api/account/current/user/info");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);
            
            return new();
        }
    }

    public async Task<string> GetCurrentUserRoleAsync() {
        try {
            return await _http
                .GetFromJsonAsync<string>("api/account/user/role");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return string.Empty;
        }
    }

    public async Task<CurrentEmployeeDto> GetCurrentEmployeeIdAsync(string email) {
        try {
            return await _http
                .GetFromJsonAsync<CurrentEmployeeDto>($"api/employee/get/id/{email}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
}