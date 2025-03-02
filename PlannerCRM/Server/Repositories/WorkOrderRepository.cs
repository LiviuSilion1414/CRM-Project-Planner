namespace PlannerCRM.Server.Repositories;

public class WorkOrderRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(SearchFilterDto filter)
    {
        try
        {
            var model = _mapper.Map<WorkOrder>((WorkOrderDto)filter.Data);

            _context.Attach(model.FirmClient);

            await _context.WorkOrders.AddAsync(model);
        
            await _context.SaveChangesAsync();

            await _context.ClientWorkOrders.AddAsync(
                new ClientWorkOrder
                {
                    FirmClientId = model.FirmClientId,
                    WorkOrderId = model.Guid
                }
            );

            await _context.SaveChangesAsync();
        }
        catch ( Exception ex ) 
        {
            throw;
        }
    }

    public async Task EditAsync(SearchFilterDto filter)
    {
        try
        {
            var model = _mapper.Map<WorkOrder>((WorkOrderDto)filter.Data);

            _context.Attach(model.FirmClient);

            var existingModel = await _context.WorkOrders
                .SingleAsync(x => x.Guid == model.Guid);
            existingModel = model;

            _context.Update(existingModel);

            await _context.SaveChangesAsync();
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task DeleteAsync(SearchFilterDto filter)
    {
        try
        {
            var workOrder = await _context.WorkOrders
                .Include(w => w.Activities)
                .Include(w => w.WorkOrderCost)
                .SingleAsync(w => w.Guid == filter.Id);

            _context.Remove(workOrder);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex) 
        {
            throw;
        }
    }

    public async Task<WorkOrderDto> GetByIdAsync(SearchFilterDto filter)
    {
        try
        {
            var workOrder = await _context.WorkOrders
                .Include(w => w.Activities)
                .Include(w => w.WorkOrderCost)
                .Include(w => w.FirmClient)
                .SingleAsync(w => w.Guid == filter.Id);

            return _mapper.Map<WorkOrderDto>(workOrder);
        }
        catch(Exception ex) 
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> GetWithPagination(SearchFilterDto filter)
    {
        try
        {
            var workOrder = await _context.WorkOrders
                .OrderBy(w => w.Guid)
                .Include(w => w.WorkOrderCost)
                .Include(w => w.FirmClient)
                .Include(w => w.Activities)
                .ToListAsync();

            var mapped = _mapper.Map<List<WorkOrderDto>>(workOrder);
            return mapped;
        }
        catch(Exception ex) 
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> SearchWorOrderByTitle(SearchFilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                .Where(wo => EF.Functions.ILike(wo.Name, $"%{filter.SearchQuery}%"))
                .Include(wo => wo.WorkOrderCost)
                .Include(wo => wo.FirmClient)
                .Include(wo => wo.Activities)
                .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrder);
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(SearchFilterDto filter)
    {
        try
        {
            var foundActivities = await _context.Activities
                .Include(ac => ac.WorkOrder)
                .Include(ac => ac.ActivityWorkTimes)
                .Include(ac => ac.EmployeeActivities)
                .Include(ac => ac.Employees)
                .Where(ac => ac.WorkOrderId == filter.Id)
                .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(foundActivities);
        }
        catch(Exception ex) 
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(SearchFilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                .Include(wo => wo.WorkOrderCost)
                .Include(wo => wo.FirmClient)
                .Include(wo => wo.Activities)
                .Where(wo => wo.FirmClientId == filter.Id)
                .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrder);
        }
        catch (Exception ex) 
        {
            throw;
        }
    }
}