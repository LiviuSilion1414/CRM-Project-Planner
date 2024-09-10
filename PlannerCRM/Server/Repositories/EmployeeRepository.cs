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

            var userHasSamePassword = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!userHasSamePassword)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, dto.Password);
                user.Password = dto.Password;
            }

            var userRole = (await _userManager.GetRolesAsync(user)).Single();
            var isInRole = await _userManager.IsInRoleAsync(user, userRole);

            if (isInRole)
            {
                await _userManager.RemoveFromRoleAsync(user, userRole);
                await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            }

            user.Email = dto.Email;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.Email;
            user.NormalizedEmail = dto.Email.ToUpper();
            user.FullName = $"{dto.FirstName} {dto.LastName}";
            user.BirthDay = dto.BirthDay ?? default;
            user.StartDate = dto.StartDate ?? default;
            user.Password = dto.Password;
            user.NumericCode = dto.NumericCode;
            user.Role = dto.Role ?? default;
            user.CurrentHourlyRate = dto.CurrentHourlyRate;
            user.Salaries = dto.EmployeeSalaries
                .Select(ems => ems.MapToEmployeeSalary(dto))
                .ToList();

            await _userManager.UpdateAsync(user);
        }
    }

    public async Task<EmployeeViewDto> GetForViewByIdAsync(int employeeId)
    {
        var employee = await _userManager.Users
            .Where(em => em.Id == employeeId)
            .SingleAsync();

        return employee.MapToEmployeeViewDto();
    }

    public async Task<EmployeeSelectDto> GetForRestoreAsync(int employeeId)
    {
        var employee = await _userManager.Users
            .Where(em => em.Id == employeeId)
            .SingleAsync();

        return employee.MapToEmployeeSelectDto();
    }

    public async Task<EmployeeFormDto> GetForEditByIdAsync(int employeeId)
    {
        var employee = await _userManager.Users
            .SingleAsync(em => em.Id == employeeId);

        var employeeSalaries = await GetEmployeeSalariesAsync(employeeId);

        var mappedEmployee = employee.MapToEmployeeFormDto();

        mappedEmployee.StartDateHourlyRate = employeeSalaries.SingleOrDefault().StartDate;
        mappedEmployee.FinishDateHourlyRate = employeeSalaries.SingleOrDefault().FinishDate;
        mappedEmployee.EmployeeSalaries = new(employeeSalaries);

        return mappedEmployee;
    }

    public async Task<List<EmployeeSalaryDto>> GetEmployeeSalariesAsync(int employeeId)
    {
        var employeesSalaries = await _dbContext.EmployeeSalaries
            .Where(ems => ems.EmployeeId == employeeId)
            .ToListAsync();

        return employeesSalaries
            .Select(em => em.MapToEmployeeSalaryDto())
            .ToList();
    }

    public async Task<EmployeeDeleteDto> GetForDeleteByIdAsync(int employeeId)
    {
        var employee = await _userManager.Users
            .Where(em => em.Id == employeeId)
            .SingleAsync();
        var employeeActivities = await GetEmployeeActivitiesByEmployeeIdAsync(employeeId);
        
        var mappedEmployee = employee.MapToEmployeeDeleteDto(employeeId);
        mappedEmployee.EmployeeActivities = new(employeeActivities);

        return mappedEmployee;
    }

    public async Task<List<EmployeeActivityDto>> GetEmployeeActivitiesByEmployeeIdAsync(int employeeId)
    {
        var employeeActivities = await _dbContext.EmployeeActivities
            .Where(ea => ea.EmployeeId == employeeId)
            .ToListAsync();

        var employee = await _userManager.Users
            .SingleAsync(em => em.Id == employeeId);
    
        var activities = await _dbContext.Activities
            .Where(ac => _dbContext.EmployeeActivities.Any(ea => ea.ActivityId == ac.Id))
            .ToListAsync();

        foreach (var ea in employeeActivities)
        {
            ea.Employee = employee;
            foreach (var ac in activities) 
            {
                if (ea.ActivityId == ac.Id) 
                { 
                    ea.Activity = ac;
                }
            }
        }

        return employeeActivities
            .Select(ea => ea.MapToEmployeeActivityDto())
            .ToList();
    }

    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email)
    {
        var employees = await _userManager.Users
            .Where(em => !em.IsDeleted && !em.IsArchived)
            .Where(em => EF.Functions.ILike(em.FullName, $"%{email}%") ||
                EF.Functions.ILike(em.Email, $"%{email}%"))
            .ToListAsync();
        
        return employees
            .Select(em => em.MapToEmployeeSelectDto())
            .ToList();
    }

    public async Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int offset, int limit)
    {
        var employees = await _userManager.Users
            .OrderBy(em => em.Id)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        var resultList = employees
            .Select(employees => employees.MapToEmployeeViewDto())
            .ToList();

        foreach (var em in resultList)
        {
            em.StartDateHourlyRate = (await _dbContext.EmployeeSalaries
                .SingleAsync(ems => em.Id == ems.EmployeeId))
                .StartDate;
            em.FinishDateHourlyRate = (await _dbContext.EmployeeSalaries
                .SingleAsync(ems => em.Id == ems.EmployeeId))
                .FinishDate;
        }

        return resultList;
    }

    public async Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email)
    {
        var employee = await _userManager.Users
            .Where(em => !em.IsDeleted && !em.IsArchived && em.Email == email)
            .SingleAsync();

        return employee.MapToCurrentEmployeeDto();
    }

    public async Task<int> GetEmployeesSizeAsync() => await _userManager.Users.CountAsync();
}