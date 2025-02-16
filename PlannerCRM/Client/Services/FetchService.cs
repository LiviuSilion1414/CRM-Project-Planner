namespace PlannerCRM.Client.Services;

public class FetchService<TItem>(LocalStorageService localStorage, HttpClient http)
    where TItem : class, new()
{
    private readonly HttpClient _http = http;
    private readonly LocalStorageService _localStorage = localStorage;

    public async Task Create(string controllerName, string action, TItem item)
        => await _http.PostAsJsonAsync($"api/{controllerName}/{action}", item);

    public async Task<TItem> Read(string controllerName, string action, int itemId)
        => await _http.GetFromJsonAsync<TItem>($"api/{controllerName}/{action}");

    public async Task Update(string controllerName, string action, TItem item)
        => await _http.PutAsJsonAsync($"api/{controllerName}/{action}", item);

    public async Task Delete(string controllerName, string action, int itemId)
        => await _http.DeleteAsync($"api/{controllerName}/{action}/{itemId}");

    public async Task<List<TItem>> GetAll(string controllerName, string action, int limit, int offset)
    {   
        var token = await GetBearerToken();
        var containsToken = _http.DefaultRequestHeaders.Contains("Authorization");
        if (!containsToken)
        {
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        return await _http.GetFromJsonAsync<List<TItem>>($"api/{controllerName}/{action}/{limit}/{offset}");
    }

    public async Task<List<TItem>> GetAll(string controllerName, string parameterizedUrl)
        => await _http.GetFromJsonAsync<List<TItem>>($"api/{controllerName}/{parameterizedUrl}");



    async Task<string> GetBearerToken()
    {
        return (await _localStorage.GetItemAsync("token")).ToString();
    }
}

