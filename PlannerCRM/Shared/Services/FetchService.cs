using Microsoft.AspNetCore.Components.Authorization;
using PlannerCRM.Shared.Dtos;
using PlannerCRM.Shared.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
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
    public List<MenuDto> menusList { get; set; } = new List<MenuDto>();

    public Dictionary<Type, List<PropertyMeta>> lookupMetaProperties { get; set; } = new Dictionary<Type, List<PropertyMeta>>();
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
                await LoadAllDtoMetaProperties();
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
            if (!menusList.Any())
            {
                await LoadGlobalMenus();
            }
        }
    }

    private async Task LoadAllDtoMetaProperties()
    {
        try
        {
            isBusy = true;

            lookupMetaProperties = new Dictionary<Type, List<PropertyMeta>>()
            {
                { typeof(ActivityDto), DynamicPropertyMetaLoader.GetModelProperties(new ActivityDto()) },
                { typeof(EmployeeDto), DynamicPropertyMetaLoader.GetModelProperties(new EmployeeDto()) },
                { typeof(EmployeesRolesDto), DynamicPropertyMetaLoader.GetModelProperties(new EmployeesRolesDto()) },
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

    private async Task LoadMenuRoles()
    {
        try
        {
            isBusy = true;

            MenuRoleFilterDto menuRoleFilter = new MenuRoleFilterDto()
            {
                idList = currentUser.menuRolesList
            };

            var result = await MenuRoles_List(menuRoleFilter);

            if (result.data is not null && result.hasCompleted && result.messageType == MessageType.Success)
            {
                menuRolesList = JsonSerializer.Deserialize<List<MenuRoleDto>>(result.data.ToString()).OrderBy(x => x.fkIdMenuNavigation.ranking).ToList();
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

    private async Task LoadGlobalMenus()
    {
        try
        {
            isBusy = true;

            var result = await Menu_List(new MenuFilterDto());

            if (result.data is not null && result.hasCompleted && result.messageType == MessageType.Success)
            {
                menusList = JsonSerializer.Deserialize<List<MenuDto>>(result.data.ToString());
            }
            isBusy = false;
        } catch (Exception exc)
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
        currentUser.menuRolesList = user.Claims
                                   .Where(c => c.Type == CustomClaimTypes.Menu)
                                   .SelectMany(c => c.Value.Split(","))
                                   .Select(c => 
                                   {
                                       if (Guid.TryParse(c, out Guid menuId))
                                       {
                                           return menuId;
                                       }

                                       return Guid.Empty;
                                   })
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
            var json = JsonSerializer.Serialize(data);
            switch (apiType)
            {
                case ApiType.Get:
                    response = await _http.GetAsync(endPointUrl);
                    break;
                case ApiType.Post:
                    //response = await _http.PostAsJsonAsync(endPointUrl, data);
                    response = await _http.PostAsync(endPointUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                    break;
                case ApiType.Put:
                    //response = await _http.PutAsJsonAsync(endPointUrl, data);
                    response = await _http.PutAsync(endPointUrl, new StringContent(json, Encoding.UTF8, "application/json"));
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
            if (actionType == ActionType.INSERT)
            {
                return await ExecuteAsync(controllerName, ApiUrl.INSERT, data, ApiType.Post);
            }
            else if (actionType == ActionType.UPDATE)
            {
                return await ExecuteAsync(controllerName, ApiUrl.UPDATE, data, ApiType.Put);
            }
            else if (actionType == ActionType.DELETE)
            {
                return await ExecuteAsync(controllerName, ApiUrl.DELETE, data, ApiType.Post);
            }
            else if (actionType == ActionType.DELETE_MULTIPLE)
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
        } 
        catch (Exception exc)
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

