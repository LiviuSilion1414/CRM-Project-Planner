namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkTimeController(IWorkTimeRepository workTimeRecordRepository) 
    : ControllerBase
{
    private readonly IWorkTimeRepository _workTimeRecordRepository = workTimeRecordRepository;

    [HttpGet("get/{workOrderId}/{activityId}/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetWorkTimeRecordAsync(int workOrderId, int activityId, int employeeId) =>
        await _workTimeRecordRepository.GetForViewByIdAsync(workOrderId, activityId, employeeId);

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkTimeRecordViewDto>> GetPaginatedWorkTimeRecordsAsync(int limit, int offset) =>
        await _workTimeRecordRepository.GetPaginatedWorkTimeRecordsAsync(limit, offset);

    [HttpGet("get/by/employee/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetAllWorkTimeRecordsByEmployeeIdAsync(int employeeId) =>
        await _workTimeRecordRepository.GetAllWorkTimeRecordsByEmployeeIdAsync(employeeId);
}
