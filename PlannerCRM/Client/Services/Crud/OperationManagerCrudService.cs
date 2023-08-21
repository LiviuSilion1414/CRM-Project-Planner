namespace PlannerCRM.Client.Services.Crud;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public class OperationManagerCrudService
{
    private readonly HttpClient _http;
    private readonly ILogger<OperationManagerCrudService> _logger;

    public OperationManagerCrudService(HttpClient http, Logger<OperationManagerCrudService> logger) {
        _http = http;
        _logger = logger;
    }


    public async Task<HttpResponseMessage> AddClientAsync(ClientFormDto dto) {
        try {
            return await _http.PostAsJsonAsync("api/client/add", dto);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> EditClientAsync(ClientFormDto dto) {
        try {
            return await _http.PutAsJsonAsync("api/client/edit", dto);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> DeleteClientAsync(int clientId) {
        try {
            return await _http.DeleteAsync($"api/client/delete/{clientId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<ClientViewDto> GetClientForViewAsync(int clientId) {
        try {
            return await _http
                .GetFromJsonAsync<ClientViewDto>($"api/client/get/for/view/{clientId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
    public async Task<ClientFormDto> GetClientForEditAsync(int clientId) {
        try {
            return await _http
                .GetFromJsonAsync<ClientFormDto>($"api/client/get/for/edit/{clientId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<ClientDeleteDto> GetClientForDeleteAsync(int clientId) {
        try {
            return await _http
                .GetFromJsonAsync<ClientDeleteDto>($"api/client/get/for/delete/{clientId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<ClientViewDto>> GetClientsPaginated(int limit = 0, int offset = 5) {
        try {
            return await _http
                .GetFromJsonAsync<List<ClientViewDto>>($"api/client/get/paginated/{limit}/{offset}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<int> GetCollectionSize() {
        try {
            return await _http
                .GetFromJsonAsync<int>("api/workorder/get/size/");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<WorkOrderViewDto>> GetCollectionPaginated(int limit = 0, int offset = 5) {
        try {
            return await _http
                .GetFromJsonAsync<List<WorkOrderViewDto>>($"api/workorder/get/paginated/{limit}/{offset}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrder) {
       try {    
            return await _http
                .GetFromJsonAsync<List<WorkOrderSelectDto>>($"api/workorder/search/{workOrder}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string employee) {
        try {
            return await _http
                .GetFromJsonAsync<List<EmployeeSelectDto>>($"api/employee/search/{employee}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<HttpResponseMessage> AddWorkOrderAsync(WorkOrderFormDto dto) {
        try {
            return await _http
                .PostAsJsonAsync("api/workorder/add", dto);   
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> EditWorkOrderAsync(WorkOrderFormDto dto) {
        try {
            return await _http
                .PutAsJsonAsync("api/workorder/edit", dto);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> AddActivityAsync(ActivityFormDto dto) {
        try { 
            return await _http
                .PutAsJsonAsync("api/activity/add", dto);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> EditActivityAsync(ActivityFormDto dto) {
        try  {
            return await _http
                .PutAsJsonAsync("api/activity/edit", dto);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> DeleteActivityAsync(int activityId) {
        try {
            return await _http
                .DeleteAsync($"api/activity/delete/{activityId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> DeleteWorkOrderAsync(int workOrderId) {
        try {
            return await _http
                .DeleteAsync($"api/workorder/delete/{workOrderId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }
    
    public async Task<ActivityFormDto> GetActivityForEditAsync(int activityId) {
        try {
            return await _http
                .GetFromJsonAsync<ActivityFormDto>($"api/activity/get/for/edit/{activityId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<ActivityDeleteDto> GetActivityForDeleteAsync(int activityId) {
        try {
            return await _http
                .GetFromJsonAsync<ActivityDeleteDto>($"api/activity/get/for/delete/{activityId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
    
    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    public async Task<WorkOrderViewDto> GetWorkOrderForViewAsync(int workOrderId) {
        try {
            return await _http
                .GetFromJsonAsync<WorkOrderViewDto>($"api/workorder/get/for/view/{workOrderId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<WorkOrderFormDto> GetWorkOrderForEditAsync(int workOrderId) {
        try {
            var response = await _http.GetAsync($"api/workorder/get/for/edit/{workOrderId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<WorkOrderFormDto>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<WorkOrderDeleteDto> GetWorkOrderForDeleteAsync(int workOrderId) {
       try {
            return await _http
                .GetFromJsonAsync<WorkOrderDeleteDto>($"api/workorder/get/for/delete/{workOrderId}");
       } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new(); 
       }
    }

    public async Task<List<ActivityViewDto>> GetActivityPerWorkOrderAsync(int workOrderId) {
        try {
            return await _http.GetFromJsonAsync<List<ActivityViewDto>>($"api/activity/get/activity/per/workorder/{workOrderId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<int> GetCollectionSizeAsync() {
        try {
            return await _http
                .GetFromJsonAsync<int>("api/client/get/size/");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
}