using System.Text;
using Newtonsoft.Json;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using static System.Net.HttpStatusCode;

namespace PlannerCRM.Client.Services;

public class LoginService
{
    private readonly HttpClient _http;
    private readonly ILogger _logger;

    public LoginService(
        HttpClient http,
        ILogger logger)
    {
        _http = http;
        _logger = logger;
    }
    
    public async Task<HttpResponseMessage> LoginAsync(EmployeeLoginDto dto) {
        try
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                RequestUri = new Uri("account/login"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            });
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new HttpResponseMessage(BadRequest);
        }
    }

    public async Task LogoutAsync() {
        await _http.GetAsync("account/logout");
    }
}