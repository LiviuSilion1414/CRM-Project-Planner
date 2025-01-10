namespace PlannerCRM.Server.Repositories.Specific;

public class FirmClientRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<FirmClientDto>> SearchClientByName(string clientName)
    {
        var foundClients = await _context.Clients
            .Where(cl => EF.Functions.ILike(cl.Name, $"%{clientName}%"))
            .Include(cl => cl.WorkOrders)
            .ToListAsync();

        foreach (var foundClient in foundClients)
        {
            foreach (var wo in foundClient.WorkOrders)
            {
                Console.WriteLine(wo.Name);
                
            }
        }

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