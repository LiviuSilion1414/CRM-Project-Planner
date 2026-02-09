using PlannerCRM.Server.Models;
using PlannerCRM.Server.System;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route(ApiUrl.WORKTIME_CONTROLLER)]
public class WorkTimeController(PlannerCrmContext context, IMapper mapper) : ControllerBase
{
    private readonly WorkTimeRepository _repo = new(context, mapper);
    private readonly SystemLogHelper _systemLog = new(context);

    [HttpPost]
    [Route(ApiUrl.INSERT)]
    public async Task<ResultDto> Insert([FromBody] WorkTimeDto dto)
    {
        try
        {
            return await _repo.Insert(dto);
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.WORKTIME_CONTROLLER + ApiUrl.INSERT, ex, User?.Identity, dto);
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
    [Route(ApiUrl.UPDATE)]
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
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.WORKTIME_CONTROLLER + ApiUrl.UPDATE, ex, User?.Identity, dto);
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

    [HttpPost]
    [Route(ApiUrl.DELETE)]
    public async Task<ResultDto> Delete([FromBody] WorkTimeDto dto)
    {
        try
        {
            await _repo.Delete(dto);
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.WORKTIME_CONTROLLER + ApiUrl.DELETE, ex, User?.Identity, dto);
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
    [Route(ApiUrl.GET)]
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
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.WORKTIME_CONTROLLER + ApiUrl.GET, ex, User?.Identity, filter);
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
    [Route(ApiUrl.LIST)]
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

        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.WORKTIME_CONTROLLER + ApiUrl.LIST, ex, User?.Identity, filter);
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
