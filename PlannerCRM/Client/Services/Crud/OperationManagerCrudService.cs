using System.Text;
using Newtonsoft.Json;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using static System.Net.HttpStatusCode;

namespace PlannerCRM.Client.Services.Crud;

public class OperationManagerCrudService
{
    private readonly HttpClient _http;
    private readonly ILogger _logger;

    public OperationManagerCrudService(
        HttpClient http,
        ILogger logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<List<WorkOrderViewDto>> GetAllWorkOrdersAsync() {
        try
        {
            var response = await _http.GetAsync("workorder/get/all");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<List<WorkOrderViewDto>>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new List<WorkOrderViewDto>();
        }
    }

    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrder) {
       try
       {
            var response = await _http.GetAsync($"workorder/search/{workOrder}");
            var jsonObject = await response.Content.ReadAsStringAsync();
 
            return JsonConvert.DeserializeObject<List<WorkOrderSelectDto>>(jsonObject);
       }
       catch (Exception exc)
       {
            _logger.Log(LogLevel.Error, exc.Message);
            return new List<WorkOrderSelectDto>();
       }
    }

    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string employee) {
        try
        {
            var response = await _http.GetAsync($"employee/search/{employee}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<List<EmployeeSelectDto>>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new List<EmployeeSelectDto>();
        }
    }

    public async Task<HttpResponseMessage> AddWorkOrderAsync(WorkOrderFormDto model) {
        try
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("workorder/add"),
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            });
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.LogError(nullRefExc.Message, nullRefExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.LogError(argNullExc.Message, argNullExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.LogError(duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return new HttpResponseMessage(MultipleChoices);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> EditWorkOrderAsync(WorkOrderFormDto model) {
        try {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Put,
                RequestUri = new Uri("workorder/edit"),
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            });
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.LogError(nullRefExc.Message, nullRefExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.LogError(argNullExc.Message, argNullExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.LogError(duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return new HttpResponseMessage(MultipleChoices);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> AddActivityAsync(ActivityFormDto model) {
        try 
        { 
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("activity/add"),
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            });
        } 
        catch (NullReferenceException nullRefExc)
        {
            _logger.LogError(nullRefExc.Message, nullRefExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.LogError(argNullExc.Message, argNullExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.LogError(duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return new HttpResponseMessage(MultipleChoices);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> EditActivityAsync(ActivityFormDto model) {
        try 
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Put,
                RequestUri = new Uri("activity/edit"),
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            });
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.LogError(nullRefExc.Message, nullRefExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.LogError(argNullExc.Message, argNullExc.StackTrace);
            return new HttpResponseMessage(BadRequest);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.LogError(duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return new HttpResponseMessage(MultipleChoices);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> DeleteActivityAsync(int activityId) {
        try
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"activity/delete/{activityId}")
            });
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.LogError(invalidOpExc.Message, invalidOpExc.StackTrace);
            return new HttpResponseMessage(NotImplemented);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }

    public async Task<HttpResponseMessage> DeleteWorkOrderAsync(int workOrderId) {
        try
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"workorder/delete/{workOrderId}")
            });
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.LogError(invalidOpExc.Message, invalidOpExc.StackTrace);
            return new HttpResponseMessage(NotImplemented);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    }
    
    public async Task<ActivityFormDto> GetActivityForEditAsync(int activityId) {
        try
        {
            var response = await _http.GetAsync($"activity/get/for/edit/{activityId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<ActivityFormDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new ActivityFormDto();
        }
    }

    public async Task<ActivityDeleteDto> GetActivityForDeleteAsync(int activityId) {
        try
        {
            var response = await _http.GetAsync($"activity/get/for/delete/{activityId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<ActivityDeleteDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new ActivityDeleteDto();
        }
    }

    public async Task<WorkOrderViewDto> GetWorkOrderForViewAsync(int workOrderId) {
        try
        {
            var response = await _http.GetAsync($"workorder/get/for/view/{workOrderId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<WorkOrderViewDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new WorkOrderViewDto();
        }
    }

    public async Task<WorkOrderFormDto> GetWorkOrderForEditAsync(int workOrderId) {
        try
        {
            var response = await _http.GetAsync($"workorder/get/for/edit/{workOrderId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<WorkOrderFormDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new WorkOrderFormDto();
        }
    }

    public async Task<WorkOrderDeleteDto> GetWorkOrderForDeleteAsync(int workOrderId) {
       try
       {
         var response = await _http.GetAsync($"workorder/get/for/delete/{workOrderId}");
         var jsonObject = await response.Content.ReadAsStringAsync();
 
         return JsonConvert.DeserializeObject<WorkOrderDeleteDto>(jsonObject);     
       }
       catch (Exception exc)
       {
            _logger.Log(LogLevel.Error, exc.Message);
            return new WorkOrderDeleteDto(); 
       }
    }

    public async Task<List<ActivityFormDto>> GetActivityPerWorkOrderAsync(int workOrderId) {
        try
        {
            var response = await _http.GetAsync($"activity/get/activity/per/workorder/{workOrderId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<List<ActivityFormDto>>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new List<ActivityFormDto>();
        }
    }
}