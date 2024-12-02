namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkTimeController(IRepository<WorkTime, WorkTimeDto> genericRepo, WorkTimeRepository specificRepo) 
    : CrudController<WorkTime, WorkTimeDto>(genericRepo)
{ 
    private readonly WorkTimeRepository _specificRepo = specificRepo;

    [HttpGet]
    [Route("searchWorkTimeByEmployeeName/{employeeName:string}")]
    public async Task<WorkTimeDto> SearchWorkTimeByEmployeeName(string employeeName)
    {
        return await _specificRepo.SearchWorkTimeByEmployeeName(employeeName);
    }
}