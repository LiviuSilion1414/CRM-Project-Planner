namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route(ApiUrl.WORKTIME_CONTROLLER)]
public class WorkTimeController(WorkTimeRepository repo) : ControllerBase
{
    private readonly WorkTimeRepository _repo = repo;

    [HttpPost]
    [Route(ApiUrl.WORKTIME_INSERT)]
    public async Task<ResultDto> Insert([FromBody] WorkTimeDto dto)
    {
        try
        {
            await _repo.Insert(dto);
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

    [HttpPut]
    [Route(ApiUrl.WORKTIME_UPDATE)]
    public async Task<ResultDto> Update(WorkTimeDto dto)
    {
        try
        {
            await _repo.Update(dto);
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
    [Route(ApiUrl.WORKTIME_DELETE)]
    public async Task<ResultDto> Delete([FromBody] WorkTimeFilterDto filter)
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
    [Route(ApiUrl.WORKTIME_GET)]
    public async Task<ResultDto> Get([FromBody] WorkTimeFilterDto filter)
    {
        try
        {
            var workTime = await _repo.Get(filter);
            return new ResultDto()
            {
                id = null,
                data = workTime,
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
    [Route(ApiUrl.WORKTIME_LIST)]
    public async Task<ResultDto> List([FromBody] WorkTimeFilterDto filter)
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
    [Route(ApiUrl.WORKTIME_FIND_ASSOCIATED_WORKTIMES_BY_EMPLOYEE_ID)]
    public async Task<ResultDto> FindAssociatedActivitiesByWorkTimeId([FromBody] WorkTimeFilterDto filter)
    {
        try
        {
            var workTimes = await _repo.FindAssociatedWorktimesByEmployeeId(filter);
            return new ResultDto()
            {
                id = null,
                data = workTimes,
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