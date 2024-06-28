namespace PlannerCRM.Server.Repositories;

public class ClientRepository(
    AppDbContext dbContext,
    DtoValidatorUtillity validator) : IRepository<ClientFormDto>, IClientRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly DtoValidatorUtillity _validator = validator;

    public async Task AddAsync(ClientFormDto dto) {
        var isValid = await _validator.ValidateClientAsync(dto, OperationType.ADD);
        
        if (isValid) {
            await _dbContext.Clients.AddAsync(dto.MapToFirmClient());
            await _dbContext.SaveChangesAsync();
        }        
    }

    public async Task EditAsync(ClientFormDto dto) {
        var isValid = await _validator.ValidateClientAsync(dto, OperationType.EDIT);
        
        if (isValid) {
            var model = await _dbContext.Clients
                .SingleAsync(cl => cl.Id == dto.Id);
            
            model = dto.MapToFirmClient();
            
            _dbContext.Clients.Update(model);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id) {
        var clientDelete = await _validator.ValidateDeleteClientAsync(id);

        if (clientDelete is not null) {
            _dbContext.Remove(clientDelete);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ClientViewDto> GetForViewByIdAsync(int clientId) {
        return await _dbContext.Clients
            .Where(cl => cl.Id == clientId)
            .Select(client => client.MapToClientViewDto())
            .SingleAsync();
    }

    public async Task<ClientFormDto> GetClientForEditByIdAsync(int id) {
        return await _dbContext.Clients
            .Select(client => client.MapToClientFormDto())
            .SingleAsync(cl => cl.Id == id);
    }

    public async Task<ClientDeleteDto> GetClientForDeleteByIdAsync(int id) {
        return await _dbContext.Clients
            .Select(client => client.MapToClientDeleteDto())
            .SingleAsync(cl => cl.Id == id);
    }

    public async Task<List<ClientViewDto>> GetClientsPaginatedAsync(int limit, int offset) {
        return await _dbContext.Clients
            .OrderBy(client => client.Id)
            .Skip(limit)
            .Take(offset)
            .Select(client => client.MapToClientViewDto())
            .ToListAsync();
    }

    public async Task<int> GetCollectionSizeAsync() =>
        await _dbContext.Clients.CountAsync();

    public async Task<List<ClientViewDto>> SearchClientAsync(string clientName) {
        return await _dbContext.Clients
            .Select(cl => cl.MapToClientViewDto())
            .Where(cl => EF.Functions.ILike(cl.Name, $"%{clientName}%"))
            .ToListAsync();
    }

    public async Task<List<ClientViewDto>> SearchClientAsync(int clientId) {
        return await _dbContext.Clients
            .Select(cl => cl.MapToClientViewDto())
            .Where(cl => cl.Id == clientId)
            .ToListAsync();
    }
}