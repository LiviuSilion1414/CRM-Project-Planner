namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController(IRepository<ActivityFormDto> repo, IActivityRepository activityRepository) : ControllerBase
{
    private readonly IRepository<ActivityFormDto> _repo = repo;
    private readonly IActivityRepository _activityRepository = activityRepository;

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
        await _activityRepository.GetForViewByIdAsync(activityId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{activityId}")]
    public async Task<ActivityFormDto> GetActivityForEditByIdAsync(int activityId) =>
        await _activityRepository.GetForEditByIdAsync(activityId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{activityId}")]
    public async Task<ActivityDeleteDto> GetActivityForDelete(int activityId) =>
        await _activityRepository.GetForDeleteByIdAsync(activityId);

    [Authorize]
    [HttpGet("get/activity/by/workorder/{workOrderId}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(int workOrderId) =>
        await _activityRepository.GetActivitiesPerWorkOrderAsync(workOrderId);

    [Authorize]
    [HttpGet("get/activity/by/employee/{employeeId}/{limit}/{offset}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerEmployeeAsync(int employeeId, int limit, int offset) =>
        await _activityRepository.GetActivityByEmployeeId(employeeId, limit, offset);

    [Authorize]
    [HttpGet("get/size/by/employee/id/{employeeId}")]
    public async Task<int> GetCollectionSizeByEmployeeIdAsync(int employeeId) =>
        await _activityRepository.GetCollectionSizeByEmployeeIdAsync(employeeId);
}