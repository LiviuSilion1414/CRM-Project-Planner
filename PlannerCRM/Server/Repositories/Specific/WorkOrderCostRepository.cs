namespace PlannerCRM.Server.Repositories.Specific;

public class WorkOrderCostRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    
    public async Task<WorkOrderCostDto> SearchWorOrderCostByWorkOrderTitle(string worOrderTitle)
    {
        var foundWorkOrder = await _context.WorkOrderCosts
            .Include(w => w.WorkOrder)
            .Include(w => w.FirmClient)
            .SingleAsync(w => EF.Functions.ILike(w.Name, $"%{worOrderTitle}%"));

        return _mapper.Map<WorkOrderCostDto>(foundWorkOrder);
    }
}