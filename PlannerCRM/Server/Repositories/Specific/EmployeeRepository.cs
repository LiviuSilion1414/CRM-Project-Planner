namespace PlannerCRM.Server.Repositories.Specific;

public class EmployeeRepository(AppDbContext context, IMapper mapper)
    : Repository<Employee, EmployeeDto>(context, mapper), IRepository<Employee, EmployeeDto>
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public override async Task AddAsync(Employee model)
    {
        await base.AddAsync(model);

        foreach (var role in model.Roles)
        {
            await _context.EmployeeRoles.AddAsync(
                new EmployeeRole()
                {
                    EmployeeId = model.Id,
                    RoleId = role.Id,
                }
            );
        }

        foreach (var salary in model.Salaries)
        {
            await _context.EmployeeSalaries.AddAsync(
                new EmployeeSalary()
                {
                    EmployeeId = model.Id,
                    SalaryId = salary.Id,
                }
            );
        }

        await _context.SaveChangesAsync();
    }

    public override async Task EditAsync(Employee model, int id)
    {
        await base.EditAsync(model, id);

        var existingEmployeeRoles = await _context.EmployeeRoles
            .Where(e => e.EmployeeId == model.Id)
            .ToListAsync();

        List<EmployeeRole> updatedEmployeeRoles = [];

        foreach (var role in model.Roles)
        {
            if (!existingEmployeeRoles.Any(ex => ex.RoleId == role.Id))
            {
                updatedEmployeeRoles.Add(
                    new EmployeeRole {
                        EmployeeId = model.Id,
                        RoleId = role.Id,
                        Name = nameof(role.RoleName), //may be an issue
                    }
                );
            }            
        }

        _context.Update(updatedEmployeeRoles);

        await _context.SaveChangesAsync();
    }

    public override async Task DeleteAsync(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.Roles)
            .SingleAsync(e => e.Id == id);

        _context.Remove(employee);

        await _context.SaveChangesAsync();
    }

    public override async Task<EmployeeDto> GetByIdAsync(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.Roles)
            .SingleAsync(e => e.Id == id);

        return _mapper.Map<EmployeeDto>(employee);
    }

    public override async Task<ICollection<EmployeeDto>> GetWithPagination(int offset, int limit)
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

        return _mapper.Map<ICollection<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto> SearchEmployeeByName(string employeeName)
    {
        var foundEmployee = await _context.Employees
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.Roles)
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