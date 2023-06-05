using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Form;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Views;
using PlannerCRM.Shared.CustomExceptions;
using static System.Net.HttpStatusCode;
using static PlannerCRM.Shared.Constants.ConstantValues;
using Microsoft.AspNetCore.Authorization;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Services.Crud;

[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
public class DeveloperService
{
    private readonly HttpClient _http;
    private readonly Logger<DeveloperService> _logger;

    public DeveloperService(
        HttpClient http, 
        Logger<DeveloperService> logger) 
    {
        _http = http;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> AddWorkedHoursAsync(WorkTimeRecordFormDto dto) {
        try
        {
            return await _http.SendAsync(new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5032/worktimerecord/add"),
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            });
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error, nullRefExc.Message);
            return new HttpResponseMessage(NotFound);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error, argNullExc.Message);
            return new HttpResponseMessage(BadRequest);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.Log(LogLevel.Error, duplicateElemExc.Message);
            return new HttpResponseMessage(MultipleChoices);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new HttpResponseMessage(ServiceUnavailable);
        }
    } 

    public async Task<ActivityViewDto> GetActivityByIdAsync(int activityId) {
        try
        {
            var response = await _http.GetAsync($"http://localhost:5032/activity/get/{activityId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<ActivityViewDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new ActivityViewDto();
        }
    }

    public async Task<WorkOrderViewDto> GetWorkOrderByIdAsync(int workOrderId) {
        try
        {
            var response = await _http.GetAsync($"http://localhost:5032/workorder/get/for/view/{workOrderId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<WorkOrderViewDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new WorkOrderViewDto();
        }
    }

    public async Task<List<ActivityViewDto>> GetActivitiesByEmployeeIdAsync(int employeeId) {
        try
        {
            var response = await _http.GetAsync($"http://localhost:5032/activity/get/activity/per/employee/{employeeId}"); 
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<List<ActivityViewDto>>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new List<ActivityViewDto>();
        }
    }

    public async Task<int> GetWorkTimesSizeByEmployeeIdAsync(int employeeId) {
        try
        {
            var response = await _http.GetAsync($"worktimerecord/get/size/by/employee/{employeeId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<int>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return INVALID_ID;
        }
    }

    public async Task<List<WorkTimeRecordViewDto>> GetAllWorkTimeRecordsByEmployeeId(int employeeId) {
        try
        {
            var response = await _http.GetAsync($"http://localhost:5032/worktimerecord/get/by/employee/{employeeId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<List<WorkTimeRecordViewDto>>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new List<WorkTimeRecordViewDto>();
        }
    }
    
    public async Task<List<WorkTimeRecordViewDto>> GetWorkTimeRecordsByActivityId(int activityId) {
        try
        {
            var response = await _http.GetAsync($"http://localhost:5032/worktimerecord/get/{activityId}");
            var jsonObject = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<List<WorkTimeRecordViewDto>>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error, exc.Message);
            return new List<WorkTimeRecordViewDto>();
        }
    }
}
