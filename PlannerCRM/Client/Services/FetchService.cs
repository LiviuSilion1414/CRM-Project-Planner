using System.Net.Http.Json;

namespace PlannerCRM.Client.Services;

public class FetchService<TItem>(HttpClient http) : IFetchService<TItem>
    where TItem : class
{
    private readonly HttpClient _http = http;
    private readonly string _baseUrl = "http://localhost:5030/api";

    public async Task Create(string url, TItem item)
    {
        await _http.PostAsJsonAsync($"{_baseUrl}/{url}", item);
    }

    public async Task<TItem> Read(string url, int itemId)
    {
        return await _http.GetFromJsonAsync<TItem>($"{_baseUrl}/{url}");
    }

    public async Task Update(string url, TItem item)
    {
        await _http.PutAsJsonAsync($"{_baseUrl}/{url}", item);
    }

    public async Task Delete(string url, int itemId)
    {
        await _http.DeleteAsync($"{_baseUrl}/{url}/{itemId}");
    }

    public async Task<ICollection<TItem>> GetAll(string url, int offset, int limit)
    {
        return await _http.GetFromJsonAsync<ICollection<TItem>>($"{_baseUrl}/{url}/{offset}/{limit}");
    }
}

