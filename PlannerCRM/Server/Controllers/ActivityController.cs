namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController(ActivityRepository specificRepo) : ControllerBase
{
    private readonly ActivityRepository _repo = specificRepo;

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<IActionResult> Add([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.AddAsync(filter);
            return Ok();
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<IActionResult> Edit([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.EditAsync(filter);
            return Ok();
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<IActionResult> Delete([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.DeleteAsync(filter);
            return Ok();
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ActionResult<ActivityDto>> GetById([FromBody] SearchFilterDto filter)
    {
        try
        {
            var activity = await _repo.GetByIdAsync(filter);
            return Ok(activity);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ActionResult<List<ActivityDto>>> GetWithPagination([FromBody] SearchFilterDto filter)
    {
        try
        {
            var entities = await _repo.GetWithPagination(filter);
            return Ok(entities);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(ActivityEndpointActions.SEARCH_BY_TITLE_PLACEHOLDER)]
    public async Task<List<ActivityDto>> SearchActivityByTitle([FromBody] SearchFilterDto filter)
    {
        try
        {
            return await _repo.SearchActivityByTitle(filter);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(ActivityEndpointActions.FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID_PLACEHOLDER)]
    public async Task<List<EmployeeDto>> FindAssociatedEmployeesWithinActivity([FromBody] SearchFilterDto filter)
    {
        try
        {
            return await _repo.FindAssociatedEmployeesWithinActivity(filter);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(ActivityEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID_PLACEHOLDER)]
    public async Task<WorkOrderDto> FindAssociatedWorkOrderByActivityId([FromBody] SearchFilterDto filter)
    {
        try
        {
            return await _repo.FindAssociatedWorkOrderByActivityId(filter);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(ActivityEndpointActions.FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY_PLACEHOLDER)]
    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesWithinActivity([FromBody] SearchFilterDto filter)
    {
        try
        {
            return await _repo.FindAssociatedWorkTimesWithinActivity(filter);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
