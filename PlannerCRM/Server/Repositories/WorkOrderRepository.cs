namespace PlannerCRM.Server.Repositories;

public class WorkOrderRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(WorkOrderDto dto)
    {
        var model = _mapper.Map<WorkOrder>(dto);

        _context.Attach(model.FirmClient);

        await _context.WorkOrders.AddAsync(model);
        
        await _context.SaveChangesAsync();

        await _context.ClientWorkOrders.AddAsync(
            new ClientWorkOrder
            {
                FirmClientId = model.FirmClientId,
                WorkOrderId = model.Id
            }
        );

        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(WorkOrderDto dto)
    {
        var model = _mapper.Map<WorkOrder>(dto);
        
        _context.Attach(model.FirmClient);

        var existingModel = await _context.WorkOrders
            .SingleAsync(x => x.Id == model.Id);
        existingModel = model;

        _context.Update(existingModel);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(WorkOrderDto dto)
    {
        var workOrder = await _context.WorkOrders
            .Include(w => w.Activities)
            .Include(w => w.WorkOrderCost)
            .SingleAsync(w => w.Id == dto.Id);

        _context.Remove(workOrder);

        await _context.SaveChangesAsync();
    }

    public async Task<WorkOrderDto> GetByIdAsync(int id)
    {
        var workOrder = await _context.WorkOrders
            .Include(w => w.Activities)
            .Include(w => w.WorkOrderCost)
            .Include(w => w.FirmClient)
            .SingleAsync(w => w.Id == id);

        return _mapper.Map<WorkOrderDto>(workOrder);
    }

    public async Task<List<WorkOrderDto>> GetWithPagination(int limit, int offset)
    {
        var workOrder = await _context.WorkOrders
            .OrderBy(w => w.Id)
            .Skip(offset)
            .Take(limit)
            .Include(w => w.WorkOrderCost)
            .Include(w => w.FirmClient)
            .Include(w => w.Activities)
            .ToListAsync();

        var mapped = _mapper.Map<List<WorkOrderDto>>(workOrder);
        return mapped;
    }

    public async Task<List<WorkOrderDto>> SearchWorOrderByTitle(string worOrderTitle)
    {
        var foundWorkOrder = await _context.WorkOrders
            .Where(wo => EF.Functions.ILike(wo.Name, $"%{worOrderTitle}%"))
            .Include(wo => wo.WorkOrderCost)
            .Include(wo => wo.FirmClient)
            .Include(wo => wo.Activities)
            .ToListAsync();

        return _mapper.Map<List<WorkOrderDto>>(foundWorkOrder);
    }

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(int workOrderId)
    {
        var foundActivities = await _context.Activities
            .Include(ac => ac.WorkOrder)
            .Include(ac => ac.ActivityWorkTimes)
            .Include(ac => ac.EmployeeActivities)
            .Include(ac => ac.Employees)
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(foundActivities);
    }

    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(int clientId)
    {
        var foundWorkOrder = await _context.WorkOrders
            .Include(wo => wo.WorkOrderCost)
            .Include(wo => wo.FirmClient)
            .Include(wo => wo.Activities)
            .Where(wo => wo.FirmClientId == clientId)
            .ToListAsync();

        return _mapper.Map<List<WorkOrderDto>>(foundWorkOrder);
    }
}