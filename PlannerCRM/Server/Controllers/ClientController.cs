namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController(FirmClientRepository repo) : ControllerBase
{
    private readonly FirmClientRepository _repo = repo;

    [HttpPost]
    [Route(EndpointsCrudActions.ADD)]
    public async Task<IActionResult> Add(FirmClientDto client)
    {
        await _repo.AddAsync(client);

        return Ok();
    }

    [HttpPut]
    [Route(EndpointsCrudActions.EDIT)]
    public async Task<IActionResult> Edit(FirmClientDto client)
    {
        await _repo.EditAsync(client);

        return Ok();
    }

    [HttpPost]
    [Route(EndpointsCrudActions.DELETE)]
    public async Task<IActionResult> Delete(FirmClientDto client)
    {
        await _repo.DeleteAsync(client);

        return Ok();
    }

    [HttpGet]
    [Route(EndpointsCrudActions.GET_BY_ID)]
    public async Task<ActionResult<FirmClient>> GetById(int clientId)
    {
        var client = await _repo.GetByIdAsync(clientId);
        return Ok(client);
    }

    [HttpGet]
    [Route(EndpointsCrudActions.GET_WITH_PAGINATION)]
    public async Task<ActionResult<List<FirmClient>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route(ClientEndpointActions.SEARCH_CLIENT_BY_NAME)]
    public async Task<List<FirmClientDto>> SearchClientByName(string clientName)
    {
        return await _repo.SearchClientByName(clientName);
    }

    [HttpGet]
    [Route(ClientEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID)]
    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(int clientId)
    {
        return await _repo.FindAssociatedWorkOrdersByClientId(clientId);
    }
}