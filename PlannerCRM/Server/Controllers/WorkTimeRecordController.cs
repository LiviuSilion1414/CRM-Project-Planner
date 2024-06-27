namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkTimeRecordController : ControllerBase
{
    private readonly WorkTimeRecordRepository _repo;
    private readonly ILogger<WorkTimeRecordRepository> _logger;

    public WorkTimeRecordController(
        WorkTimeRecordRepository repo, 
        Logger<WorkTimeRecordRepository> logger) 
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddWorkTimeRecordAsync(WorkTimeRecordFormDto dto) {
        await _repo.AddAsync(dto);
        return Ok();
    }
    
    [Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
    [Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditWorkTimeRecordAsync(WorkTimeRecordFormDto dto) {
        await _repo.EditAsync(dto);
        return Ok();
    }
    
    [HttpGet("get/{workOrderId}/{activityId}/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetWorkTimeRecordAsync(int workOrderId, int activityId, string employeeId) {
        return await _repo.GetAsync(workOrderId, activityId, employeeId);
    }
    
    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkTimeRecordViewDto>> GetPaginatedWorkTimeRecordsAsync(int limit, int offset) {
        return await _repo.GetPaginatedWorkTimeRecordsAsync(limit, offset);
    }
    
    [HttpGet("get/by/employee/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetAllWorkTimeRecordsByEmployeeIdAsync(string employeeId) {
        return await _repo.GetAllWorkTimeRecordsByEmployeeIdAsync(employeeId);
    }
}
