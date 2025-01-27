namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(EmployeeDto dto)
    {
        var model = _mapper.Map<Employee>(dto);

        await _context.Employees.AddAsync(model);

        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(EmployeeDto dto)
    {
        var model = _mapper.Map<Employee>(dto);

        var existingModel = await _context.Employees.SingleAsync(em => em.Id == model.Id);
        existingModel = model;

        _context.Update(existingModel);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(EmployeeDto dto)
    {
        var employee = await _context.Employees
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.Roles)
            .SingleAsync(e => e.Id == dto.Id);

        _context.Remove(employee);

        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.Roles)
            .SingleAsync(e => e.Id == id);

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<List<EmployeeDto>> GetWithPagination(int offset, int limit)
    {
        var employees = await _context.Employees
            .OrderBy(e => e.Id)
            .Skip(offset)
            .Take(limit)
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.Roles)
            .ToListAsync();

        return _mapper.Map<List<EmployeeDto>>(employees);
    }

    public async Task<List<EmployeeDto>> SearchEmployeeByName(string employeeName)
    {
        var foundEmployee = await _context.Employees
            .Where(em => EF.Functions.ILike(em.Name, $"{employeeName}"))
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.Roles)
            .ToListAsync();

        return _mapper.Map<List<EmployeeDto>>(foundEmployee);
    }

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByEmployeeId(int employeeId)
    {
        var foundActivities = await _context.Activities
            .Include(ac => ac.EmployeeActivities)
            .Include(ac => ac.ActivityWorkTimes)
            .Include(ac => ac.Employees)
            .Where(ac => ac.Employees.Any(em => em.Id == employeeId))
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(foundActivities);
    }

    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesByActivityIdAndEmployeeId(int employeeId, int activityId)
    {
        var foundWorkTimes = await _context.WorkTimes
            .Include(wt => wt.WorkOrder)
            .Include(wt => wt.Activity)
            .Include(wt => wt.ActivityWorkTimes)
            .Include(wt => wt.Employee)
            .Where(wt => wt.EmployeeId == employeeId && wt.ActivityId == activityId)
            .ToListAsync();

        return _mapper.Map<List<WorkTimeDto>>(foundWorkTimes);
    }

    public async Task<List<SalaryDto>> FindAssociatedSalaryDataByEmployeeId(int employeeId)
    {
        var foundSalaries = await _context.Salaries
            .Include(em => em.EmployeeSalaries)
            .Include(em => em.Employee)
            .Where(sl => sl.EmployeeId == employeeId)
            .ToListAsync();

        return _mapper.Map<List<SalaryDto>>(foundSalaries);
    }
}