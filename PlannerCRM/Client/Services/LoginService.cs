namespace PlannerCRM.Client.Services;

public class LoginService
{
    private readonly HttpClient _http;
    private readonly ILogger<LoginService> _logger;

    public LoginService(HttpClient http, Logger<LoginService> logger) {
        _http = http;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> LoginAsync(EmployeeLoginDto dto) {
        try {
            return await _http
                .PostAsJsonAsync("api/account/login", dto);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task LogoutAsync() => 
        await _http.GetAsync("api/account/logout");
}