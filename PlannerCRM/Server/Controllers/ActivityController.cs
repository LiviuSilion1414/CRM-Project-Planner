namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityController(IRepository<Activity, ActivityDto> genericRepo, ActivityRepository specificRepo) 
    : CrudController<Activity, ActivityDto>(genericRepo)
{
    private readonly ActivityRepository _specificRepo = specificRepo;

    [HttpGet]
    [Route("searchByTitle/{activityTitle}")]
    public async Task<ICollection<ActivityDto>> SearchActivityByTitle(string activityTitle)
    {
        return await _specificRepo.SearchActivityByTitle(activityTitle);
    }

    [HttpGet]
    [Route("findAssociatedEmployeesByActivityId/{activityId}")]
    public async Task<ICollection<EmployeeDto>> FindAssociatedEmployeesWithinActivity(int activityId)
    {
        return await _specificRepo.FindAssociatedEmployeesWithinActivity(activityId);
    }

    [HttpGet]
    [Route("findAssociatedWorkOrdersByActivityId/{activityId}")]
    public async Task<WorkOrderDto> FindAssociatedWorkOrderByActivityId(int activityId)
    {
        return await _specificRepo.FindAssociatedWorkOrderByActivityId(activityId);
    }

    [HttpGet]
    [Route("findAssociatedWorkTimesWithinActivity/{activityId}")]
    public async Task<ICollection<WorkTimeDto>> FindAssociatedWorkTimesWithinActivity(int activityId)
    {
        return await _specificRepo.FindAssociatedWorkTimesWithinActivity(activityId);
    }
}