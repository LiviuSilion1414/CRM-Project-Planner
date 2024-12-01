namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController(IRepository<FirmClient, FirmClientDto> repo) : CrudController<FirmClient, FirmClientDto>(repo)
{ }