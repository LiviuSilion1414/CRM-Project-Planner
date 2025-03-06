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
            var model = _mapper.Map<WorkOrder>(dto);

            _context.Attach(model.FirmClient);

            var existingModel = await _context.WorkOrders
                .SingleAsync(x => x.Id == model.Id);
            existingModel = model;

            _context.Update(existingModel);

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
                .SingleAsync(w => w.Id == filter.Id);

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
                .SingleAsync(w => w.Id == filter.Id);

            return _mapper.Map<WorkOrderDto>(workOrder);
        }
        catch 
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> List(FilterDto filter)
    {
        try
        {
            var workOrder = await _context.WorkOrders
                .OrderBy(w => w.Id)
                .Include(w => w.FirmClient)
                .Include(w => w.Activities)
                .ToListAsync();

            var mapped = _mapper.Map<List<WorkOrderDto>>(workOrder);
            return mapped;
        }
        catch 
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> Search(FilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                .Where(wo => EF.Functions.ILike(wo.Name, $"%{filter.SearchQuery}%"))
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

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(FilterDto filter)
    {
        try
        {
            var foundActivities = await _context.Activities
                .Include(ac => ac.WorkOrder)
                .Include(ac => ac.EmployeeActivities)
                .Include(ac => ac.Employees)
                .Where(ac => ac.WorkOrderId == filter.Id)
                .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(foundActivities);
        }
        catch 
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(FilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                .Include(wo => wo.FirmClient)
                .Include(wo => wo.Activities)
                .Where(wo => wo.FirmClientId == filter.Id)
                .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrder);
        }
        catch  
        {
            throw;
        }
    }
}