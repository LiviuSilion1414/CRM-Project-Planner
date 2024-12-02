using Microsoft.EntityFrameworkCore;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkOrderController(IRepository<WorkOrder, WorkOrderDto> genericRepo, WorkOrderRepository specificRepo) : CrudController<WorkOrder, WorkOrderDto>(genericRepo)
{ 
    private readonly WorkOrderRepository _specificRepo = specificRepo;

    [HttpGet]
    [Route("searchWorOrderByTitle/{worOrderTitle:string}")]
    public async Task<WorkOrderDto> SearchWorOrderByTitle(string worOrderTitle)
    {
        return await _specificRepo.SearchWorOrderByTitle(worOrderTitle);
    }

    [HttpGet]
    [Route("findAssociatedActivitiesByWorkOrderId/{workOrderId:int}")]
    public async Task<ICollection<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(int workOrderId)
    {
        return await _specificRepo.FindAssociatedActivitiesByWorkOrderId(workOrderId);
    }

    [HttpGet]
    [Route("findAssociatedWorkOrdersByClientId/{clientId:int}")]
    public async Task<ICollection<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(int clientId)
    {
        return await _specificRepo.FindAssociatedWorkOrdersByClientId(clientId);
    }
}