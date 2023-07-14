namespace PlannerCRM.Client.Services;

public class LoginService
{
    private readonly HttpClient _http;
    private readonly Logger<LoginService> _logger;

    public LoginService(HttpClient http, Logger<LoginService> logger) {
        _http = http;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> LoginAsync(EmployeeLoginDto dto) {
        try {
            return await _http.SendAsync(new HttpRequestMessage() {
                RequestUri = new Uri("http://localhost:5032/account/login"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            });
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.BadRequest);
        }
    }

    public async Task LogoutAsync() => await _http.GetAsync("account/logout");
}