namespace PlannerCRM.Server.Repositories.Specific;

public class ActivityRepository(AppDbContext context, IMapper mapper)
    : Repository<Activity, ActivityDto>(context, mapper), IRepository<Activity, ActivityDto>
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public override async Task AddAsync(Activity model)
    {
        await base.AddAsync(model); //may be an issue

        await _context.WorkOrderActivities.AddAsync(
            new WorkOrderActivity 
            {
                ActivityId = model.Id,
                WorkOrderId = model.WorkOrderId
            }
        );

        await _context.SaveChangesAsync();
    }

    public override async Task EditAsync(Activity model, int id)
    {
        await base.EditAsync(model, id);

        var existingEmployeeActivity = await _context.EmployeeActivities
            .Where(a => a.Id == id)
            .ToListAsync();

        List<EmployeeActivity> updatedEmployeeActivities = [];

        foreach (var em in model.Employees)
        {
            if (!existingEmployeeActivity.Any(ex => ex.EmployeeId == em.Id))
            {
                updatedEmployeeActivities.Add(
                    new EmployeeActivity()
                    {
                        EmployeeId = em.Id
                    }
                );
            }
        }

        _context.Update(updatedEmployeeActivities);

        await _context.SaveChangesAsync();
    }

    public override async Task DeleteAsync(int id)
    {
        var activity = await _context.Activities
            .Include(a => a.EmployeeActivities)
            .Include(a => a.ActivityWorkTimes)
            .SingleAsync(a => a.Id == id);

        _context.Remove(activity);

        await _context.SaveChangesAsync();
    }

    public override async Task<ActivityDto> GetByIdAsync(int id)
    {
        var activity = await _context.Activities
            .Include(a => a.EmployeeActivities)
            .Include(a => a.ActivityWorkTimes)
            .SingleAsync(a => a.Id == id);

        return _mapper.Map<ActivityDto>(activity);
    }

    public override async Task<ICollection<ActivityDto>> GetWithPagination(int limit, int offset)
    {
        var activities = await _context.Activities
            .OrderBy(a => a.Id)
            .Skip(offset)
            .Take(limit)
            .Include(a => a.EmployeeActivities)
            .Include(a => a.ActivityWorkTimes)
            .ToListAsync();

        return _mapper.Map<ICollection<ActivityDto>>(activities);
    }

    public async Task<ICollection<ActivityDto>> SearchActivityByTitle(string activityTitle)
    {
        var foundItem = await _context.Activities
            .Where(ac => EF.Functions.ILike(ac.Name, $"{activityTitle}"))
            .Include(ac => ac.WorkOrder)
            .ThenInclude(wo => wo.FirmClient)
            .ToListAsync();

        return _mapper?.Map<ICollection<ActivityDto>>(foundItem);
    }

    public async Task<ICollection<EmployeeDto>> FindAssociatedEmployeesWithinActivity(int activityId)
    {
        var foundEmployees = await _context.Employees
            .Include(em => em.Activities)
            .Where(em => em.Activities.Any(ac => ac.Id == activityId))
            .ToListAsync();

        return _mapper.Map<ICollection<EmployeeDto>>(foundEmployees);
    }

    public async Task<WorkOrderDto> FindAssociatedWorkOrderByActivityId(int activityId)
    {
        var foundWorkOrder = await _context.WorkOrders
            .Include(wo => wo.Activities)
            .Include(wo => wo.FirmClient)
            .SingleAsync(em => em.Activities.Any(ac => ac.Id == activityId));

        return _mapper.Map<WorkOrderDto>(foundWorkOrder);
    }

    public async Task<ICollection<WorkTimeDto>> FindAssociatedWorkTimesWithinActivity(int activityId)
    {
        var foundWorkTimes = await _context.WorkTimes
            .Include(wt => wt.Activity)
            .Include(wt => wt.Employee)
            .Include(wt => wt.WorkOrder)
            .ThenInclude(wo => wo.FirmClient)
            .Where(wt => wt.Activity.Id == activityId)
            .ToListAsync();

        return _mapper.Map<ICollection<WorkTimeDto>>(foundWorkTimes);
    }
}