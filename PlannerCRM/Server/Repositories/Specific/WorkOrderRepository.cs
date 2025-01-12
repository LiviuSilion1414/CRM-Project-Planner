namespace PlannerCRM.Server.Repositories.Specific;

public class WorkOrderRepository(AppDbContext context, IMapper mapper)
    : Repository<WorkOrder, WorkOrderDto>(context, mapper), IRepository<WorkOrder, WorkOrderDto>
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public override async Task AddAsync(WorkOrder model)
    {
        await base.AddAsync(model);

        await _context.ClientWorkOrders.AddAsync(
            new ClientWorkOrder { 
                FirmClientId = model.FirmClientId,
                WorkOrderId = model.Id
            }
        );
        await _context.SaveChangesAsync();
    }

    public override async Task EditAsync(WorkOrder model, int id)
    {
        await base.EditAsync(model, id);
        
        var existingClientWorkOrder = await _context.ClientWorkOrders
            .SingleAsync(x => x.WorkOrderId == id);

        existingClientWorkOrder.FirmClientId = model.FirmClientId;
        existingClientWorkOrder.WorkOrderId = model.Id;

        _context.Update(existingClientWorkOrder);

        await _context.SaveChangesAsync();

    }

    public override async Task DeleteAsync(int id)
    {
        var workOrder = await _context.WorkOrders
            .Include(w => w.Activities)
            .Include(w => w.WorkOrderCost)
            .SingleAsync(w => w.Id == id);

        _context.Remove(workOrder);
        
        await _context.SaveChangesAsync();
    }

    public override async Task<WorkOrderDto> GetByIdAsync(int id)
    {
        var workOrder = await _context.WorkOrders
            .Include(w => w.Activities)
            .Include(w => w.WorkOrderCost)
            .Include(w => w.FirmClient)
            .SingleAsync(w => w.Id == id);

        return _mapper.Map<WorkOrderDto>(workOrder);
    }

    public override async Task<ICollection<WorkOrderDto>> GetWithPagination(int limit, int offset)
    {
        var workOrder = await _context.WorkOrders
            .OrderBy(w => w.Id)
            .Skip(offset)
            .Take(limit)
            .Include(w => w.Activities)
            .Include(w => w.WorkOrderCost)
            .Include(w => w.FirmClient)
            .ToListAsync();

        return _mapper.Map<ICollection<WorkOrderDto>>(workOrder);
    }

    public async Task<ICollection<WorkOrderDto>> SearchWorOrderByTitle(string worOrderTitle)
    {
        var foundWorkOrder = await _context.WorkOrders
            .Where(wo => EF.Functions.ILike(wo.Name, $"%{worOrderTitle}%"))
            .Include(wo => wo.WorkOrderCost)
            .Include(wo => wo.FirmClient)
            .Include(wo => wo.Activities)
            .ToListAsync();

        return _mapper.Map<ICollection<WorkOrderDto>>(foundWorkOrder);
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