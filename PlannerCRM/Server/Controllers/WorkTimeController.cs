namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkTimeController(WorkTimeRepository repo) : ControllerBase
{
    private readonly WorkTimeRepository _repo = repo;

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<IActionResult> Add([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.AddAsync(filter);
            return Ok();
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<IActionResult> Edit([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.EditAsync(filter);
            return Ok();
        }
        catch (Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<IActionResult> Delete([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.DeleteAsync(filter);
            return Ok();
        } 
        catch (Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ActionResult<WorkTimeDto>> GetById([FromBody] SearchFilterDto filter)
    {
        try
        {
            var workTime = await _repo.GetByIdAsync(filter);
            return Ok(workTime);
        }
        catch (Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ActionResult<List<WorkTimeDto>>> GetWithPagination([FromBody] SearchFilterDto filter)
    {
        try
        {
            var entities = await _repo.GetWithPagination(filter);
            return Ok(entities);
        }
        catch (Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }
}