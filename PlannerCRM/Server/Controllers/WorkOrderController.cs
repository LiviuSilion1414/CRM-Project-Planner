namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkOrderController(WorkOrderRepository repo) : ControllerBase
{ 
    private readonly WorkOrderRepository _repo = repo;

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<IActionResult> Add(WorkOrderDto workOrder)
    {
        await _repo.AddAsync(workOrder);

        return Ok();
    }

    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<IActionResult> Edit(WorkOrderDto workOrder)
    {
        await _repo.EditAsync(workOrder);

        return Ok();
    }

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<IActionResult> Delete(WorkOrderDto workOrder)
    {
        await _repo.DeleteAsync(workOrder);

        return Ok();
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ActionResult<WorkOrderDto>> GetById(Guid workOrderId)
    {
        var workOrder = await _repo.GetByIdAsync(workOrderId);
        return Ok(workOrder);
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ActionResult<List<WorkOrderDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route(WorkOrderEndpointActions.SEARCH_WORKORDER_BY_TITLE_PLACEHOLDER)]
    public async Task<List<WorkOrderDto>> SearchWorOrderByTitle(string worOrderTitle)
    {
        return await _repo.SearchWorOrderByTitle(worOrderTitle);
    }

    [HttpGet]
    [Route(WorkOrderEndpointActions.FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID_PLACEHOLDER)]
    public async Task<List<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(Guid itemId)
    {
        return await _repo.FindAssociatedActivitiesByWorkOrderId(itemId);
    }

    [HttpGet]
    [Route(WorkOrderEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_PLACEHOLDER)]
    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(Guid clientId)
    {
        return await _repo.FindAssociatedWorkOrdersByClientId(clientId);
    }
}