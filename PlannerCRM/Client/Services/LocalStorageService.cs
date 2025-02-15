using Microsoft.JSInterop;
using PlannerCRM.Shared.Models;
using System.Text.Json;

namespace PlannerCRM.Client.Services;

public class LocalStorageService(IJSRuntime js)
{
    private readonly IJSRuntime _js = js;

    public async Task<object?> GetItemAsync(string key)
    {
        try
        {
            var result = await _js.InvokeAsync<object?>("localStorage.getItem", key);

            if (result is not null)
            {
                return JsonSerializer.Deserialize<object?>(result.ToString());
            }

            return null;
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task SetItem(string key, object value)
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task RemoveItemAsync(string key)
    {
        try 
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", key);
        }
        catch (Exception ex) 
        {
            throw;
        }
    }

    public async Task ClearAsync()
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.clear");
        } 
        catch (Exception ex)
        {
            throw;
        }
    }
}