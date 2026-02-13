using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.Dtos;
using PlannerCRM.Shared.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Shared.Services;

public partial class FetchService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;
    private readonly LocalStorageService _localStorage;

    public CurrentUserDto? currentUser { get; set; } = new CurrentUserDto();
    public bool isBusy { get; set; }
    public string? token { get; set; }
    public List<string> supportedControllers { get; set; } = new List<string>();
    public List<MenuRoleDto> menuRolesList { get; set; } = new List<MenuRoleDto>();
    public List<RoleDto> rolesList { get; set; } = new List<RoleDto>();
    public AuthenticationState authState { get; set; } = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public FetchService (LocalStorageService localStorage, HttpClient http, AuthService auth)
    {
        _http = http;
        _localStorage = localStorage;
        _auth = auth;
        supportedControllers = typeof(ApiUrl)
                          .GetFields()
                          .Where(x => x.Name.EndsWith("_CONTROLLER"))
                          .Select(x => x.Name)
                          .ToList();
    }

    // metodo chiamato nel layout principale e se le voci di menu sono state cambiate
    public async Task LoadData()
    {
        authState = await _auth.GetAuthenticationStateAsync();

        if (authState.User.Identity.IsAuthenticated)
        {
            if (string.IsNullOrEmpty(token))
            {
                await GetBearerToken();
                await GetCurrentUserDataAsync();
            }
            if (!menuRolesList.Any())
            {
                await LoadMenuRoles();
            }
            if (!rolesList.Any())
            {
                await LoadGlobalRoles();
            }
        }
    }

    private async Task LoadMenuRoles()
    {
        try
        {
            isBusy = true;

            var result = await Menu_Role_List(new MenuRoleFilterDto());

            if (result.data is not null && result.hasCompleted && result.messageType == MessageType.Success)
            {
                menuRolesList = JsonSerializer.Deserialize<List<MenuRoleDto>>(result.data.ToString());
            }
            isBusy = false;
        } 
        catch (Exception exc)
        {
            throw;
        }
    }

    private async Task LoadGlobalRoles()
    {
        try
        {
            isBusy = true;

            var result = await Role_List(new RoleFilterDto());

            if (result.data is not null && result.hasCompleted && result.messageType == MessageType.Success)
            {
                rolesList = JsonSerializer.Deserialize<List<RoleDto>>(result.data.ToString());
            }
            isBusy = false;
        } 
        catch (Exception exc)
        {
            throw;
        }
    }

    public async Task GetCurrentUserDataAsync()
    {
        var authState = await _auth.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user != null || user.Identity.IsAuthenticated)
        {

        currentUser.token = token;
        currentUser.email = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Email)?.Value ?? string.Empty;
        currentUser.name = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Name)?.Value ?? string.Empty;
        currentUser.isAuthenticated = user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.IsAuthenticated)?.Value != null
                                                        ? bool.Parse(user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.IsAuthenticated)?.Value)
                                                        : false;
        currentUser.id = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Guid)?.Value != null
                                           ? Guid.Parse(user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Guid)?.Value)
                                           : Guid.Empty;
        currentUser.rolesList = user.Claims
                                   .Where(c => c.Type == CustomClaimTypes.Role)
                                   .Select(c => c.Value)
                                   .ToList();
        currentUser.menuList = user.Claims
                                   .Where(c => c.Type == CustomClaimTypes.Menu)
                                   .Select(c => c.Value)
                                   .ToList();
        }
    }

    public async Task<ResultDto> ExecuteAsync<TItem>(string controllerName, string endpoint, TItem data, ApiType apiType)
        where TItem : class
    {
        try
        {
            if (_http.DefaultRequestHeaders != null && !_http.DefaultRequestHeaders.Contains("Authorization"))
            {
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var response = new HttpResponseMessage();

            var endPointUrl = controllerName + endpoint;
            switch (apiType)
            {
                case ApiType.Get:
                    response = await _http.GetAsync(endPointUrl);
                    break;
                case ApiType.Post:
                    response = await _http.PostAsJsonAsync(endPointUrl, data);
                    break;
                case ApiType.Put:
                    response = await _http.PutAsJsonAsync(endPointUrl, data);
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

    public async Task<ResultDto> Dynamic_ExecuteAsync<TItem>(string controllerName, ActionType actionType, TItem data)
        where TItem : class
    {
        try
        {
            if (actionType == ActionType.Insert)
            {
                return await ExecuteAsync(controllerName, ApiUrl.INSERT, data, ApiType.Post);
            }
            if (actionType == ActionType.Update)
            {
                return await ExecuteAsync(controllerName, ApiUrl.UPDATE, data, ApiType.Put);
            }
            if (actionType == ActionType.Delete)
            {
                return await ExecuteAsync(controllerName, ApiUrl.DELETE, data, ApiType.Post);
            }

            return new ResultDto
            {
                data = null,
                hasCompleted = false,
                messageType = MessageType.Error,
                statusCode = System.Net.HttpStatusCode.ServiceUnavailable,
                message = string.Empty,
            };
        } 
        catch (Exception exc)
        {
            throw;
        }
    }

    public async Task GetBearerToken()
    {
        try
        {
            var value = await _localStorage.GetItemAsync(CustomClaimTypes.Token);
            token = value.ToString();
        } 
        catch (Exception)
        {
            throw;
        }
    }
}

