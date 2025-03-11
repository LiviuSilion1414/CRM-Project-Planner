using PlannerCRM.Server.Models.Entities;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route(ApiUrl.EMPLOYEE_CONTROLLER)]
public class EmployeeController(EmployeeRepository repo) : ControllerBase
{
    private readonly EmployeeRepository _repo = repo;

    [Authorize]
    [HttpPost]
    [Route(ApiUrl.EMPLOYEE_INSERT)]
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

    [Authorize]
    [HttpPut]
    [Route(ApiUrl.EMPLOYEE_UPDATE)]
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

    [Authorize]
    [HttpPut]
    [Route(ApiUrl.EMPLOYEE_UPDATE_ROLE)]
    public async Task<ResultDto> UpdateRole(EmployeeFilterDto filter)
    {
        try
        {
            await _repo.UpdateEmployeeRole(filter);
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
                statusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [Authorize]
    [HttpPost]
    [Route(ApiUrl.EMPLOYEE_DELETE)]
    public async Task<ResultDto> Delete([FromBody] EmployeeFilterDto filter)
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

    [Authorize]
    [HttpPost]
    [Route(ApiUrl.EMPLOYEE_GET)]
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

    [Authorize]
    [HttpPost]
    [Route(ApiUrl.EMPLOYEE_LIST)]
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

    [Authorize]
    [HttpPost]
    [Route(ApiUrl.EMPLOYEE_SEARCH)]
    public async Task<ResultDto> Search([FromBody] EmployeeFilterDto filter)
    {
        try
        {
            var employees = await _repo.Search(filter);
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
                statusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [Route(ApiUrl.EMPLOYEE_SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY)]
    public async Task<ResultDto> SearchEmployeeByNameForRecovery([FromBody] EmployeeFilterDto filter)
    {
        try
        {
            var recoveryEmployees = await _repo.Search(filter);
            return new ResultDto()
            {
                id = null,
                data = recoveryEmployees,
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

    [Authorize]
    [HttpPost]
    [Route(ApiUrl.EMPLOYEE_FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID)]
    public async Task<ResultDto> FindAssociatedActivitiesByEmployeeId([FromBody] EmployeeFilterDto filter)
    {
        try
        {
            var activities = await _repo.FindAssociatedActivitiesByEmployeeId(filter);
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
                statusCode = HttpStatusCode.BadRequest
            };
        }
    }
}
