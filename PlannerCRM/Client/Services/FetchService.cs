using System.Net.Http.Json;

namespace PlannerCRM.Client.Services;

public class FetchService<TItem>(HttpClient http) : IFetchService<TItem>
    where TItem : class, new()
{
    private readonly HttpClient _http = http;

    public async Task Create(string controllerName, string action, TItem item)
        => await _http.PostAsJsonAsync($"api/{controllerName}/{action}", item);

    public async Task<TItem> Read(string controllerName, string action, int itemId)
        => await _http.GetFromJsonAsync<TItem>($"api/{controllerName}/{action}");

    public async Task Update(string controllerName, string action, TItem item)
        => await _http.PutAsJsonAsync($"api/{controllerName}/{action}", item);

    public async Task Delete(string controllerName, string action, int itemId)
        => await _http.DeleteAsync($"api/{controllerName}/{action}/{itemId}");

    public async Task<List<TItem>> GetAll(string controllerName, string action, int offset, int limit)
        => await _http.GetFromJsonAsync<List<TItem>>($"api/{controllerName}/{action}/{offset}/{limit}");

    public async Task<List<TItem>> GetAll(string controllerName, string parameterizedUrl)
        => await _http.GetFromJsonAsync<List<TItem>>($"api/{controllerName}/{parameterizedUrl}");
}

