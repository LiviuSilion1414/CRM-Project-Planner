using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Services;

public class FetchService<TItem>(LocalStorageService localStorage, HttpClient http)
    where TItem : class, new()
{
    private readonly HttpClient _http = http;
    private readonly LocalStorageService _localStorage = localStorage;

    public async Task Create(string controllerName, string action, TItem item)
    {
        try
        {
            var token = await GetBearerToken();
            var containsToken = _http.DefaultRequestHeaders.Contains("Authorization");
            if (!containsToken)
            {
                _http.DefaultRequestHeaders.Add("Authorization", token);
            }
            await _http.PostAsJsonAsync($"api/{controllerName}/{action}", item);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<TItem> Read(string controllerName, string action, Guid itemId)
    {
        try
        {
            var token = await GetBearerToken();
            var containsToken = _http.DefaultRequestHeaders.Contains("Authorization");
            if (!containsToken)
            {
                _http.DefaultRequestHeaders.Add("Authorization", token);
            }
            return await _http.GetFromJsonAsync<TItem>($"api/{controllerName}/{action}");
        } 
        catch (Exception)
        {

            throw;
        }
    }

    public async Task Update(string controllerName, string action, TItem item)
    {
        try
        {
            var token = await GetBearerToken();
            var containsToken = _http.DefaultRequestHeaders.Contains("Authorization");
            if (!containsToken)
            {
                _http.DefaultRequestHeaders.Add("Authorization", token);
            }
            await _http.PutAsJsonAsync($"api/{controllerName}/{action}", item);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task Delete(string controllerName, string action, Guid itemId)
    {
        try
        {
            var token = await GetBearerToken();
            var containsToken = _http.DefaultRequestHeaders.Contains("Authorization");
            if (!containsToken)
            {
                _http.DefaultRequestHeaders.Add("Authorization", token);
            }
            await _http.DeleteAsync($"api/{controllerName}/{action}/{itemId}");
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<TItem>> GetAll(string controllerName, string action, int limit, int offset)
    {
        try
        {
            var token = await GetBearerToken();
            var containsToken = _http.DefaultRequestHeaders.Contains("Authorization");
            if (!containsToken)
            {
                var res = _http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                if (!res)
                    Console.WriteLine("Something went wrong");
            }
            var data = await _http.GetFromJsonAsync<List<TItem>>($"api/{controllerName}/{action}/{limit}/{offset}");

            return data;
        } 
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<List<TItem>> GetAll(string controllerName, string parameterizedUrl)
    {
        try
        {
            var token = await GetBearerToken();
            var containsToken = _http.DefaultRequestHeaders.Contains("Authorization");
            if (!containsToken)
            {
                _http.DefaultRequestHeaders.Add("Authorization", token);
            }
            return await _http.GetFromJsonAsync<List<TItem>>($"api/{controllerName}/{parameterizedUrl}");
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> GetBearerToken()
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

