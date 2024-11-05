namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController(IRepository<FirmClient, FirmClientDto> repo) : CrudController<FirmClient, FirmClientDto>(repo)
{ }