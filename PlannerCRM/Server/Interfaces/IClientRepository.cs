namespace PlannerCRM.Server.Interfaces;

public interface IClientRepository
{
    public Task<ClientViewDto> GetForViewByIdAsync(int id);
    public Task<ClientFormDto> GetClientForEditByIdAsync(int FirmClientId);
    public Task<ClientDeleteDto> GetClientForDeleteByIdAsync(int FirmClientId);
    public Task<List<ClientViewDto>> GetClientsPaginatedAsync(int limit, int offset);
    public Task<List<ClientViewDto>> SearchClientAsync(int FirmClientId);
    public Task<List<ClientViewDto>> SearchClientAsync(string clientName);
    public Task<ClientViewDto> GetClientByWorkOrderIdAsync(int workOrderId);
    public Task<int> GetCollectionSizeAsync();
}