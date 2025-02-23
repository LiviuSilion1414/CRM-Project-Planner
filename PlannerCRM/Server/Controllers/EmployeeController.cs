namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(EmployeeRepository repo) : ControllerBase
{
    private readonly EmployeeRepository _repo = repo;

    [Authorize]
    [HttpPost]
    [Route(EmployeeEndpointActions.ASSIGN_ROLE_PLACEHOLDER)]
    public async Task<IActionResult> AssignRole(EmployeeDto dto, string roleName)
    {
        await _repo.AssignRole(dto, roleName);

        return Ok();
    }

    [Authorize]
    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<IActionResult> Add(EmployeeDto employee)
    {
        await _repo.AddAsync(employee);

        return Ok();
    }

    [Authorize]
    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<IActionResult> Edit(EmployeeDto employee)
    {
        await _repo.EditAsync(employee);

        return Ok();
    }

    [Authorize]
    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<IActionResult> Delete(EmployeeDto employee)
    {
        await _repo.DeleteAsync(employee);

        return Ok();
    }

    [Authorize]
    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ActionResult<EmployeeDto>> GetById(Guid employeeId)
    {
        var employee = await _repo.GetByIdAsync(employeeId);
        return Ok(employee);
    }

    [Authorize]
    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ActionResult<List<EmployeeDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.SEARCH_EMPLOYEE_BY_NAME_PLACEHOLDER)]
    public async Task<List<EmployeeDto>> SearchEmployeeByName(string employeeName)
    {
        return await _repo.SearchEmployeeByName(employeeName);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route(EmployeeEndpointActions.SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY_PLACEHOLDER)]
    public async Task<List<EmployeeLoginRecoveryDto>> SearchEmployeeByNameForRecovery(string name, string email, string phone)
    {
        return await _repo.SearchEmployeeByName(name, email, phone);
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID_PLACEHOLDER)]
    public async Task<List<ActivityDto>> FindAssociatedActivitiesByEmployeeId(Guid employeeId)
    {
        return await _repo.FindAssociatedActivitiesByEmployeeId(employeeId);
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_WORKTIMES_BY_ACTIVITYID_AND_EMPLOYEEID_PLACEHOLDER)]
    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesByActivityIdAndEmployeeId(Guid employeeId, Guid activityId)
    {
        return await _repo.FindAssociatedWorkTimesByActivityIdAndEmployeeId(employeeId, activityId);
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_SALARY_DATA_BY_EMPLOYEEID_PLACEHOLDER)]
    public async Task<List<SalaryDto>> FindAssociatedSalaryDataByEmployeeId(Guid employeeId)
    {
        return await _repo.FindAssociatedSalaryDataByEmployeeId(employeeId);
    }
}