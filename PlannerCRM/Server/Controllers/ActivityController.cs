using PlannerCRM.Server.Models.Entities;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController(IActivityRepository activityRepository) : CrudController<Activity>
{
    private readonly IActivityRepository _activityRepository = activityRepository;

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

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/view/{activityId}")]
    public async Task<ActivityViewDto> GetActivityForView(int activityId) =>
        await _activityRepository.GetForViewByIdAsync(activityId);

    [Authorize]
    [HttpGet("get/activity/by/workorder/{workOrderId}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(int workOrderId) =>
        await _activityRepository.GetActivitiesPerWorkOrderAsync(workOrderId);

    [Authorize]
    [HttpGet("get/activity/by/employee/{employeeId}/{limit}/{offset}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerEmployeeAsync(int employeeId, int limit, int offset) =>
        await _activityRepository.GetActivityByEmployeeId(employeeId, limit, offset);

    [Authorize]
    [HttpGet("get/size/by/employeeId/{employeeId}")]
    public async Task<int> GetCollectionSizeByEmployeeIdAsync(int employeeId) =>
        await _activityRepository.GetCollectionSizeByEmployeeIdAsync(employeeId);
}