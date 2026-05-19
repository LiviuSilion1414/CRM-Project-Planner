using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.Dtos;
using PlannerCRM.Shared.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Shared.Services;

public partial class FetchService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;
    private readonly LocalStorageService _localStorage;

    public CurrentUserDto? currentUser { get; set; } = new CurrentUserDto();
    public bool isBusy { get; set; } = false;
    public List<string> supportedControllers { get; set; } = new List<string>();

    public Dictionary<Type, List<PropertyMeta>> lookupMetaProperties { get; set; } = new Dictionary<Type, List<PropertyMeta>>();
    public AuthenticationState authState { get; set; } = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public FetchService(LocalStorageService localStorage, HttpClient http, AuthService auth)
    {
        _http = http;
        _localStorage = localStorage;
        _auth = auth;
        supportedControllers = typeof(ApiUrl)
                          .GetFields()
                          .Where(x => x.Name.EndsWith("_CONTROLLER"))
                          .Select(x => x.Name)
                          .ToList();
        LoadAllDtoMetaProperties();
    }

    // metodo chiamato nel layout principale e se le voci di menu sono state cambiate
    public async Task LoadData()
    {
        authState = await _auth.GetAuthenticationStateAsync();

        if (authState.User.Identity.IsAuthenticated)
        {
            await GetCurrentUserDataAsync(authState.User);
        }
    }

    public async Task ReloadSettings()
    {
        await _localStorage.RemoveItemAsync(CustomClaimTypes.Setting);

        currentUser.settingsList = null;
        currentUser?.claims?.RemoveAll(x => x.Issuer == CustomClaimTypes.Setting);

        try
        {
            List<SettingDto> settingsList = new List<SettingDto>();
            var result = await Settings_List(new SettingFilterDto());

            if (result.data is not null && result.hasCompleted && result.messageType == MessageType.Success)
            {
                settingsList = JsonSerializer.Deserialize<List<SettingDto>>(result.data.ToString());
            }
            
            await _localStorage.SetItemAsync(CustomClaimTypes.Setting, settingsList);

            currentUser.claims.Add(new Claim(CustomClaimTypes.Setting, JsonSerializer.Serialize(settingsList)));
            currentUser.settingsList = settingsList;
        } 
        catch (Exception exc)
        {
        }
    }

    private void LoadAllDtoMetaProperties()
    {
        try
        {
            isBusy = true;

            lookupMetaProperties = new Dictionary<Type, List<PropertyMeta>>()
            {
                { typeof(ActivityDto), DynamicPropertyMetaLoader.GetModelProperties(new ActivityDto()) },
                { typeof(EmployeeDto), DynamicPropertyMetaLoader.GetModelProperties(new EmployeeDto()) },
                { typeof(EmployeesRolesDto), DynamicPropertyMetaLoader.GetModelProperties(new EmployeesRolesDto()) },
                { typeof(EmployeeActivityDto), DynamicPropertyMetaLoader.GetModelProperties(new EmployeeActivityDto()) },
                { typeof(FirmClientDto), DynamicPropertyMetaLoader.GetModelProperties(new FirmClientDto()) },
                { typeof(MenuDto), DynamicPropertyMetaLoader.GetModelProperties(new MenuDto()) },
                { typeof(MenuRoleDto), DynamicPropertyMetaLoader.GetModelProperties(new MenuRoleDto()) },
                { typeof(RoleDto), DynamicPropertyMetaLoader.GetModelProperties(new RoleDto()) },
                { typeof(SettingDto), DynamicPropertyMetaLoader.GetModelProperties(new SettingDto()) },
                { typeof(SystemLogDto), DynamicPropertyMetaLoader.GetModelProperties(new SystemLogDto()) },
                { typeof(WorkOrderDto), DynamicPropertyMetaLoader.GetModelProperties(new WorkOrderDto()) },
                { typeof(WorkTimeDto), DynamicPropertyMetaLoader.GetModelProperties(new WorkTimeDto()) },
            };

            isBusy = false;
        } catch (Exception exc)
        {
            throw;
        }
    }

    public async Task GetCurrentUserDataAsync(ClaimsPrincipal user)
    {
        currentUser.id = Guid.Parse((await _localStorage.GetItemAsync(CustomClaimTypes.Guid)).ToString());
        currentUser.isAuthenticated = bool.Parse((await _localStorage.GetItemAsync(CustomClaimTypes.IsAuthenticated)).ToString());
        currentUser.email = (await _localStorage.GetItemAsync(CustomClaimTypes.Email)).ToString();
        currentUser.name = (await _localStorage.GetItemAsync(CustomClaimTypes.Name)).ToString();
        currentUser.token = (await _localStorage.GetItemAsync(CustomClaimTypes.Token)).ToString();
        currentUser.roleList = JsonSerializer.Deserialize<List<RoleDto>>((await _localStorage.GetItemAsync(CustomClaimTypes.Role)).ToString());
        currentUser.roleListString = JsonSerializer.Deserialize<List<string>>((await _localStorage.GetItemAsync(CustomClaimTypes.RoleString)).ToString());
        currentUser.menuList = JsonSerializer.Deserialize<List<MenuDto>>((await _localStorage.GetItemAsync(CustomClaimTypes.Menu)).ToString());
        currentUser.settingsList = JsonSerializer.Deserialize<List<SettingDto>>((await _localStorage.GetItemAsync(CustomClaimTypes.Setting)).ToString());
        currentUser.menuTree = MenuTreeBuilder.Build(currentUser.menuList);
    }




    public async Task<ResultDto> ExecuteAsync<TItem>(string controllerName, string endpoint, TItem data, ApiType apiType)
        where TItem : class
    {
        try
        {
            if (_http.DefaultRequestHeaders != null && !_http.DefaultRequestHeaders.Contains("Authorization"))
            {
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUser.token}");
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

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResultDto>();
            }

            return new ResultDto
            {
                data = null,
                hasCompleted = false,
                messageType = MessageType.Error,
                statusCode = System.Net.HttpStatusCode.ServiceUnavailable,
                message = string.Empty,
            };
        } catch (Exception exc)
        {
            throw;
        }
    }

    public async Task<ResultDto> Dynamic_ExecuteAsync<TItem>(string controllerName, ActionType actionType, TItem data)
        where TItem : class
    {
        try
        {
            if (actionType == ActionType.INSERT)
            {
                return await ExecuteAsync(controllerName, ApiUrl.INSERT, data, ApiType.Post);
            } else if (actionType == ActionType.UPDATE)
            {
                return await ExecuteAsync(controllerName, ApiUrl.UPDATE, data, ApiType.Put);
            } else if (actionType == ActionType.DELETE)
            {
                return await ExecuteAsync(controllerName, ApiUrl.DELETE, data, ApiType.Post);
            } else if (actionType == ActionType.DELETE_MULTIPLE)
            {
                return await ExecuteAsync(controllerName, ApiUrl.DELETE_MULTIPLE, data, ApiType.Post);
            }

            return new ResultDto
            {
                data = null,
                hasCompleted = false,
                messageType = MessageType.Error,
                statusCode = System.Net.HttpStatusCode.ServiceUnavailable,
                message = string.Empty,
            };
        } catch (Exception exc)
        {
            throw;
        }
    }

    public async Task<ResultDto> Dynamic_ExecuteAsync<Guid>(string controllerName, ActionType actionType, List<Guid?> data)
    {
        try
        {
            if (actionType == ActionType.INSERT)
            {
                return await ExecuteAsync(controllerName, ApiUrl.INSERT, data, ApiType.Post);
            } else if (actionType == ActionType.UPDATE)
            {
                return await ExecuteAsync(controllerName, ApiUrl.UPDATE, data, ApiType.Put);
            } else if (actionType == ActionType.DELETE)
            {
                return await ExecuteAsync(controllerName, ApiUrl.DELETE, data, ApiType.Post);
            } else if (actionType == ActionType.DELETE_MULTIPLE)
            {
                return await ExecuteAsync(controllerName, ApiUrl.DELETE_MULTIPLE, data, ApiType.Post);
            }

            return new ResultDto
            {
                data = null,
                hasCompleted = false,
                messageType = MessageType.Error,
                statusCode = System.Net.HttpStatusCode.ServiceUnavailable,
                message = string.Empty,
            };
        } catch (Exception exc)
        {
            throw;
        }
    }
}

