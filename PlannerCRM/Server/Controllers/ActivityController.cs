namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route(ApiUrl.ACTIVITY_CONTROLLER)]
public class ActivityController(ActivityRepository specificRepo) : ControllerBase
{
    private readonly ActivityRepository _repo = specificRepo;

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_INSERT)]
    public async Task<ResultDto> Insert(ActivityDto dto)
    {
        try
        {
            return await _repo.Insert(dto);
        }
        catch
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [HttpPut]
    [Route(ApiUrl.ACTIVITY_UPDATE)]
    public async Task<ResultDto> Update(ActivityDto dto)
    {
        try
        {
            return await _repo.Update(dto);
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_DELETE)]
    public async Task<ResultDto> Delete([FromBody] ActivityFilterDto filter)
    {
        try
        {
            await _repo.Delete(filter);
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_GET)]
    public async Task<ResultDto> GetById([FromBody] ActivityFilterDto filter)
    {
        try
        {
            var activity = await _repo.Get(filter);
            return new ResultDto()
            {
                id = null,
                data = activity,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_LIST)]
    public async Task<ResultDto> List([FromBody] ActivityFilterDto filter)
    {
        try
        {
            var entities = await _repo.List(filter);
            return new ResultDto()
            {
                id = null,
                data = entities,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_SEARCH)]
    public async Task<ResultDto> SearchActivityByTitle([FromBody] ActivityFilterDto filter)
    {
        try
        {
            var activities = await _repo.Search(filter);
            return new ResultDto()
            {
                id = null,
                data = activities,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID)]
    public async Task<ResultDto> FindAssociatedEmployeesWithinActivity([FromBody] ActivityFilterDto filter)
    {
        try
        {
            var employees = await _repo.FindAssociatedEmployeesWithinActivity(filter);
            return new ResultDto()
            {
                id = null,
                data = employees,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID)]
    public async Task<ResultDto> FindAssociatedWorkOrderByActivityId([FromBody] ActivityFilterDto filter)
    {
        try
        {
            var workOrder = await _repo.FindAssociatedWorkOrderByActivityId(filter);
            return new ResultDto()
            {
                id = null,
                data = workOrder,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_ASSIGN_ACTIVITY)]
    public async Task<ResultDto> AssignActivity([FromBody] ActivityFilterDto filter)
    {
        try
        {
            await _repo.AssignActivityToEmployee(filter);
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_REMOVE_ASSIGNED_EMPLOYEE)]
    public async Task<ResultDto> RemoveAssignedEmployeeFromActivity([FromBody] ActivityFilterDto filter)
    {
        try
        {
            await _repo.RemoveAssignedEmployeeFromActivity(filter);
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }
}
