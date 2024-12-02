namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController(IRepository<FirmClient, FirmClientDto> genericRepo, FirmClientRepository specificRepo) 
    : CrudController<FirmClient, FirmClientDto>(genericRepo)
{
    private readonly FirmClientRepository _specificRepo = specificRepo;

    [HttpGet]
    [Route("searchClientByName/{clientName:string}")]
    public async Task<FirmClientDto> SearchClientByName(string clientName)
    {
        return await _specificRepo.SearchClientByName(clientName);
    }

    [HttpGet]
    [Route("findAssociatedWorkOrdersByClientId/{clientId:int}")]
    public async Task<ICollection<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(int clientId)
    {
        return await _specificRepo.FindAssociatedWorkOrdersByClientId(clientId);
    }
}