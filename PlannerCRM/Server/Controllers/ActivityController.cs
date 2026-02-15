using Humanizer;
using PlannerCRM.Server.Models;
using PlannerCRM.Server.System;
using System.Security.Claims;
using System.Text.Json;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route(ApiUrl.ACTIVITY_CONTROLLER)]
public class ActivityController(PlannerCrmContext context, IMapper mapper) : ControllerBase
{
    private readonly ActivityRepository _repo = new(context, mapper);
    private readonly SystemLogHelper _systemLog = new(context);

    [HttpPost]
    [Route(ApiUrl.INSERT)]
    public async Task<ResultDto> Insert(ActivityDto dto)
    {
        try
        {
            return await _repo.Insert(dto);
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.ACTIVITY_CONTROLLER + ApiUrl.INSERT, ex, User?.Identity, dto);
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
    [Route(ApiUrl.UPDATE)]
    public async Task<ResultDto> Update(ActivityDto dto)
    {
        try
        {
            return await _repo.Update(dto);
        } catch(Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.ACTIVITY_CONTROLLER + ApiUrl.UPDATE, ex, User?.Identity, dto);
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
    [Route(ApiUrl.DELETE)]
    public async Task<ResultDto> Delete([FromBody] ActivityDto dto)
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
            await _systemLog.WriteLog(ApiUrl.ACTIVITY_CONTROLLER + ApiUrl.DELETE_MULTIPLE, ex, User?.Identity, dto);
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
    [Route(ApiUrl.DELETE_MULTIPLE)]
    public async Task<ResultDto> DeleteMultiple([FromBody] List<Guid?> idList)
    {
        try
        {
            await _repo.DeleteMultiple(idList);
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
            await _systemLog.WriteLog(ApiUrl.ACTIVITY_CONTROLLER + ApiUrl.DELETE_MULTIPLE, ex, User?.Identity, idList);
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
    [Route(ApiUrl.GET)]
    public async Task<ResultDto> Get([FromBody] ActivityFilterDto filter)
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
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.ACTIVITY_CONTROLLER + ApiUrl.GET, ex, User?.Identity, filter);
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
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.ACTIVITY_CONTROLLER + ApiUrl.LIST, ex, User?.Identity, filter);
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
