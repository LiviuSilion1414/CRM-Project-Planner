namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController(ActivityRepository specificRepo) : ControllerBase
{
    private readonly ActivityRepository _repo = specificRepo;

    [HttpPost]
    [Route(EndpointsCrudActions.ADD)]
    public async Task<IActionResult> Add(ActivityDto activity)
    {
        await _repo.AddAsync(activity);

        return Ok();
    }

    [HttpPut]
    [Route(EndpointsCrudActions.EDIT)]
    public async Task<IActionResult> Edit(ActivityDto activity)
    {
        await _repo.EditAsync(activity);

        return Ok();
    }

    [HttpPost]
    [Route(EndpointsCrudActions.DELETE)]
    public async Task<IActionResult> Delete(ActivityDto activity)
    {
        await _repo.DeleteAsync(activity);

        return Ok();
    }

    [HttpGet]
    [Route(EndpointsCrudActions.GET_BY_ID)]
    public async Task<ActionResult<ActivityDto>> GetById(Guid activityId)
    {
        var activity = await _repo.GetByIdAsync(activityId);
        return Ok(activity);
    }

    [HttpGet]
    [Route(EndpointsCrudActions.GET_WITH_PAGINATION)]
    public async Task<ActionResult<List<ActivityDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route(ActivityEndpointActions.SEARCH_BY_TITLE)]
    public async Task<List<ActivityDto>> SearchActivityByTitle(string title)
    {
        return await _repo.SearchActivityByTitle(title);
    }

    [HttpGet]
    [Route(ActivityEndpointActions.FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID)]
    public async Task<List<EmployeeDto>> FindAssociatedEmployeesWithinActivity(Guid itemId)
    {
        return await _repo.FindAssociatedEmployeesWithinActivity(itemId);
    }

    [HttpGet]
    [Route(ActivityEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID)]
    public async Task<WorkOrderDto> FindAssociatedWorkOrderByActivityId(Guid itemId)
    {
        return await _repo.FindAssociatedWorkOrderByActivityId(itemId);
    }

    [HttpGet]
    [Route(ActivityEndpointActions.FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY)]
    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesWithinActivity(Guid itemId)
    {
        return await _repo.FindAssociatedWorkTimesWithinActivity(itemId);
    }
}