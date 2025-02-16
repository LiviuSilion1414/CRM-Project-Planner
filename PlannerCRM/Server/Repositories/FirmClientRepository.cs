namespace PlannerCRM.Server.Repositories;

public class FirmClientRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(FirmClientDto dto)
    {
        var model = _mapper.Map<FirmClient>(dto);

        await _context.Clients.AddAsync(model);

        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(FirmClientDto dto)
    {
        var model = _mapper.Map<FirmClient>(dto);

        var existingModel = await _context.Clients.SingleAsync(cl => cl.Guid == model.Guid);
        existingModel = model;

        _context.Update(existingModel);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(FirmClientDto dto)
    {
        var client = await _context.Clients
            .Include(c => c.WorkOrders)
            .Include(c => c.WorkOrderCosts)
            .SingleAsync(c => c.Guid == dto.Guid);

        _context.Remove(client);

        await _context.SaveChangesAsync();
    }

    public async Task<FirmClientDto> GetByIdAsync(Guid id)
    {
        var client = await _context.Clients
            .Include(c => c.WorkOrders)
            .Include(c => c.WorkOrderCosts)
            .SingleAsync(c => c.Guid == id);

        return _mapper.Map<FirmClientDto>(client);
    }

    public async Task<List<FirmClientDto>> GetWithPagination(int limit, int offset)
    {
        var clients = await _context.Clients
            .OrderBy(c => c.Guid)
            .Skip(offset)
            .Take(limit)
            .Include(c => c.WorkOrders)
            .Include(c => c.WorkOrderCosts)
            .ToListAsync();

        return _mapper.Map<List<FirmClientDto>>(clients);
    }

    public async Task<List<FirmClientDto>> SearchClientByName(string clientName)
    {
        var foundClients = await _context.Clients
            .Where(cl => EF.Functions.ILike(cl.Name, $"%{clientName}%"))
            .Include(cl => cl.WorkOrders)
            .ToListAsync();

        return _mapper.Map<List<FirmClientDto>>(foundClients);
    }

    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(Guid clientId)
    {
        var foundWorkOrders = await _context.WorkOrders
            .Include(wo => wo.FirmClient)
            .Include(wo => wo.Activities)
            .Include(wo => wo.WorkOrderCost)
            .Where(wo => wo.FirmClientId == clientId)
            .ToListAsync();

        return _mapper.Map<List<WorkOrderDto>>(foundWorkOrders);
    }
}