using System.Net.Http.Json;

namespace PlannerCRM.Client.Services;

public class FetchService<TItem>(HttpClient http) : IFetchService<TItem>
    where TItem : class
{
    private readonly HttpClient _http = http;

    public async Task Create(string url, TItem item)
    {
        await _http.PostAsJsonAsync($"api/{url}", item);
    }

    public async Task<TItem> Read(string url, int itemId)
    {
        return await _http.GetFromJsonAsync<TItem>($"api/{url}");
    }

    public async Task Update(string url, TItem item)
    {
        await _http.PutAsJsonAsync($"api/{url}", item);
    }

    public async Task Delete(string url, int itemId)
    {
        await _http.DeleteAsync($"api/{url}/{itemId}");
    }

    public async Task<ICollection<TItem>> GetAll(string url, int offset, int limit)
    {
        return await _http.GetFromJsonAsync<ICollection<TItem>>($"api/{url}/{offset}/{limit}");
    }
}

