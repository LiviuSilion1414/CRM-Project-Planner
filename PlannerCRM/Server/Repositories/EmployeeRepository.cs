namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository(AppDbContext dbContext,
                          DtoValidatorUtillity validator,
                          UserManager<Employee> userManager,
                          RoleManager<EmployeeRole> roleManager) : IRepository<EmployeeFormDto>, IEmployeeRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly DtoValidatorUtillity _validator = validator;
    private readonly UserManager<Employee> _userManager = userManager;
    private readonly RoleManager<EmployeeRole> _roleManager = roleManager;

    public async Task AddAsync(EmployeeFormDto dto)
    {
        var isValid = await _validator.ValidateEmployeeAsync(dto, OperationType.ADD);

        if (isValid)
        {
            var user = dto.MapToEmployee();

            await _userManager.CreateAsync(user, dto.Password);
            await _userManager.AddToRoleAsync(user, dto.Role.ToString());
        }
    }

    public async Task ArchiveAsync(int employeeId)
    {
        var isValid = await _validator.ValidateDeleteEmployeeAsync(employeeId);

        if (isValid)
        {
            var employee = await _userManager.Users
                .SingleAsync(em => em.Id == employeeId);

            employee.IsArchived = true;

            _dbContext.Update(employee);

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RestoreAsync(int employeeId)
    {
        var isValid = await _validator.ValidateDeleteEmployeeAsync(employeeId);

        if (isValid)
        {
            var employee = await _userManager.Users
                .SingleAsync(em => em.Id == employeeId);

            employee.IsArchived = false;

            _dbContext.Update(employee);

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int employeeId)
    {
        var isValid = await _validator.ValidateDeleteEmployeeAsync(employeeId);

        if (isValid)
        {
            var employee = await _userManager.Users
                .SingleAsync(em => em.Id == employeeId);

            await _userManager.DeleteAsync(employee);
        }
    }

    public async Task EditAsync(EmployeeFormDto dto)
    {
        var isValid = await _validator.ValidateEmployeeAsync(dto, OperationType.EDIT);

        if (isValid)
        {
            var user = await _userManager.FindByEmailAsync(dto.OldEmail);

            user = dto.MapToEmployee();

            var isContainedModifiedHourlyRate = await _dbContext.Users
                .AnyAsync(em => em.Salaries
                    .Any(s => s.Salary != dto.CurrentHourlyRate));

            if (!isContainedModifiedHourlyRate)
            {
                user.Salaries = dto.EmployeeSalaries
                    .Where(ems => _dbContext.Users
                        .Any(em => em.Id == ems.EmployeeId))
                    .Select(ems => ems.MapToEmployeeSalary(dto))
                    .ToList();
            }

            var userHasSamePassword = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!userHasSamePassword)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, dto.Password);
            }

            var userRole = (await _userManager.GetRolesAsync(user)).Single();
            var isInRole = await _userManager.IsInRoleAsync(user, userRole);

            if (isInRole)
            {
                await _userManager.RemoveFromRoleAsync(user, userRole);
                await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            }
        }
    }

    public async Task<EmployeeViewDto> GetForViewByIdAsync(int employeeId)
    {
        return await _userManager.Users
            .Where(em => em.Id == employeeId)
            .Select(em => em.MapToEmployeeViewDto())
            .SingleAsync();
    }

    public async Task<EmployeeSelectDto> GetForRestoreAsync(int employeeId)
    {
        return await _userManager.Users
            .Where(em => em.Id == employeeId)
            .Select(em => em.MapToEmployeeSelectDto())
            .SingleAsync();
    }

    public async Task<EmployeeFormDto> GetForEditByIdAsync(int employeeId)
    {
        var employee = await _userManager.Users
            .Where(em => em.Id == employeeId)
            .Select(em => em.MapToEmployeeFormDto())
            .SingleAsync();
        var employeeSalaries = await GetEmployeeSalariesAsync(employeeId);
        
        employee.StartDateHourlyRate = employeeSalaries.Single().StartDate;
        employee.FinishDateHourlyRate = employeeSalaries.Single().FinishDate;
        employee.EmployeeSalaries = new(employeeSalaries);

        return employee;
    }

    public async Task<List<EmployeeSalaryDto>> GetEmployeeSalariesAsync(int employeeId)
    {
        return await _dbContext.EmployeeSalaries
            .Where(ems => ems.EmployeeId == employeeId)
            .Select(ems => ems.MapToEmployeeSalaryDto())
            .ToListAsync();
    }

    public async Task<EmployeeDeleteDto> GetForDeleteByIdAsync(int employeeId)
    {
        var employee = await _userManager.Users
            .Where(em => em.Id == employeeId)
            .Select(em => em.MapToEmployeeDeleteDto(employeeId))
            .SingleAsync();
        var employeeActivities = await GetEmployeeActivitiesByEmployeeIdAsync(employeeId);
        employee.EmployeeActivities = new(employeeActivities);

        return employee;
    }

    public async Task<List<EmployeeActivityDto>> GetEmployeeActivitiesByEmployeeIdAsync(int employeeId)
    {
        var employee = await _userManager.Users
            .SingleAsync(em => em.Id == employeeId);

        var activity = await _dbContext.EmployeeActivities
            .Where(ea => ea.EmployeeId == employeeId)
            .ToListAsync();

        return activity
            .Select(ea => ea.MapToEmployeeActivityDto())
            .ToList();
    }

    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email)
    {
        return await _userManager.Users
            .Where(em => !em.IsDeleted && !em.IsArchived)
            .Where(em => EF.Functions.ILike(em.FullName, $"%{email}%") ||
                EF.Functions.ILike(em.Email, $"%{email}%"))
            .Select(em => em.MapToEmployeeSelectDto())
            .ToListAsync();
    }

    public async Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int offset, int limit)
    {
        return await _userManager.Users
            .OrderBy(em => em.Id)
            .Skip(offset)
            .Take(limit)
            .Select(employee => employee.MapToEmployeeViewDto())
            .ToListAsync();
    }

    public async Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email)
    {
        return await _userManager.Users
            .Where(em => !em.IsDeleted && !em.IsArchived && em.Email == email)
            .Select(em => em.MapToCurrentEmployeeDto())
            .SingleAsync();
    }

    public async Task<int> GetEmployeesSizeAsync() => await _userManager.Users.CountAsync();

    public Task<List<EmployeeSalaryDto>> GetEmployeeActivitiesAsync(int employeeId)
    {
        throw new NotImplementedException();
    }
}