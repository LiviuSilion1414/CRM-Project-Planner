namespace PlannerCRM.Server.Repositories;

public class WorkOrderRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(WorkOrderDto dto)
    {
        try
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
        catch ( Exception ex ) 
        {
            throw;
        }
    }

    public async Task Update(WorkOrderDto dto)
    {
        try
        {
            var existingClient = await _context.Clients.FindAsync(dto.id);

            var model = _mapper.Map<WorkOrder>(dto);

            model.FirmClient = existingClient;

            _context.Update(model);

            await _context.SaveChangesAsync();
        } 
        catch 
        {
            throw;
        }
    }

    public async Task Delete(WorkOrderFilterDto filter)
    {
        try
        {
            var workOrder = await _context.WorkOrders
                .Include(w => w.Activities)
                .SingleAsync(w => w.Id == filter.id);

            _context.Remove(workOrder);

            await _context.SaveChangesAsync();
        }
        catch  
        {
            throw;
        }
    }

    public async Task<WorkOrderDto> Get(WorkOrderFilterDto filter)
    {
        try
        {
            var workOrder = await _context.WorkOrders
                .Include(w => w.Activities)
                .Include(w => w.FirmClient)
                .SingleAsync(w => w.Id == filter.id);

            return _mapper.Map<WorkOrderDto>(workOrder);
        }
        catch 
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> List(WorkOrderFilterDto filter)
    {
        try
        {
            var workOrders = await _context.WorkOrders
                .OrderBy(w => w.Id)
                .Include(w => w.FirmClient)
                .Include(w => w.Activities)
                .Where(x => (filter.firmClientId == Guid.Empty || filter.firmClientId == x.FirmClientId) &&
                            (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery.ToLower().Trim())))
                .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(workOrders);
        }
        catch 
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> Search(WorkOrderFilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                .Where(wo => EF.Functions.ILike(wo.Name, $"%{filter.searchQuery}%"))
                .Include(wo => wo.FirmClient)
                .Include(wo => wo.Activities)
                .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrder);
        } 
        catch 
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(WorkOrderFilterDto filter)
    {
        try
        {
            var foundActivities = await _context.Activities
                .Include(ac => ac.WorkOrder)
                .Include(ac => ac.EmployeeActivities)
                .Include(ac => ac.Employees)
                .Where(ac => ac.WorkOrderId == filter.id)
                .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(foundActivities);
        }
        catch 
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(WorkOrderFilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                .Include(wo => wo.FirmClient)
                .Include(wo => wo.Activities)
                .Where(wo => wo.FirmClientId == filter.id)
                .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrder);
        }
        catch  
        {
            throw;
        }
    }
}