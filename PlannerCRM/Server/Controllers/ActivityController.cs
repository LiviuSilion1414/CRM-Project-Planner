namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController : ControllerBase
{
    private readonly ActivityRepository _repo;
    private readonly ILogger<ActivityRepository> _logger;

    public ActivityController(
        ActivityRepository repo,
        Logger<ActivityRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddActivityAsync(ActivityFormDto dto)
    {
        await _repo.AddAsync(dto);

        return Ok(SuccessfulCrudFeedBack.ACTIVITY_ADD);
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditActivityAsync(ActivityFormDto dto)
    {
        await _repo.EditAsync(dto);

        return Ok(SuccessfulCrudFeedBack.ACTIVITY_EDIT);
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{activityId}")]
    public async Task<IActionResult> DeleteActivityAsync(int activityId)
    {
        await _repo.DeleteAsync(activityId);

        return Ok(SuccessfulCrudFeedBack.ACTIVITY_DELETE);
    }

    [Authorize]
    [HttpGet("get/{activityId}")]
    public async Task<ActivityViewDto> GetActivityForViewByIdAsync(int activityId)
    {
        return await _repo.GetForViewByIdAsync(activityId);
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{activityId}")]
    public async Task<ActivityFormDto> GetActivityForEditByIdAsync(int activityId)
    {
        return await _repo.GetForEditByIdAsync(activityId);
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{activityId}")]
    public async Task<ActivityDeleteDto> GetActivityForDelete(int activityId)
    {
        return await _repo.GetForDeleteByIdAsync(activityId);
    }

    [Authorize]
    [HttpGet("get/activity/by/workorder/{workOrderId}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(int workOrderId)
    {
        return await _repo.GetActivitiesPerWorkOrderAsync(workOrderId);
    }

    [Authorize]
    [HttpGet("get/activity/by/employee/{employeeId}/{limit}/{offset}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerEmployeeAsync(string employeeId, int limit, int offset)
    {
        return await _repo.GetActivityByEmployeeId(employeeId, limit, offset);
    }

    [Authorize]
    [HttpGet("get/size/by/employee/id/{employeeId}")]
    public async Task<int> GetCollectionSizeByEmployeeIdAsync(string employeeId)
    {
        return await _repo.GetCollectionSizeByEmployeeIdAsync(employeeId);
    }
}