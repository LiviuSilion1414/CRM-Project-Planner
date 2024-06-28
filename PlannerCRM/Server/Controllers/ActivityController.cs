namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController(
    ActivityRepository repo,
    Logger<ActivityRepository> logger) : ControllerBase
{
    private readonly ActivityRepository _repo = repo;
    private readonly ILogger<ActivityRepository> _logger = logger;

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddActivityAsync(ActivityFormDto dto) {
        await _repo.AddAsync(dto);
        return Ok(SuccessfulCrudFeedBack.ACTIVITY_ADD);
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditActivityAsync(ActivityFormDto dto) {
        await _repo.EditAsync(dto);
        return Ok(SuccessfulCrudFeedBack.ACTIVITY_EDIT);
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{activityId}")]
    public async Task<IActionResult> DeleteActivityAsync(int activityId) {
        await _repo.DeleteAsync(activityId);
        return Ok(SuccessfulCrudFeedBack.ACTIVITY_DELETE);
    }

    [Authorize]
    [HttpGet("get/{activityId}")]
    public async Task<ActivityViewDto> GetActivityForViewByIdAsync(int activityId) =>
        await _repo.GetForViewByIdAsync(activityId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{activityId}")]
    public async Task<ActivityFormDto> GetActivityForEditByIdAsync(int activityId) =>
        await _repo.GetForEditByIdAsync(activityId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{activityId}")]
    public async Task<ActivityDeleteDto> GetActivityForDelete(int activityId) =>
        await _repo.GetForDeleteByIdAsync(activityId);

    [Authorize]
    [HttpGet("get/activity/by/workorder/{workOrderId}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(int workOrderId) =>
        await _repo.GetActivitiesPerWorkOrderAsync(workOrderId);

    [Authorize]
    [HttpGet("get/activity/by/employee/{employeeId}/{limit}/{offset}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerEmployeeAsync(string employeeId, int limit, int offset) =>
        await _repo.GetActivityByEmployeeId(employeeId, limit, offset);

    [Authorize]
    [HttpGet("get/size/by/employee/id/{employeeId}")]
    public async Task<int> GetCollectionSizeByEmployeeIdAsync(string employeeId) =>
        await _repo.GetCollectionSizeByEmployeeIdAsync(employeeId);
}