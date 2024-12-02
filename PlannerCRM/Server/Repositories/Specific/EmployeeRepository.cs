namespace PlannerCRM.Server.Repositories.Specific;

public class EmployeeRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<EmployeeDto> SearchEmployeeByName(string employeeName)
    {
        var foundEmployee = await _context.Employees
            .SingleOrDefaultAsync(em => EF.Functions.ILike(em.Name, $"{employeeName}"));

        return _mapper.Map<EmployeeDto>(foundEmployee);
    }

    public async Task<ICollection<ActivityDto>> FindAssociatedActivitiesByEmployeeId(int employeeId)
    {
        var foundActivities = await _context.Activities
            .Include(ac => ac.EmployeeActivities)
            .Include(ac => ac.ActivityWorkTimes)
            .Include(ac => ac.Employees)
            .Where(ac => ac.Employees.Any(em => em.Id == employeeId))
            .ToListAsync();

        return _mapper.Map<ICollection<ActivityDto>>(foundActivities);
    }

    public async Task<ICollection<WorkTimeDto>> FindAssociatedWorkTimesByActivityIdAndEmployeeId(int employeeId, int activityId)
    {
        var foundWorkTimes = await _context.WorkTimes
            .Include(wt => wt.WorkOrder)
            .Include(wt => wt.Activity)
            .Include(wt => wt.ActivityWorkTimes)
            .Include(wt => wt.Employee)
            .Where(wt => wt.EmployeeId == employeeId && wt.ActivityId == activityId)
            .ToListAsync();

        return _mapper.Map<ICollection<WorkTimeDto>>(foundWorkTimes);
    }

    public async Task<ICollection<SalaryDto>> FindAssociatedSalaryDataByEmployeeId(int employeeId)
    {
        var foundSalaries = await _context.Salaries
            .Include(em => em.EmployeeSalaries)
            .Include(em => em.Employee)
            .Where(sl => sl.EmployeeId == employeeId)
            .ToListAsync();

        return _mapper.Map<ICollection<SalaryDto>>(foundSalaries);
    }
}