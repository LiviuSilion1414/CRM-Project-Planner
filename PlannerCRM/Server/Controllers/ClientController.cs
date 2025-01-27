using PlannerCRM.Server.Repositories;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController(FirmClientRepository repo) : ControllerBase
{
    private readonly FirmClientRepository _repo = repo;

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(FirmClientDto client)
    {
        await _repo.AddAsync(client);

        return Ok();
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit(FirmClientDto client)
    {
        await _repo.EditAsync(client);

        return Ok();
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(FirmClientDto client)
    {
        await _repo.DeleteAsync(client);

        return Ok();
    }

    [HttpGet]
    [Route("getById/{clientId}")]
    public async Task<ActionResult<FirmClient>> GetById(int clientId)
    {
        var client = await _repo.GetByIdAsync(clientId);
        return Ok(client);
    }

    [HttpGet]
    [Route("getWithPagination/{limit}/{offset}")]
    public async Task<ActionResult<List<FirmClient>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }

    [HttpGet]
    [Route("searchClientByName/{clientName}")]
    public async Task<List<FirmClientDto>> SearchClientByName(string clientName)
    {
        return await _repo.SearchClientByName(clientName);
    }

    [HttpGet]
    [Route("findAssociatedWorkOrdersByClientId/{clientId}")]
    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(int clientId)
    {
        return await _repo.FindAssociatedWorkOrdersByClientId(clientId);
    }
}