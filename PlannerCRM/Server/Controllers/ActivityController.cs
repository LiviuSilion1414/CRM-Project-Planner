namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController(IRepository<Activity, ActivityDto> repo) : CrudController<Activity, ActivityDto>(repo)
{ }