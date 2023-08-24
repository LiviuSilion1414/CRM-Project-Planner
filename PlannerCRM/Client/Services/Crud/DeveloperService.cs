namespace PlannerCRM.Client.Services.Crud;

[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
public class DeveloperService
{
    private readonly HttpClient _http;
    private readonly ILogger<DeveloperService> _logger;

    public DeveloperService(HttpClient http, Logger<DeveloperService> logger) {
        _http = http;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> AddWorkedHoursAsync(WorkTimeRecordFormDto dto) {
        try {
            return await _http
                .PostAsJsonAsync("api/worktimerecord/add", dto);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    } 

    public async Task<ActivityViewDto> GetActivityByIdAsync(int activityId) {
        try {
            return await _http
                .GetFromJsonAsync<ActivityViewDto>($"api/activity/get/{activityId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<WorkOrderViewDto> GetWorkOrderByIdAsync(int workOrderId) {
        try {
            return await _http
                .GetFromJsonAsync<WorkOrderViewDto>($"api/workorder/get/for/view/{workOrderId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<ActivityViewDto>> GetActivitiesByEmployeeIdAsync(int employeeId, int limit = 0, int offset = 5) {
        try {
            return await _http
                .GetFromJsonAsync<List<ActivityViewDto>>($"api/activity/get/activity/per/employee/{employeeId}/{limit}/{offset}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<int> GetWorkTimesSizeByEmployeeIdAsync(int employeeId) {
        try {
            return await _http
                .GetFromJsonAsync<int>($"api/worktimerecord/get/size/by/employee/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return default;
        }
    }

    public async Task<List<WorkTimeRecordViewDto>> GetAllWorkTimeRecordsByEmployeeId(int employeeId) {
        try {
            return await _http
                .GetFromJsonAsync<List<WorkTimeRecordViewDto>>($"api/worktimerecord/get/by/employee/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
    
    public async Task<WorkTimeRecordViewDto> GetWorkTimeRecords(int workOrderId, int activityId, int employeeId) {
        try {
            return await _http
                .GetFromJsonAsync<WorkTimeRecordViewDto>($"api/worktimerecord/get/{workOrderId}/{activityId}/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
    
    public async Task<int> GetCollectionSizeByEmployeeId(int employeeId) {
        try {
            return await _http
                .GetFromJsonAsync<int>($"api/activity/get/size/by/employee/id/{employeeId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
}
