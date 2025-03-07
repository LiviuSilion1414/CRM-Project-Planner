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
            await _repo.Insert(dto);
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
        catch 
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
    [Route(ApiUrl.ACTIVITY_UPDATE)]
    public async Task<ResultDto> Update(ActivityDto dto)
    {
        try
        {
            await _repo.Update(dto);
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
        catch 
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
    [Route(ApiUrl.ACTIVITY_DELETE)]
    public async Task<ResultDto> Delete([FromBody] ActivityFilterDto filter)
    {
        try
        {
            await _repo.Delete(filter);
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
        catch 
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
    [Route(ApiUrl.ACTIVITY_GET)]
    public async Task<ResultDto> GetById([FromBody] ActivityFilterDto filter)
    {
        try
        {
            var activity = await _repo.Get(filter);
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
        catch 
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

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_LIST)]
    public async Task<ResultDto> List([FromBody] ActivityFilterDto filter)
    {
        try
        {
            var entities = await _repo.List(filter);
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
        catch 
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

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_SEARCH)]
    public async Task<ResultDto> SearchActivityByTitle([FromBody] ActivityFilterDto filter)
    {
        try
        {
            var activities = await _repo.Search(filter);
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
        catch 
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

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_FIND_ASSOCIATED_EMPLOYEES_BY_ACTIVITYID)]
    public async Task<ResultDto> FindAssociatedEmployeesWithinActivity([FromBody] ActivityFilterDto filter)
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
        catch 
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

    [HttpPost]
    [Route(ApiUrl.ACTIVITY_FIND_ASSOCIATED_WORKORDERS_BY_ACTIVITYID)]
    public async Task<ResultDto> FindAssociatedWorkOrderByActivityId([FromBody] ActivityFilterDto filter)
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
        catch 
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
