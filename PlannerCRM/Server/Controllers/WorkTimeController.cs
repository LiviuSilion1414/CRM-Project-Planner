namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkTimeController(WorkTimeRepository repo) : ControllerBase
{
    private readonly WorkTimeRepository _repo = repo;

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<IActionResult> Add(WorkTimeDto workTime)
    {
        await _repo.AddAsync(workTime);

        return Ok();
    }

    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<IActionResult> Edit(WorkTimeDto workTime)
    {
        await _repo.EditAsync(workTime);

        return Ok();
    }

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<IActionResult> Delete(WorkTimeDto workTime)
    {
        await _repo.DeleteAsync(workTime);

        return Ok();
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ActionResult<WorkTimeDto>> GetById(Guid workTimeId)
    {
        var workTime = await _repo.GetByIdAsync(workTimeId);
        return Ok(workTime);
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ActionResult<List<WorkTimeDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }
}