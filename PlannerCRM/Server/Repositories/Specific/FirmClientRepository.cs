namespace PlannerCRM.Server.Repositories.Specific;

public class FirmClientRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<FirmClientDto> SearchClientByName(string name)
    {
        var foundClient = await _context.Clients
            .SingleOrDefaultAsync(cl => EF.Functions.ILike(cl.Name, $"{cl.Name}"));

        return _mapper.Map<FirmClientDto>(foundClient);
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