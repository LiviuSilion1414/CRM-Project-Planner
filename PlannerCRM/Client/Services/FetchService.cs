using System.Net.Http.Json;

namespace PlannerCRM.Client.Services;

public class FetchService<TItem>(HttpClient http, ControllersNames controllerName) : IFetchService<TItem>
    where TItem : class, new()
{
    private readonly HttpClient _http = http;
    private readonly string _controllerName = ControllersNamesHelper.GetControllerName(controllerName);

    public async Task Create(string action, TItem item)
        => await _http.PostAsJsonAsync($"api/{_controllerName}/{action}", item);

    public async Task<TItem> Read(string action, int itemId)
        => await _http.GetFromJsonAsync<TItem>($"api/{_controllerName}/{action}");

    public async Task Update(string action, TItem item)
        => await _http.PutAsJsonAsync($"api/{_controllerName}/{action}", item);

    public async Task Delete(string action, int itemId)
        => await _http.DeleteAsync($"api/{_controllerName}/{action}/{itemId}");

    public async Task<ICollection<TItem>> GetAll(string action, int offset, int limit)
        => await _http.GetFromJsonAsync<ICollection<TItem>>($"api/{_controllerName}/{action}/{offset}/{limit}");
}

