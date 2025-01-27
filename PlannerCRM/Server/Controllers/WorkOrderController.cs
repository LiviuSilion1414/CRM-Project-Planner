using PlannerCRM.Server.Repositories;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrderController(WorkOrderRepository repo) : ControllerBase
{ 
    private readonly WorkOrderRepository _repo = repo;

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(WorkOrderDto workOrder)
    {
        await _repo.AddAsync(workOrder);

        return Ok();
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit(WorkOrderDto workOrder)
    {
        await _repo.EditAsync(workOrder);

        return Ok();
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(WorkOrderDto workOrder)
    {
        await _repo.DeleteAsync(workOrder);

        return Ok();
    }

    [HttpGet]
    [Route("getById/{workOrderId}")]
    public async Task<ActionResult<WorkOrderDto>> GetById(int workOrderId)
    {
        var workOrder = await _repo.GetByIdAsync(workOrderId);
        return Ok(workOrder);
    }

    [HttpGet]
    [Route("getWithPagination/{limit}/{offset}")]
    public async Task<ActionResult<List<WorkOrderDto>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route("searchWorkOrderByTitle/{worOrderTitle}")]
    public async Task<List<WorkOrderDto>> SearchWorOrderByTitle(string worOrderTitle)
    {
        return await _repo.SearchWorOrderByTitle(worOrderTitle);
    }

    [HttpGet]
    [Route("findAssociatedActivitiesByWorkOrderId/{workOrderId}")]
    public async Task<List<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(int workOrderId)
    {
        return await _repo.FindAssociatedActivitiesByWorkOrderId(workOrderId);
    }

    [HttpGet]
    [Route("findAssociatedWorkOrdersByClientId/{clientId}")]
    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(int clientId)
    {
        return await _repo.FindAssociatedWorkOrdersByClientId(clientId);
    }
}