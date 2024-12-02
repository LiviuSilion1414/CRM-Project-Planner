using Microsoft.EntityFrameworkCore;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController(IRepository<Employee, EmployeeDto> genericRepo, EmployeeRepository specificRepo) 
    : CrudController<Employee, EmployeeDto>(genericRepo)
{
    private readonly EmployeeRepository _specificRepo = specificRepo;

    [HttpGet]
    [Route("searchEmployeeByName/{employeeName}")]
    public async Task<EmployeeDto> SearchEmployeeByName(string employeeName)
    {
        return await _specificRepo.SearchEmployeeByName(employeeName);
    }

    [HttpGet]
    [Route("findAssociatedActivitiesByEmployeeId/{employeeId}")]
    public async Task<ICollection<ActivityDto>> FindAssociatedActivitiesByEmployeeId(int employeeId)
    {
        return await _specificRepo.FindAssociatedActivitiesByEmployeeId(employeeId);
    }

    [HttpGet]
    [Route("findAssociatedWorkTimesByActivityIdAndEmployeeId/{employeeId}/{activityId}")]
    public async Task<ICollection<WorkTimeDto>> FindAssociatedWorkTimesByActivityIdAndEmployeeId(int employeeId, int activityId)
    {
        return await _specificRepo.FindAssociatedWorkTimesByActivityIdAndEmployeeId(employeeId, activityId);
    }

    [HttpGet]
    [Route("findAssociatedSalaryDataByEmployeeId/{employeeId}")]
    public async Task<ICollection<SalaryDto>> FindAssociatedSalaryDataByEmployeeId(int employeeId)
    {
        return await _specificRepo.FindAssociatedSalaryDataByEmployeeId(employeeId);
    }
}