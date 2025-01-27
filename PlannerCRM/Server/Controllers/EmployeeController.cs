using PlannerCRM.Server.Repositories;
using PlannerCRM.Shared.Constants.ApiEndpoints.Routes;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(EmployeeRepository repo) : ControllerBase
{
    private readonly EmployeeRepository _repo = repo;

    [HttpPost]
    [Route(EndpointCrudActions.ADD)]
    public async Task<IActionResult> Add(EmployeeDto employee)
    {
        await _repo.AddAsync(employee);

        return Ok();
    }

    [HttpPut]
    [Route(EndpointCrudActions.EDIT)]
    public async Task<IActionResult> Edit(EmployeeDto employee)
    {
        await _repo.EditAsync(employee);

        return Ok();
    }

    [HttpPost]
    [Route(EndpointCrudActions.DELETE)]
    public async Task<IActionResult> Delete(EmployeeDto employee)
    {
        await _repo.DeleteAsync(employee);

        return Ok();
    }

    [HttpGet]
    [Route(EndpointCrudActions.GET_BY_ID)]
    public async Task<ActionResult<EmployeeDto>> GetById(int employeeId)
    {
        var employee = await _repo.GetByIdAsync(employeeId);
        return Ok(employee);
    }

    [HttpGet]
    [Route(EndpointCrudActions.GET_WITH_PAGINATION)]
    public async Task<ActionResult<List<EmployeeDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route(EmployeeEndpointActions.SEARCH_EMPLOYEE_BY_NAME)]
    public async Task<List<EmployeeDto>> SearchEmployeeByName(string employeeName)
    {
        return await _repo.SearchEmployeeByName(employeeName);
    }

    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_ACTIVITIES_BY_EMPLOYEEID)]
    public async Task<List<ActivityDto>> FindAssociatedActivitiesByEmployeeId(int employeeId)
    {
        return await _repo.FindAssociatedActivitiesByEmployeeId(employeeId);
    }

    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_WORKTIMES_BY_ACTIVITYID_AND_EMPLOYEEID)]
    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesByActivityIdAndEmployeeId(int employeeId, int activityId)
    {
        return await _repo.FindAssociatedWorkTimesByActivityIdAndEmployeeId(employeeId, activityId);
    }

    [HttpGet]
    [Route(EmployeeEndpointActions.FIND_ASSOCIATED_SALARY_DATA_BY_EMPLOYEEID)]
    public async Task<List<SalaryDto>> FindAssociatedSalaryDataByEmployeeId(int employeeId)
    {
        return await _repo.FindAssociatedSalaryDataByEmployeeId(employeeId);
    }
}