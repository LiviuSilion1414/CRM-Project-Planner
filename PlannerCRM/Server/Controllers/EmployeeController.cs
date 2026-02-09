using Humanizer;
using PlannerCRM.Server.Models;
using PlannerCRM.Server.System;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route(ApiUrl.EMPLOYEE_CONTROLLER)]
public class EmployeeController(PlannerCrmContext context, IMapper mapper) : ControllerBase
{
    private readonly EmployeeRepository _repo = new(context, mapper);
    private readonly SystemLogHelper _systemLog = new(context);

    [HttpPost]
    [Route(ApiUrl.INSERT)]
    public async Task<ResultDto> Insert(EmployeeDto dto)
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
            await _systemLog.WriteLog(ApiUrl.EMPLOYEE_CONTROLLER + ApiUrl.INSERT, ex, User?.Identity, dto);
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
    public async Task<ResultDto> Update(EmployeeDto dto)
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
            await _systemLog.WriteLog(ApiUrl.EMPLOYEE_CONTROLLER + ApiUrl.UPDATE, ex, User?.Identity, dto);
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
    public async Task<ResultDto> Delete([FromBody] EmployeeDto dto)
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
        } catch(Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.EMPLOYEE_CONTROLLER + ApiUrl.DELETE, ex, User?.Identity, dto);
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
    public async Task<ResultDto> Get([FromBody] EmployeeFilterDto filter)
    {
        try
        {
            var employee = await _repo.Get(filter);
            return new ResultDto()
            {
                id = null,
                data = employee,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.EMPLOYEE_CONTROLLER + ApiUrl.GET, ex, User?.Identity, filter);
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
    [Route(ApiUrl.LIST)]
    public async Task<ResultDto> List([FromBody] EmployeeFilterDto filter)
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
            await _systemLog.WriteLog(ApiUrl.EMPLOYEE_CONTROLLER + ApiUrl.LIST, ex, User?.Identity, filter);
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
}
