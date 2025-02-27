using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Services;

public class FetchService(LocalStorageService localStorage, HttpClient http)
{
    private readonly HttpClient _http = http;
    private readonly LocalStorageService _localStorage = localStorage;

    public bool IsBusy { get; set; }

    public async Task<ResultDto> ExecuteAsync(string endpoint, SearchFilterDto filter, ApiType apiType)
    {
        try
        {
            if (!_http.DefaultRequestHeaders.Contains("Authorization"))
            {
                var jwt = await GetBearerToken() ?? throw new InvalidOperationException("Jwt token was not found. Please login and retry");
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
            }

            var result = new ResultDto();
            var response = new HttpResponseMessage();

            switch (apiType)
            {
                case ApiType.Get:
                    response = await _http.GetAsync($"api/{endpoint}");
                    break;
                case ApiType.Post:
                    response = await _http.PostAsJsonAsync($"api/{endpoint}", filter);
                    break;
                case ApiType.Put:
                    response = await _http.PutAsJsonAsync($"api/{endpoint}", filter);
                    break;
            }

            if (!response.IsSuccessStatusCode)
            {
                if (((int)response.StatusCode) == 500 || ((int)response.StatusCode) == 400)
                {
                    var errorMessage = (await response.Content.ReadFromJsonAsync<ResultDto>()).Message;
                    result.MessageType = MessageType.Error;
                    result.HasCompleted = false;
                    result.Message = !string.IsNullOrEmpty(errorMessage) || !string.IsNullOrWhiteSpace(errorMessage)
                                     ? errorMessage
                                     : "Something went wrong, please retry!";
                    return result;
                }

                //Method not allowed
                if (((int)response.StatusCode) == 405)
                {
                    result.MessageType = MessageType.Warning;
                    result.HasCompleted = false;
                    result.Message = "Method not allowed";
                    return result;

                }

                //Not found
                if (((int)response.StatusCode) == 404)
                {
                    result.MessageType = MessageType.Warning;
                    result.HasCompleted = false;
                    result.Message = "Data not found";
                    return result;

                }

                //Unauthorized
                if (((int)response.StatusCode) == 401)
                {
                    result.MessageType = MessageType.Warning;
                    result.HasCompleted = false;
                    result.Message = "Unauthorized";
                    return result;
                }
            } 
            else
            {
                result.MessageType = MessageType.Success;
                result.HasCompleted = true;
                result.Message = "Operation completed";
                return result;
            }

            result.MessageType = MessageType.Error;
            result.HasCompleted = false;
            result.Message = "Something went wrong, please retry!";
            return result;
        } 
        catch (Exception ex)
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

