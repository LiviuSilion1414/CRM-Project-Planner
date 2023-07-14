namespace PlannerCRM.Client.Services.Crud;

public class OperationManagerCrudService
{
    private readonly HttpClient _http;
    private readonly Logger<OperationManagerCrudService> _logger;

    public OperationManagerCrudService(HttpClient http, Logger<OperationManagerCrudService> logger) {
        _http = http;
        _logger = logger;
    }

    public async Task<int> GetCollectionSize() {
        try {
            var response = await _http.GetAsync("http://localhost:5032/workorder/get/size/");
            var jsonObject = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<int>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return default;
        }
    }

    public async Task<List<WorkOrderViewDto>> GetCollectionPaginated(int limit = 0, int offset = 5) {
        try {
            var response = await _http.GetAsync($"http://localhost:5032/workorder/get/paginated/{limit}/{offset}");
            var jsonObject = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<WorkOrderViewDto>>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }
   
    public async Task<List<WorkOrderViewDto>> GetAllWorkOrdersAsync() {
        try {
            var response = await _http.GetAsync("workorder/get/all");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<List<WorkOrderViewDto>>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrder) {
       try {    
            var response = await _http.GetAsync($"workorder/search/{workOrder}");
            var jsonObject = await response.Content.ReadAsStringAsync();
 
            return JsonConvert.DeserializeObject<List<WorkOrderSelectDto>>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string employee) {
        try {
            var response = await _http.GetAsync($"employee/search/{employee}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<List<EmployeeSelectDto>>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    public async Task<HttpResponseMessage> AddWorkOrderAsync(WorkOrderFormDto dto) {
        try {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5032/workorder/add"),
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            });   
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.BadRequest);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.BadRequest);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.MultipleChoices);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> EditWorkOrderAsync(WorkOrderFormDto dto) {
        try {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost:5032/workorder/edit"),
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            });
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.BadRequest);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.BadRequest);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.MultipleChoices);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> AddActivityAsync(ActivityFormDto dto) {
        try { 
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5032/activity/add"),
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            });
        }  catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.BadRequest);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.BadRequest);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.MultipleChoices);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> EditActivityAsync(ActivityFormDto dto) {
        try  {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost:5032/activity/edit"),
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            });
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.BadRequest);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.BadRequest);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.MultipleChoices);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> DeleteActivityAsync(int activityId) {
        try {
            return await _http.DeleteAsync($"http://localhost:5032/activity/delete/{activityId}");
        } catch (InvalidOperationException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.NotImplemented);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> DeleteWorkOrderAsync(int workOrderId) {
        try {
            return await _http.DeleteAsync($"http://localhost:5032/workorder/delete/{workOrderId}");
        } catch (InvalidOperationException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.NotImplemented);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(HttpStatusCode.ServiceUnavailable);
        }
    }
    
    public async Task<ActivityFormDto> GetActivityForEditAsync(int activityId) {
        try {
            var response = await _http.GetAsync($"activity/get/for/edit/{activityId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<ActivityFormDto>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    public async Task<ActivityDeleteDto> GetActivityForDeleteAsync(int activityId) {
        try {
            var response = await _http.GetAsync($"activity/get/for/delete/{activityId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<ActivityDeleteDto>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    public async Task<WorkOrderViewDto> GetWorkOrderForViewAsync(int workOrderId) {
        try {
            var response = await _http.GetAsync($"workorder/get/for/view/{workOrderId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<WorkOrderViewDto>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    public async Task<WorkOrderFormDto> GetWorkOrderForEditAsync(int workOrderId) {
        try {
            var response = await _http.GetAsync($"workorder/get/for/edit/{workOrderId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<WorkOrderFormDto>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    public async Task<WorkOrderDeleteDto> GetWorkOrderForDeleteAsync(int workOrderId) {
       try {
         var response = await _http.GetAsync($"workorder/get/for/delete/{workOrderId}");
         var jsonObject = await response.Content.ReadAsStringAsync();
 
         return JsonConvert.DeserializeObject<WorkOrderDeleteDto>(jsonObject);     
       } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new(); 
       }
    }

    public async Task<List<ActivityFormDto>> GetActivityPerWorkOrderAsync(int workOrderId) {
        try {
            var response = await _http.GetAsync($"activity/get/activity/per/workorder/{workOrderId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<List<ActivityFormDto>>(jsonObject);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }
}