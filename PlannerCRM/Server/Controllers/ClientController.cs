namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController(FirmClientRepository repo) : ControllerBase
{
    private readonly FirmClientRepository _repo = repo;

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<IActionResult> Add([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.AddAsync(filter);
            return Ok();
        } catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<IActionResult> Edit([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.EditAsync(filter);
            return Ok();
        } catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<IActionResult> Delete([FromBody] SearchFilterDto filter)
    {
        try
        {
            await _repo.DeleteAsync(filter);
            return Ok();
        } catch (Exception ex)
        {
            throw;
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ActionResult<FirmClient>> GetById([FromBody] SearchFilterDto filter)
    {
        try
        {
            var client = await _repo.GetByIdAsync(filter);
            return Ok(client);
        } catch (Exception ex)
        {
            throw;
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ActionResult<List<FirmClient>>> GetWithPagination([FromBody] SearchFilterDto filter)
    {
        try
        {
            var entities = await _repo.GetWithPagination(filter);
            return Ok(entities);
        } catch (Exception ex)
        {
            throw;
        }
    }

    [HttpGet]
    [Route(ClientEndpointActions.SEARCH_CLIENT_BY_NAME_PLACEHOLDER)]
    public async Task<List<FirmClientDto>> SearchClientByName([FromBody] SearchFilterDto filter)
    {
        try
        {
            return await _repo.SearchClientByName(filter);
        } catch (Exception ex)
        {
            throw;
        }
    }

    [HttpGet]
    [Route(ClientEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_PLACEHOLDER)]
    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId([FromBody] SearchFilterDto filter)
    {
        try
        {
            return await _repo.FindAssociatedWorkOrdersByClientId(filter);
        } catch (Exception ex)
        {
            throw;
        }
    }
}