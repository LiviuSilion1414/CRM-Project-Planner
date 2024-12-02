namespace PlannerCRM.Server.Repositories.Specific;

public class ActivityRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ActivityDto> SearchActivityByTitle(string title)
    {
        var foundItem = await _context.Activities
            .SingleOrDefaultAsync(ac => EF.Functions.ILike(ac.Name, $"{title}"));

        return _mapper?.Map<ActivityDto>(foundItem);
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
            .Include(wt => wt.WorkOrder)
            .Include(wt => wt.Activity)
            .Include(wt => wt.Employee)
            .Where(wt => wt.Activity.Id == activityId)
            .ToListAsync();

        return _mapper.Map<ICollection<WorkTimeDto>>(foundWorkTimes);
    }
}