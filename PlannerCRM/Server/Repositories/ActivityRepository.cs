namespace PlannerCRM.Server.Repositories;

public class ActivityRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(SearchFilterDto filter)
    {
        try
        {
            var model = _mapper.Map<ActivityDto, Activity>((ActivityDto)filter.Data);


            _context.WorkOrders.Attach(model.WorkOrder);
            _context.Clients.Attach(model.WorkOrder.FirmClient);

            await _context.Activities.AddAsync(model);

            await _context.SaveChangesAsync();

            await _context.WorkOrderActivities.AddAsync(
                new()
                {
                    ActivityId = model.Guid,
                    WorkOrderId = model.WorkOrder.Guid
                }
            );

            await _context.SaveChangesAsync();
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task EditAsync(SearchFilterDto filter)
    {
        try
        { 
            var model = _mapper.Map<Activity>((ActivityDto)filter.Data);
        
            _context.WorkOrders.Attach(model.WorkOrder);
            _context.Clients.Attach(model.WorkOrder.FirmClient);

            _context.Update(model);

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
            var activity = await _context.Activities
                .Include(a => a.EmployeeActivities)
                .Include(a => a.ActivityWorkTimes)
                .SingleAsync(a => a.Guid == filter.Id);

            _context.Remove(activity);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<ActivityDto> GetByIdAsync(SearchFilterDto filter)
    {
        try
        {
            var activity = await _context.Activities
                .Include(a => a.EmployeeActivities)
                .Include(a => a.ActivityWorkTimes)
                .SingleAsync(a => a.Guid == filter.Id);

            return _mapper.Map<ActivityDto>(activity);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> GetWithPagination(SearchFilterDto filter)
    {
        try
        {
            var activities = await _context.Activities
                                           .OrderBy(a => a.Guid)
                                           .Include(a => a.EmployeeActivities)
                                           .Include(a => a.ActivityWorkTimes)
                                           .Include(a => a.WorkOrder)
                                           .ThenInclude(w => w.FirmClient)
                                           .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(activities);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> SearchActivityByTitle(SearchFilterDto filter)
    {
        try
        {
            var foundItem = await _context.Activities
                                          .Where(ac => EF.Functions.ILike(ac.Name, $"%{filter.SearchQuery}%"))
                                          .Include(ac => ac.WorkOrder)
                                          .ThenInclude(wo => wo.FirmClient)
                                          .ToListAsync();

            return _mapper?.Map<List<ActivityDto>>(foundItem);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> FindAssociatedEmployeesWithinActivity(SearchFilterDto filter)
    {
        try
        {
            var foundEmployees = await _context.Employees
                                               .Include(em => em.Activities)
                                               .Where(em => em.Activities.Any(ac => ac.Guid == filter.Id))
                                               .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(foundEmployees);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<WorkOrderDto> FindAssociatedWorkOrderByActivityId(SearchFilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                                               .Include(wo => wo.Activities)
                                               .Include(wo => wo.FirmClient)
                                               .SingleAsync(em => em.Activities.Any(ac => ac.Guid == filter.Id));

            return _mapper.Map<WorkOrderDto>(foundWorkOrder);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesWithinActivity(SearchFilterDto filter)
    {
        try
        {
            var foundWorkTimes = await _context.WorkTimes
                .Include(wt => wt.Activity)
                .Include(wt => wt.Employee)
                .Include(wt => wt.WorkOrder)
                .ThenInclude(wo => wo.FirmClient)
                .Where(wt => wt.Activity.Guid == filter.Id)
                .ToListAsync();

            return _mapper.Map<List<WorkTimeDto>>(foundWorkTimes);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}