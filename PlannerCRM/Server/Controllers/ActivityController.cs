namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController(ActivityRepository specificRepo) : ControllerBase
{
    private readonly ActivityRepository _repo = specificRepo;

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<ResultDto> Add([FromBody] FilterDto filter)
    {
        try
        {
            await _repo.AddAsync(filter);
            return new ResultDto() 
            { 
                Guid = null, 
                Data = null, 
                HasCompleted = true, 
                Message = "Operation completed", 
                MessageType = MessageType.Success, 
                StatusCode = HttpStatusCode.OK 
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<ResultDto> Edit([FromBody] FilterDto filter)
    {
        try
        {
            await _repo.EditAsync(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<ResultDto> Delete([FromBody] FilterDto filter)
    {
        try
        {
            await _repo.DeleteAsync(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ResultDto> GetById([FromBody] FilterDto filter)
    {
        try
        {
            var activity = await _repo.GetByIdAsync(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = activity,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ResultDto> GetWithPagination([FromBody] FilterDto filter)
    {
        try
        {
            var entities = await _repo.GetWithPagination(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = entities,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpGet]
    [Route(ActivityEndpointActions.SEARCH_BY_TITLE_PLACEHOLDER)]
    public async Task<ResultDto> SearchActivityByTitle([FromBody] FilterDto filter)
    {
        try
        {
            var activities = await _repo.SearchActivityByTitle(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = activities,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpGet]
    [Route(ActivityEndpointActions.FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID_PLACEHOLDER)]
    public async Task<ResultDto> FindAssociatedEmployeesWithinActivity([FromBody] FilterDto filter)
    {
        try
        {
            var employees = await _repo.FindAssociatedEmployeesWithinActivity(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = employees,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpGet]
    [Route(ActivityEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID_PLACEHOLDER)]
    public async Task<ResultDto> FindAssociatedWorkOrderByActivityId([FromBody] FilterDto filter)
    {
        try
        {
            var workOrder = await _repo.FindAssociatedWorkOrderByActivityId(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = workOrder,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpGet]
    [Route(ActivityEndpointActions.FIND_ASSOCIATED_WORKTIMES_WITHIN_ACTIVITY_PLACEHOLDER)]
    public async Task<ResultDto> FindAssociatedWorkTimesWithinActivity([FromBody] FilterDto filter)
    {
        try
        {
            var workTimes = await _repo.FindAssociatedWorkTimesWithinActivity(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = workTimes,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }
}
