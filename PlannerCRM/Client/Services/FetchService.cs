using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Services;

public class FetchService(LocalStorageService localStorage, HttpClient http)
{
    private readonly HttpClient _http = http;
    private readonly LocalStorageService _localStorage = localStorage;

    public bool IsBusy { get; set; }

    public async Task<ResultDto> ExecuteAsync<TItem>(string controllerName, string endpoint, TItem data, ApiType apiType)
        where TItem : class
    {
        try
        {
            if (!_http.DefaultRequestHeaders.Contains("Authorization"))
            {
                var jwt = await GetBearerToken() ?? throw new InvalidOperationException("Jwt token was not found. Please login and retry");
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
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

            return await response.Content.ReadFromJsonAsync<ResultDto>();
        } 
        catch 
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

