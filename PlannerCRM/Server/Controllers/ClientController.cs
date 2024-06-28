namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController(IRepository<ClientFormDto> repo, IClientRepository clientRepository) : ControllerBase 
{
    private readonly IRepository<ClientFormDto> _repo = repo;
    private readonly IClientRepository _clientRepository = clientRepository;

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddClientAsync(ClientFormDto dto) {
        await _repo.AddAsync(dto);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditClientAsync(ClientFormDto dto) {
        await _repo.EditAsync(dto);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{clientId}")]
    public async Task<IActionResult> DeleteClientAsync(int clientId) {
        await _repo.DeleteAsync(clientId);
        return Ok();
    }

    [HttpGet("get/for/view/{clientId}")]
    public async Task<ClientViewDto> GetClientForViewByIdAsync(int clientId) =>
        await _clientRepository.GetForViewByIdAsync(clientId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{clientId}")]
    public async Task<ClientFormDto> GetClientForEditByIdAsync(int clientId) =>
        await _clientRepository.GetClientForEditByIdAsync(clientId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{clientId}")]
    public async Task<ClientDeleteDto> GetClientForDeleteByIdAsync(int clientId) =>
        await _clientRepository.GetClientForDeleteByIdAsync(clientId);
       
    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<ClientViewDto>> GetPaginatedClientsAsync(int limit, int offset) =>
        await _clientRepository.GetClientsPaginatedAsync(limit, offset);
    
    [HttpGet("get/size")]
    public async Task<int> GetCollectionSizeAsync() =>
        await _clientRepository.GetCollectionSizeAsync();

    [HttpGet("search/{clientName}/")]
    public async Task<List<ClientViewDto>> SearchClientByNameAsync(string clientName) =>
        await _clientRepository.SearchClientAsync(clientName);

    [HttpGet("search/by/id/{clientId}")]
    public async Task<List<ClientViewDto>> SearchClientByIdAsync(int clientId) =>
        await _clientRepository.SearchClientAsync(clientId);
}