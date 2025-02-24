namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkOrderController(WorkOrderRepository repo) : ControllerBase
{ 
    private readonly WorkOrderRepository _repo = repo;

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
    public async Task<ActionResult<WorkOrderDto>> GetById([FromBody] SearchFilterDto filter)
    {
        try
        {
            var workOrder = await _repo.GetByIdAsync(filter);
            return Ok(workOrder);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ActionResult<List<WorkOrderDto>>> GetWithPagination([FromBody] SearchFilterDto filter)
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

    [HttpGet]
    [Route(WorkOrderEndpointActions.SEARCH_WORKORDER_BY_TITLE_PLACEHOLDER)]
    public async Task<ActionResult<List<WorkOrderDto>>> SearchWorOrderByTitle([FromBody] SearchFilterDto filter)
    {
        try
        {
            return await _repo.SearchWorOrderByTitle(filter);
        }
        catch(Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(WorkOrderEndpointActions.FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID_PLACEHOLDER)]
    public async Task<ActionResult<List<ActivityDto>>> FindAssociatedActivitiesByWorkOrderId([FromBody] SearchFilterDto filter)
    {
        try
        {
            return await _repo.FindAssociatedActivitiesByWorkOrderId(filter);
        }
        catch(Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route(WorkOrderEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_PLACEHOLDER)]
    public async Task<ActionResult<List<WorkOrderDto>>> FindAssociatedWorkOrdersByClientId([FromBody] SearchFilterDto filter)
    {
        try
        {
            return await _repo.FindAssociatedWorkOrdersByClientId(filter);
        } 
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}