using PlannerCRM.Server.Models.Entities;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(EmployeeRepository repo) : ControllerBase
{
    private readonly EmployeeRepository _repo = repo;

    [Authorize]
    [HttpPost]
    [Route(EmployeeEndpointActions.ASSIGN_ROLE_PLACEHOLDER)]
    public async Task<ResultDto> AssignRole([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.AssignRole(filter);
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

    [Authorize]
    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<ResultDto> Add([FromBody] SearchFilterDto filter)
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

    [Authorize]
    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<ResultDto> Edit([FromBody] SearchFilterDto filter)
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

    [Authorize]
    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<ResultDto> Delete([FromBody] SearchFilterDto filter)
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

    [Authorize]
    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ResultDto> GetById([FromBody] SearchFilterDto filter)
    {
        try
        {
            var employee = await _repo.GetByIdAsync(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = employee,
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

    [Authorize]
    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ResultDto> GetWithPagination([FromBody] SearchFilterDto filter)
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
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.SEARCH_EMPLOYEE_BY_NAME_PLACEHOLDER)]
    public async Task<ResultDto> SearchEmployeeByName([FromBody] SearchFilterDto filter)
    {
        try
        {
            var employees = await _repo.SearchEmployeeByName(filter);
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
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [AllowAnonymous]
    [HttpGet]
    [Route(EmployeeEndpointActions.SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY_PLACEHOLDER)]
    public async Task<ResultDto> SearchEmployeeByNameForRecovery([FromBody] SearchFilterDto filter)
    {
        try
        {
            var recoveryEmployees = await _repo.SearchEmployeeByName(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = recoveryEmployees,
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

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID_PLACEHOLDER)]
    public async Task<ResultDto> FindAssociatedActivitiesByEmployeeId([FromBody] SearchFilterDto filter)
    {
        try
        {
            var activities = await _repo.FindAssociatedActivitiesByEmployeeId(filter);
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
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_WORKTIMES_BY_ACTIVITYID_AND_EMPLOYEEID_PLACEHOLDER)]
    public async Task<ResultDto> FindAssociatedWorkTimesByActivityIdAndEmployeeId([FromBody] SearchFilterDto filter)
    {
        try
        {
            var workTimes = await _repo.FindAssociatedWorkTimesByActivityIdAndEmployeeId(filter);
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
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_SALARY_DATA_BY_EMPLOYEEID_PLACEHOLDER)]
    public async Task<ResultDto> FindAssociatedSalaryDataByEmployeeId([FromBody] SearchFilterDto filter)
    {
        try
        {
            var salaryData = await _repo.FindAssociatedSalaryDataByEmployeeId(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = salaryData,
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
}
