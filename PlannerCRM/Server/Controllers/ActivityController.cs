using PlannerCRM.Server.Repositories;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityController(ActivityRepository specificRepo) : ControllerBase
{
    private readonly ActivityRepository _repo = specificRepo;

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(ActivityDto activity)
    {
        await _repo.AddAsync(activity);

        return Ok();
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit(ActivityDto activity)
    {
        await _repo.EditAsync(activity);

        return Ok();
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(ActivityDto activity)
    {
        await _repo.DeleteAsync(activity);

        return Ok();
    }

    [HttpGet]
    [Route("getById/{activityId}")]
    public async Task<ActionResult<ActivityDto>> GetById(int activityId)
    {
        var activity = await _repo.GetByIdAsync(activityId);
        return Ok(activity);
    }

    [HttpGet]
    [Route("getWithPagination/{limit}/{offset}")]
    public async Task<ActionResult<List<ActivityDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route("searchByTitle/{activityTitle}")]
    public async Task<List<ActivityDto>> SearchActivityByTitle(string activityTitle)
    {
        return await _repo.SearchActivityByTitle(activityTitle);
    }

    [HttpGet]
    [Route("findAssociatedEmployeesByActivityId/{activityId}")]
    public async Task<List<EmployeeDto>> FindAssociatedEmployeesWithinActivity(int activityId)
    {
        return await _repo.FindAssociatedEmployeesWithinActivity(activityId);
    }

    [HttpGet]
    [Route("findAssociatedWorkOrdersByActivityId/{activityId}")]
    public async Task<WorkOrderDto> FindAssociatedWorkOrderByActivityId(int activityId)
    {
        return await _repo.FindAssociatedWorkOrderByActivityId(activityId);
    }

    [HttpGet]
    [Route("findAssociatedWorkTimesWithinActivity/{activityId}")]
    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesWithinActivity(int activityId)
    {
        return await _repo.FindAssociatedWorkTimesWithinActivity(activityId);
    }
}