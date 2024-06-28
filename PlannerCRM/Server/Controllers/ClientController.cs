namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController(
    ClientRepository repo,
    Logger<ClientRepository> logger) : ControllerBase 
{
    private readonly ClientRepository _repo = repo;
    private readonly ILogger<ClientRepository> _logger = logger;

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddClientAsync(ClientFormDto dto) {
        await _repo.AddClientAsync(dto);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditClientAsync(ClientFormDto dto) {
        await _repo.EditClientAsync(dto);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{clientId}")]
    public async Task<IActionResult> DeleteClientAsync(int clientId) {
        await _repo.DeleteClientAsync(clientId);
        return Ok();
    }

    [HttpGet("get/for/view/{clientId}")]
    public async Task<ClientViewDto> GetClientForViewByIdAsync(int clientId) =>
        await _repo.GetClientForViewByIdAsync(clientId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{clientId}")]
    public async Task<ClientFormDto> GetClientForEditByIdAsync(int clientId) =>
        await _repo.GetClientForEditByIdAsync(clientId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{clientId}")]
    public async Task<ClientDeleteDto> GetClientForDeleteByIdAsync(int clientId) =>
        await _repo.GetClientForDeleteByIdAsync(clientId);
       
    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<ClientViewDto>> GetPaginatedClientsAsync(int limit, int offset) =>
        await _repo.GetClientsPaginatedAsync(limit, offset);
    
    [HttpGet("get/size")]
    public async Task<int> GetCollectionSizeAsync() =>
        await _repo.GetCollectionSizeAsync();

    [HttpGet("search/{clientName}/")]
    public async Task<List<ClientViewDto>> SearchClientByNameAsync(string clientName) =>
        await _repo.SearchClientAsync(clientName);

    [HttpGet("search/by/id/{clientId}")]
    public async Task<List<ClientViewDto>> SearchClientByIdAsync(int clientId) =>
        await _repo.SearchClientAsync(clientId);
}