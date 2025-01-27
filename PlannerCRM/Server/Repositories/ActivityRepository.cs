namespace PlannerCRM.Server.Repositories;

public class ActivityRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(ActivityDto dto)
    {
        var model = _mapper.Map<ActivityDto, Activity>(dto);

        _context.WorkOrders.Attach(model.WorkOrder);
        _context.Clients.Attach(model.WorkOrder.FirmClient);

        await _context.Activities.AddAsync(model);

        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(ActivityDto dto)
    {
        var model = _mapper.Map<Activity>(dto);
        
        _context.WorkOrders.Attach(model.WorkOrder);
        _context.Clients.Attach(model.WorkOrder.FirmClient);

        _context.Update(model);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ActivityDto dto)
    {
        var activity = await _context.Activities
            .Include(a => a.EmployeeActivities)
            .Include(a => a.ActivityWorkTimes)
            .SingleAsync(a => a.Id == dto.Id);

        _context.Remove(activity);

        await _context.SaveChangesAsync();
    }

    public async Task<ActivityDto> GetByIdAsync(int id)
    {
        var activity = await _context.Activities
            .Include(a => a.EmployeeActivities)
            .Include(a => a.ActivityWorkTimes)
            .SingleAsync(a => a.Id == id);

        return _mapper.Map<ActivityDto>(activity);
    }

    public async Task<List<ActivityDto>> GetWithPagination(int limit, int offset)
    {
        var activities = await _context.Activities
            .OrderBy(a => a.Id)
            .Skip(offset)
            .Take(limit)
            .Include(a => a.EmployeeActivities)
            .Include(a => a.ActivityWorkTimes)
            .Include(a => a.WorkOrder)
            .ThenInclude(w => w.FirmClient)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);
    }

    public async Task<List<ActivityDto>> SearchActivityByTitle(string activityTitle)
    {
        var foundItem = await _context.Activities
            .Where(ac => EF.Functions.ILike(ac.Name, $"{activityTitle}"))
            .Include(ac => ac.WorkOrder)
            .ThenInclude(wo => wo.FirmClient)
            .ToListAsync();

        return _mapper?.Map<List<ActivityDto>>(foundItem);
    }

    public async Task<List<EmployeeDto>> FindAssociatedEmployeesWithinActivity(int activityId)
    {
        var foundEmployees = await _context.Employees
            .Include(em => em.Activities)
            .Where(em => em.Activities.Any(ac => ac.Id == activityId))
            .ToListAsync();

        return _mapper.Map<List<EmployeeDto>>(foundEmployees);
    }

    public async Task<WorkOrderDto> FindAssociatedWorkOrderByActivityId(int activityId)
    {
        var foundWorkOrder = await _context.WorkOrders
            .Include(wo => wo.Activities)
            .Include(wo => wo.FirmClient)
            .SingleAsync(em => em.Activities.Any(ac => ac.Id == activityId));

        return _mapper.Map<WorkOrderDto>(foundWorkOrder);
    }

    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesWithinActivity(int activityId)
    {
        var foundWorkTimes = await _context.WorkTimes
            .Include(wt => wt.Activity)
            .Include(wt => wt.Employee)
            .Include(wt => wt.WorkOrder)
            .ThenInclude(wo => wo.FirmClient)
            .Where(wt => wt.Activity.Id == activityId)
            .ToListAsync();

        return _mapper.Map<List<WorkTimeDto>>(foundWorkTimes);
    }
}