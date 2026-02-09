using Humanizer;
using PlannerCRM.Server.Models;
using PlannerCRM.Server.System;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route(ApiUrl.SETTINGS_CONTROLLER)]
public class SettingsController(PlannerCrmContext context, IMapper mapper) : ControllerBase
{
    private readonly SettingRepository _repo = new(context, mapper);
    private readonly SystemLogHelper _systemLog = new(context);

    [HttpPost]
    [Route(ApiUrl.INSERT)]
    public async Task<ResultDto> Insert(SettingDto dto)
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
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.SETTINGS_CONTROLLER + ApiUrl.INSERT, ex, User?.Identity, dto);
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
    public async Task<ResultDto> Update(SettingDto dto)
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
            await _systemLog.WriteLog(ApiUrl.SETTINGS_CONTROLLER + ApiUrl.UPDATE, ex, User?.Identity, dto);
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
    [Route(ApiUrl.DELETE)]
    public async Task<ResultDto> Delete(SettingDto dto)
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
            await _systemLog.WriteLog(ApiUrl.SETTINGS_CONTROLLER + ApiUrl.DELETE, ex, User?.Identity, dto);
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
    public async Task<ResultDto> Get(SettingFilterDto filter)
    {
        try
        {
            var role = await _repo.Get(filter);
            return new ResultDto()
            {
                id = null,
                data = role,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.SETTINGS_CONTROLLER + ApiUrl.GET, ex, User?.Identity, filter);
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
    public async Task<ResultDto> List(SettingFilterDto filter)
    {
        try
        {
            var roles = await _repo.List(filter);
            return new ResultDto()
            {
                id = null,
                data = roles,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.SETTINGS_CONTROLLER + ApiUrl.LIST, ex, User?.Identity, filter);
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
