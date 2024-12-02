namespace PlannerCRM.Server.Repositories.Specific;

public class WorkOrderRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<WorkOrderDto> SearchWorOrderByTitle(string worOrderTitle)
    {
        var foundWorkOrder = await _context.WorkOrders
            .Include(wo => wo.WorkOrderCost)
            .Include(wo => wo.FirmClient)
            .Include(wo => wo.Activities)
            .SingleOrDefaultAsync(wo => EF.Functions.ILike(wo.Name, $"%{worOrderTitle}%"));

        return _mapper.Map<WorkOrderDto>(foundWorkOrder);
    }

    public async Task<ICollection<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(int workOrderId)
    {
        var foundActivities = await _context.Activities
            .Include(ac => ac.WorkOrder)
            .Include(ac => ac.ActivityWorkTimes)
            .Include(ac => ac.EmployeeActivities)
            .Include(ac => ac.Employees)
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();

        return _mapper.Map<ICollection<ActivityDto>>(foundActivities);
    }

    public async Task<ICollection<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(int clientId)
    {
        var foundWorkOrder = await _context.WorkOrders
            .Include(wo => wo.WorkOrderCost)
            .Include(wo => wo.FirmClient)
            .Include(wo => wo.Activities)
            .Where(wo => wo.FirmClientId == clientId)
            .ToListAsync();

        return _mapper.Map<ICollection<WorkOrderDto>>(foundWorkOrder);
    }
}