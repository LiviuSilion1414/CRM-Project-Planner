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
    [Route("searchEmployeeByName/{employeeName:string}")]
    public async Task<EmployeeDto> SearchEmployeeByName(string employeeName)
    {
        return await _specificRepo.SearchEmployeeByName(employeeName);
    }

    [HttpGet]
    [Route("findAssociatedActivitiesByEmployeeId/{employeeId:int}")]
    public async Task<ICollection<ActivityDto>> FindAssociatedActivitiesByEmployeeId(int employeeId)
    {
        return await _specificRepo.FindAssociatedActivitiesByEmployeeId(employeeId);
    }

    [HttpGet]
    [Route("findAssociatedWorkTimesByActivityIdAndEmployeeId/{employeeId:int}/{activityId:int}")]
    public async Task<ICollection<WorkTimeDto>> FindAssociatedWorkTimesByActivityIdAndEmployeeId(int employeeId, int activityId)
    {
        return await _specificRepo.FindAssociatedWorkTimesByActivityIdAndEmployeeId(employeeId, activityId);
    }

    [HttpGet]
    [Route("findAssociatedSalaryDataByEmployeeId/{employeeId:int}")]
    public async Task<ICollection<SalaryDto>> FindAssociatedSalaryDataByEmployeeId(int employeeId)
    {
        return await _specificRepo.FindAssociatedSalaryDataByEmployeeId(employeeId);
    }
}