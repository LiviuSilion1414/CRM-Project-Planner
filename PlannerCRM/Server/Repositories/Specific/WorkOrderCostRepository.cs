namespace PlannerCRM.Server.Repositories.Specific;

public class WorkOrderCostRepository(AppDbContext context, IMapper mapper)
    : Repository<WorkOrderCost, WorkOrderCostDto>(context, mapper), IRepository<WorkOrderCost, WorkOrderCostDto>
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ICollection<WorkOrderCostDto>> SearchWorOrderCostByWorkOrderTitle(string worOrderTitle)
    {
        var foundWorkOrder = await _context.WorkOrderCosts
            .Where(w => EF.Functions.ILike(w.Name, $"%{worOrderTitle}%"))
            .Include(w => w.WorkOrder)
            .Include(w => w.FirmClient)
            .ToListAsync();

        return _mapper.Map<ICollection<WorkOrderCostDto>>(foundWorkOrder);
    }
}