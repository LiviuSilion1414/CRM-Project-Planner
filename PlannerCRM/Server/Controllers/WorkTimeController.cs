namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkTimeController(WorkTimeRepository repo) : ControllerBase
{ 
    private readonly WorkTimeRepository _repo = repo;

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(WorkTimeDto workTime)
    {
        await _repo.AddAsync(workTime);

        return Ok();
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit(WorkTimeDto workTime)
    {
        await _repo.EditAsync(workTime);

        return Ok();
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(WorkTimeDto workTime)
    {
        await _repo.DeleteAsync(workTime);

        return Ok();
    }

    [HttpGet]
    [Route("getById/{workTimeId}")]
    public async Task<ActionResult<WorkTimeDto>> GetById(Guid workTimeId)
    {
        var workTime = await _repo.GetByIdAsync(workTimeId);
        return Ok(workTime);
    }

    [HttpGet]
    [Route("getWithPagination/{limit}/{offset}")]
    public async Task<ActionResult<List<WorkTimeDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route("searchWorkTimeByEmployeeName/{employeeName}")]
    public async Task<List<WorkTimeDto>> SearchWorkTimeByEmployeeName(string employeeName)
    {
        return await _repo.SearchWorkTimeByEmployeeName(employeeName);
    }
}