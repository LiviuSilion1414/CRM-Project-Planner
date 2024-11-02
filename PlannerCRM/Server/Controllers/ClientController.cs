using PlannerCRM.Server.Models.Entities;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController(IClientRepository clientRepository) : CrudController<FirmClient>
{
    private readonly IClientRepository _clientRepository = clientRepository;

    [HttpGet("get/for/view/{FirmClientId}")]
    public async Task<ClientViewDto> GetClientForViewByIdAsync(int FirmClientId) =>
        await _clientRepository.GetForViewByIdAsync(FirmClientId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{FirmClientId}")]
    public async Task<ClientFormDto> GetClientForEditByIdAsync(int FirmClientId) =>
        await _clientRepository.GetClientForEditByIdAsync(FirmClientId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{FirmClientId}")]
    public async Task<ClientDeleteDto> GetClientForDeleteByIdAsync(int FirmClientId) =>
        await _clientRepository.GetClientForDeleteByIdAsync(FirmClientId);
       
    [HttpGet("get/paginated/{offset}/{limit}")]
    public async Task<List<ClientViewDto>> GetPaginatedClientsAsync(int offset, int limit) =>
        await _clientRepository.GetClientsPaginatedAsync(offset, limit);
    
    [HttpGet("get/size")]
    public async Task<int> GetCollectionSizeAsync() =>
        await _clientRepository.GetCollectionSizeAsync();

    [HttpGet("search/{clientName}/")]
    public async Task<List<ClientViewDto>> SearchClientByNameAsync(string clientName) =>
        await _clientRepository.SearchClientAsync(clientName);

    [HttpGet("search/by/id/{FirmClientId}")]
    public async Task<List<ClientViewDto>> SearchClientByIdAsync(int FirmClientId) =>
        await _clientRepository.SearchClientAsync(FirmClientId);

    [HttpGet("get/by/workorderid/{workOrderId}")]
    public async Task<ClientViewDto> SearchClientByWorkOrderIdAsync(int workOrderId) =>
        await _clientRepository.GetClientByWorkOrderIdAsync(workOrderId);
}