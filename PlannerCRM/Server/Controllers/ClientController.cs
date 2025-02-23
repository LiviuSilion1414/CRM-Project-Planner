namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController(FirmClientRepository repo) : ControllerBase
{
    private readonly FirmClientRepository _repo = repo;

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<IActionResult> Add(FirmClientDto client)
    {
        await _repo.AddAsync(client);

        return Ok();
    }

    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<IActionResult> Edit(FirmClientDto client)
    {
        await _repo.EditAsync(client);

        return Ok();
    }

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<IActionResult> Delete(FirmClientDto client)
    {
        await _repo.DeleteAsync(client);

        return Ok();
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ActionResult<FirmClient>> GetById(Guid clientId)
    {
        var client = await _repo.GetByIdAsync(clientId);
        return Ok(client);
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ActionResult<List<FirmClient>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route(ClientEndpointActions.SEARCH_CLIENT_BY_NAME_PLACEHOLDER)]
    public async Task<List<FirmClientDto>> SearchClientByName(string clientName)
    {
        return await _repo.SearchClientByName(clientName);
    }

    [HttpGet]
    [Route(ClientEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_PLACEHOLDER)]
    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(Guid clientId)
    {
        return await _repo.FindAssociatedWorkOrdersByClientId(clientId);
    }
}