namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkOrderController(IRepository<WorkOrder, WorkOrderDto> repo) : CrudController<WorkOrder, WorkOrderDto>(repo)
{ }