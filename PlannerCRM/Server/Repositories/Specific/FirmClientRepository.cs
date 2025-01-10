namespace PlannerCRM.Server.Repositories.Specific;

public class FirmClientRepository(AppDbContext context, IMapper mapper)
    : Repository<FirmClient, FirmClientDto>(context, mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public override async Task DeleteAsync(int id)
    {
        var client = await _context.Clients
            .Include(c => c.WorkOrders)
            .Include(c => c.WorkOrderCosts)
            .SingleAsync(c => c.Id == id);

        _context.Remove(client);

        await _context.SaveChangesAsync();
    }

    public override async Task<FirmClientDto> GetByIdAsync(int id)
    {
        var client = await _context.Clients
            .Include(c => c.WorkOrders)
            .Include(c => c.WorkOrderCosts)
            .SingleAsync(c => c.Id == id);

        return _mapper.Map<FirmClientDto>(client);
    }

    public override async Task<ICollection<FirmClientDto>> GetWithPagination(int limit, int offset)
    {
        var clients = await _context.Clients
            .OrderBy(c => c.Id)
            .Skip(offset)
            .Take(limit)
            .Include(c => c.WorkOrders)
            .Include(c => c.WorkOrderCosts)
            .ToListAsync();

        return _mapper.Map<ICollection<FirmClientDto>>(clients);
    }

    public async Task<IEnumerable<FirmClientDto>> SearchClientByName(string clientName)
    {
        var foundClients = await _context.Clients
            .Where(cl => EF.Functions.ILike(cl.Name, $"%{clientName}%"))
            .Include(cl => cl.WorkOrders)
            .ToListAsync();

        return _mapper.Map<IEnumerable<FirmClientDto>>(foundClients);
    }

    public async Task<ICollection<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(int clientId)
    {
        var foundWorkOrders = await _context.WorkOrders
            .Include(wo => wo.FirmClient)
            .Include(wo => wo.Activities)
            .Include(wo => wo.WorkOrderCost)
            .Where(wo => wo.Id == clientId)
            .ToListAsync();

        return _mapper.Map<ICollection<WorkOrderDto>>(foundWorkOrders);
    }
}