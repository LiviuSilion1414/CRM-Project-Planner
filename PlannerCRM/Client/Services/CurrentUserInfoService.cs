using Newtonsoft.Json;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.Models;
using System.Net.Http.Json;

namespace PlannerCRM.Client.Services;

public class CurrentUserInfoService
{
    private readonly HttpClient _http;
    private readonly ILogger _logger;

    public CurrentUserInfoService(
        HttpClient http,
        ILogger logger
        )
    {
        _http = http;
        _logger = logger;
    }
    
    public async Task<CurrentUser> GetCurrentUserInfoAsync() {
        try
        {
            var response = await _http.GetAsync("account/current/user/info");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<CurrentUser>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new CurrentUser();
        }
    }

    public async Task<string> GetCurrentUserRoleAsync() {
        try
        {
            var response = await _http.GetAsync("account/user/role");
    
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return string.Empty;
        }
    }

    public async Task<CurrentEmployeeDto> GetCurrentEmployeeIdAsync(string email) {
        try
        {
            var response = await _http.GetAsync($"employee/get/id/{email}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<CurrentEmployeeDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new CurrentEmployeeDto();
        }
    }
}