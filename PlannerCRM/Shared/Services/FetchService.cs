using PlannerCRM.Shared.Dtos;
using PlannerCRM.Shared.Services;
using System.Net.Http.Json;

namespace PlannerCRM.Client.Services;

public partial class FetchService(LocalStorageService localStorage, HttpClient http)
{
    private readonly HttpClient _http = http;
    private readonly LocalStorageService _localStorage = localStorage;

    public CurrentUserDto? currentUser  = new ();
    public bool IsBusy { get; set; }
    public string? Token { get; set; }
    public List<MenuRoleDto> MenuRolesList { get; set; } = new List<MenuRoleDto>();
    public List<RoleDto> RolesList { get; set; } = new List<RoleDto>();


    public async Task<ResultDto> ExecuteAsync<TItem>(string controllerName, string endpoint, TItem data, ApiType apiType)
        where TItem : class
    {
        try
        {
            if (!_http.DefaultRequestHeaders.Contains("Authorization"))
            {
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            }

            var response = new HttpResponseMessage();

            switch (apiType)
            {
                case ApiType.Get:
                    response = await _http.GetAsync($"{controllerName}/{endpoint}");
                    break;
                case ApiType.Post:
                    response = await _http.PostAsJsonAsync($"{controllerName}/{endpoint}", data);
                    break;
                case ApiType.Put:
                    response = await _http.PutAsJsonAsync($"{controllerName}/{endpoint}", data);
                    break;
            }

            var result = await response.Content.ReadFromJsonAsync<ResultDto>();
            return result;
        } 
        catch(Exception exc)
        {
            throw;
        }
    }

    public async Task<string?> GetBearerToken()
    {
        try
        {
            return (await _localStorage.GetItemAsync(CustomClaimTypes.Token)).ToString();
        } 
        catch (Exception)
        {
            throw;
        }
    }
}

