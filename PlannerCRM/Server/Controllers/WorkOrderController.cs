using PlannerCRM.Server.Repositories;
using PlannerCRM.Shared.Constants.ApiEndpoints.Routes;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrderController(WorkOrderRepository repo) : ControllerBase
{ 
    private readonly WorkOrderRepository _repo = repo;

    [HttpPost]
    [Route(EndpointCrudActions.ADD)]
    public async Task<IActionResult> Add(WorkOrderDto workOrder)
    {
        await _repo.AddAsync(workOrder);

        return Ok();
    }

    [HttpPut]
    [Route(EndpointCrudActions.EDIT)]
    public async Task<IActionResult> Edit(WorkOrderDto workOrder)
    {
        await _repo.EditAsync(workOrder);

        return Ok();
    }

    [HttpPost]
    [Route(EndpointCrudActions.DELETE)]
    public async Task<IActionResult> Delete(WorkOrderDto workOrder)
    {
        await _repo.DeleteAsync(workOrder);

        return Ok();
    }

    [HttpGet]
    [Route(EndpointCrudActions.GET_BY_ID)]
    public async Task<ActionResult<WorkOrderDto>> GetById(int workOrderId)
    {
        var workOrder = await _repo.GetByIdAsync(workOrderId);
        return Ok(workOrder);
    }

    [HttpGet]
    [Route(EndpointCrudActions.GET_WITH_PAGINATION)]
    public async Task<ActionResult<List<WorkOrderDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route(WorkOrderEndpointActions.SEARCH_WORKORDER_BY_TITLE)]
    public async Task<List<WorkOrderDto>> SearchWorOrderByTitle(string worOrderTitle)
    {
        return await _repo.SearchWorOrderByTitle(worOrderTitle);
    }

    [HttpGet]
    [Route(WorkOrderEndpointActions.FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID)]
    public async Task<List<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(int workOrderId)
    {
        return await _repo.FindAssociatedActivitiesByWorkOrderId(workOrderId);
    }

    [HttpGet]
    [Route(WorkOrderEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID)]
    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(int clientId)
    {
        return await _repo.FindAssociatedWorkOrdersByClientId(clientId);
    }
}