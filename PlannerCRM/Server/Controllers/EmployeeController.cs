namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(EmployeeRepository repo) : ControllerBase
{
    private readonly EmployeeRepository _repo = repo;

    [Authorize]
    [HttpPost]
    [Route(EmployeeEndpointActions.ASSIGN_ROLE_PLACEHOLDER)]
    public async Task<IActionResult> AssignRole([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.AssignRole(filter);
            return Ok();
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<IActionResult> Add([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.AddAsync(filter);
            return Ok();
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<IActionResult> Edit([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.EditAsync(filter);
            return Ok();
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<IActionResult> Delete([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.DeleteAsync(filter);
            return Ok();
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ActionResult<EmployeeDto>> GetById([FromBody] SearchFilterDto filter)
    {
        try
        {
            var employee = await _repo.GetByIdAsync(filter);
            return Ok(employee);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ActionResult<List<EmployeeDto>>> GetWithPagination([FromBody] SearchFilterDto filter)
    {
        try
        {
            var entities = await _repo.GetWithPagination(filter);
            return Ok(entities);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Modifica: utilizzo di SearchFilterDto al posto di string employeeName
    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.SEARCH_EMPLOYEE_BY_NAME_PLACEHOLDER)]
    public async Task<ActionResult<List<EmployeeDto>>> SearchEmployeeByName([FromBody] SearchFilterDto filter)
    {
        try
        {
            var employees = await _repo.SearchEmployeeByName(filter);
            return Ok(employees);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    [Route(EmployeeEndpointActions.SEARCH_EMPLOYEE_BY_NAME_EMAIL_PHONE_FOR_RECOVERY_PLACEHOLDER)]
    public async Task<ActionResult<List<EmployeeLoginRecoveryDto>>> SearchEmployeeByNameForRecovery([FromBody] SearchFilterDto filter)
    {
        try
        {
            var recoveryEmployees = await _repo.SearchEmployeeByName(filter);
            return Ok(recoveryEmployees);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID_PLACEHOLDER)]
    public async Task<ActionResult<List<ActivityDto>>> FindAssociatedActivitiesByEmployeeId([FromBody] SearchFilterDto filter)
    {
        try
        {
            var activities = await _repo.FindAssociatedActivitiesByEmployeeId(filter);
            return Ok(activities);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_WORKTIMES_BY_ACTIVITYID_AND_EMPLOYEEID_PLACEHOLDER)]
    public async Task<ActionResult<List<WorkTimeDto>>> FindAssociatedWorkTimesByActivityIdAndEmployeeId([FromBody] SearchFilterDto filter)
    {
        try
        {
            var workTimes = await _repo.FindAssociatedWorkTimesByActivityIdAndEmployeeId(filter);
            return Ok(workTimes);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_SALARY_DATA_BY_EMPLOYEEID_PLACEHOLDER)]
    public async Task<ActionResult<List<SalaryDto>>> FindAssociatedSalaryDataByEmployeeId([FromBody] SearchFilterDto filter)
    {
        try
        {
            var salaryData = await _repo.FindAssociatedSalaryDataByEmployeeId(filter);
            return Ok(salaryData);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
