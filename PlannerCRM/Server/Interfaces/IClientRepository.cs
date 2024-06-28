namespace PlannerCRM.Server.Interfaces;

public interface IClientRepository
{
    public Task<ClientViewDto> GetForViewByIdAsync(int id);
    public Task<ClientFormDto> GetClientForEditByIdAsync(int clientId);
    public Task<ClientDeleteDto> GetClientForDeleteByIdAsync(int clientId);
    public Task<List<ClientViewDto>> GetClientsPaginatedAsync(int limit, int offset);
    public Task<List<ClientViewDto>> SearchClientAsync(int clientId);
    public Task<List<ClientViewDto>> SearchClientAsync(string clientName);
    public Task<int> GetCollectionSizeAsync();
}