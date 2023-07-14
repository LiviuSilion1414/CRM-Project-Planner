namespace PlannerCRM.Client.Services;

public class CurrentUserInfoService
{
    private readonly HttpClient _http;
    private readonly Logger<CurrentUserInfoService> _logger;

    public CurrentUserInfoService(HttpClient http, Logger<CurrentUserInfoService> logger) {
        _http = http;
        _logger = logger;
    }
    
    public async Task<CurrentUser> GetCurrentUserInfoAsync() {
        try {
            var response = await _http.GetAsync("account/current/user/info");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<CurrentUser>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    public async Task<string> GetCurrentUserRoleAsync() {
        try {
            var response = await _http.GetAsync("account/user/role");
    
            return await response.Content.ReadAsStringAsync();
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return string.Empty;
        }
    }

    public async Task<CurrentEmployeeDto> GetCurrentEmployeeIdAsync(string email) {
        try {
            var response = await _http.GetAsync($"employee/get/id/{email}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<CurrentEmployeeDto>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }
}