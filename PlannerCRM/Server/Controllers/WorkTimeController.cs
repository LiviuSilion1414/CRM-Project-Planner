namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkTimeController(IRepository<WorkTime, WorkTimeDto> repo) : CrudController<WorkTime, WorkTimeDto>(repo)
{ }