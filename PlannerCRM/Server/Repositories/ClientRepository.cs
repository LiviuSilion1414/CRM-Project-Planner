namespace PlannerCRM.Server.Repositories;

public class ClientRepository(
    AppDbContext dbContext,
    DtoValidatorUtillity validator) : IRepository<ClientFormDto>, IClientRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly DtoValidatorUtillity _validator = validator;

    public async Task AddAsync(ClientFormDto dto)
    {
        var isValid = await _validator.ValidateClientAsync(dto, OperationType.ADD);

        if (isValid)
        {
            await _dbContext.Clients.AddAsync(dto.MapToFirmClient());
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task EditAsync(ClientFormDto dto)
    {
        var isValid = await _validator.ValidateClientAsync(dto, OperationType.EDIT);

        if (isValid)
        {
            var model = await _dbContext.Clients
                .SingleAsync(cl => cl.Id == dto.Id);

            model = dto.MapToFirmClient();

            _dbContext.Clients.Update(model);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var clientDelete = await _validator.ValidateDeleteClientAsync(id);

        if (clientDelete is not null)
        {
            _dbContext.Remove(clientDelete);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ClientViewDto> GetForViewByIdAsync(int FirmClientId)
    {
        return await _dbContext.Clients
            .Where(cl => cl.Id == FirmClientId)
            .Select(client => client.MapToClientViewDto())
            .SingleAsync();
    }

    public async Task<ClientFormDto> GetClientForEditByIdAsync(int id)
    {
        var client = await _dbContext.Clients
            .SingleAsync(cl => cl.Id == id);

        return client.MapToClientFormDto();
    }

    public async Task<ClientDeleteDto> GetClientForDeleteByIdAsync(int id)
    {
        var client = await _dbContext.Clients
            .SingleAsync(cl => cl.Id == id);

        return client.MapToClientDeleteDto();
    }

    public async Task<List<ClientViewDto>> GetClientsPaginatedAsync(int offset, int limit)
    {
        return await _dbContext.Clients
            .OrderBy(client => client.Id)
            .Skip(offset)
            .Take(limit)
            .Select(client => client.MapToClientViewDto())
            .ToListAsync();
    }

    public async Task<int> GetCollectionSizeAsync() =>
        await _dbContext.Clients.CountAsync();

    public async Task<List<ClientViewDto>> SearchClientAsync(string clientName)
    {
        var clients = await _dbContext.Clients
            .Where(cl => EF.Functions.ILike(cl.Name, $"%{clientName}%"))
            .ToListAsync();

        return clients
            .Select(cl => cl.MapToClientViewDto())
            .ToList();
    }

    public async Task<List<ClientViewDto>> SearchClientAsync(int FirmClientId)
    {
        var clients = await _dbContext.Clients
            .Where(cl => cl.Id == FirmClientId)
            .ToListAsync();

        return clients
            .Select(cl => cl.MapToClientViewDto())
            .ToList();
    }

    public async Task<ClientViewDto> GetClientByWorkOrderIdAsync(int workOrderId)
    {
        var workOrder = await _dbContext.WorkOrders.SingleAsync(wo => wo.Id == workOrderId);
        var client = await _dbContext.Clients.SingleAsync(cl => cl.Id == workOrder.FirmClientId);

        return client.MapToClientViewDto();
    }
}