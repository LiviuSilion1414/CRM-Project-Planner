using PlannerCRM.Server.Repositories;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(EmployeeRepository repo) : ControllerBase
{
    private readonly EmployeeRepository _repo = repo;

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(EmployeeDto employee)
    {
        await _repo.AddAsync(employee);

        return Ok();
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit(EmployeeDto employee)
    {
        await _repo.EditAsync(employee);

        return Ok();
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(EmployeeDto employee)
    {
        await _repo.DeleteAsync(employee);

        return Ok();
    }

    [HttpGet]
    [Route("getById/{employeeId}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int employeeId)
    {
        var employee = await _repo.GetByIdAsync(employeeId);
        return Ok(employee);
    }

    [HttpGet]
    [Route("getWithPagination/{limit}/{offset}")]
    public async Task<ActionResult<List<EmployeeDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route("searchEmployeeByName/{employeeName}")]
    public async Task<List<EmployeeDto>> SearchEmployeeByName(string employeeName)
    {
        return await _repo.SearchEmployeeByName(employeeName);
    }

    [HttpGet]
    [Route("findAssociatedActivitiesByEmployeeId/{employeeId}")]
    public async Task<List<ActivityDto>> FindAssociatedActivitiesByEmployeeId(int employeeId)
    {
        return await _repo.FindAssociatedActivitiesByEmployeeId(employeeId);
    }

    [HttpGet]
    [Route("findAssociatedWorkTimesByActivityIdAndEmployeeId/{employeeId}/{activityId}")]
    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesByActivityIdAndEmployeeId(int employeeId, int activityId)
    {
        return await _repo.FindAssociatedWorkTimesByActivityIdAndEmployeeId(employeeId, activityId);
    }

    [HttpGet]
    [Route("findAssociatedSalaryDataByEmployeeId/{employeeId}")]
    public async Task<List<SalaryDto>> FindAssociatedSalaryDataByEmployeeId(int employeeId)
    {
        return await _repo.FindAssociatedSalaryDataByEmployeeId(employeeId);
    }
}