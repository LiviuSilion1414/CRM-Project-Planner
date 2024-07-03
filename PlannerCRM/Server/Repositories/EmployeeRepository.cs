namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository(
    AppDbContext dbContext,
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
            await _dbContext.Users.AddAsync(dto.MapToEmployee());

            await _dbContext.SaveChangesAsync();

            await AddUserAsync(dto);
            await SetRoleAsync(dto.Email, dto.Role ?? throw new ArgumentNullException(nameof(dto.Role), ExceptionsMessages.NULL_ARG));
            await SetProfilePictureAsync(dto);
            await SetFKProfilePictureIdAsync(dto.Email);
        }
    }

    private async Task AddUserAsync(EmployeeFormDto dto)
    {
        var user = new Employee
        {
            Email = dto.Email,
            UserName = dto.Email,
            NormalizedEmail = dto.Email.ToUpper()
        };

        await _userManager.CreateAsync(user, dto.Password);
    }

    private async Task SetRoleAsync(string email, Roles role)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is not null)
        {
            await _userManager.AddToRoleAsync(user, role.ToString());
        }
    }

    private async Task SetProfilePictureAsync(EmployeeFormDto dto)
    {
        var employee = await _dbContext.Users
            .SingleAsync(em => em.Email == dto.Email);

        await _dbContext.ProfilePictures.AddAsync(dto.MapToEmployeeProfilePicture(employee));

        await _dbContext.SaveChangesAsync();
    }

    private async Task SetFKProfilePictureIdAsync(string employeeEmail)
    {
        var employee = await _dbContext.Users
            .SingleAsync(em => em.Email == employeeEmail);

        var profilePic = await _dbContext.ProfilePictures
            .SingleAsync(pic => pic.EmployeeInfo.Email == employeeEmail);

        employee.ProfilePictureId = profilePic.Id;

        _dbContext.Update(employee);

        await _dbContext.SaveChangesAsync();
    }

    public async Task ArchiveAsync(int employeeId)
    {
        var isValid = await _validator.ValidateDeleteEmployeeAsync(employeeId);

        if (isValid)
        {
            var employee = await _dbContext.Users
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
            var employee = await _dbContext.Users
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
            var employee = await _dbContext.Users
                .SingleAsync(em => em.Id == employeeId);
            _dbContext.Users.Remove(employee);
        }
    }

    public async Task EditAsync(EmployeeFormDto dto)
    {
        var isValid = await _validator.ValidateEmployeeAsync(dto, OperationType.EDIT);

        if (isValid)
        {
            var user = await _userManager.FindByEmailAsync(dto.OldEmail);
            var model = await _dbContext.Users.SingleAsync(em => em.Id == dto.Id);

            model = dto.MapToEmployee();

            var isContainedModifiedHourlyRate = await _dbContext.Users
                .AnyAsync(em => em.Salaries
                    .Any(s => s.Salary != dto.CurrentHourlyRate));

            if (!isContainedModifiedHourlyRate)
            {
                model.Salaries = dto.EmployeeSalaries
                    .Where(ems => _dbContext.Users
                        .Any(em => em.Id == ems.EmployeeId))
                    .Select(ems => ems.MapToEmployeeSalary(dto))
                    .ToList();
            }

            _dbContext.Users.Update(model);

            user.Email = dto.Email;
            user.NormalizedEmail = dto.Email.ToUpper();
            user.UserName = dto.Email;

            var passChangeResult = await _userManager.RemovePasswordAsync(user);
            var updateResult = await _userManager.AddPasswordAsync(user, dto.Password);

            if (!passChangeResult.Succeeded || !updateResult.Succeeded)
            {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }

            var rolesList = await _userManager.GetRolesAsync(user);
            var userRole = rolesList
                .SingleOrDefault()
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_PARAM);

            var isInRole = await _userManager.IsInRoleAsync(user, userRole);

            if (isInRole)
            {
                var deleteRoleResult = await _userManager.RemoveFromRoleAsync(user, userRole);
                var reassignmentRoleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());

                if (!deleteRoleResult.Succeeded || !reassignmentRoleResult.Succeeded)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            }

            await _dbContext.SaveChangesAsync();

            await SetProfilePictureAsync(dto);
            await SetFKProfilePictureIdAsync(dto.Email);
        }
    }

    public async Task<EmployeeViewDto> GetForViewByIdAsync(int employeeId)
    {
        return await _dbContext.Users
            .Select(em => em.MapToEmployeeViewDto(_dbContext))
            .SingleAsync(em => em.Id == employeeId);
    }

    public async Task<EmployeeSelectDto> GetForRestoreAsync(int employeeId)
    {
        return await _dbContext.Users
            .Select(em => em.MapToEmployeeSelectDto())
            .SingleAsync(em => em.Id == employeeId);
    }

    public async Task<EmployeeFormDto> GetForEditByIdAsync(int employeeId)
    {
        return await _dbContext.Users
            .Where(em => !em.IsDeleted || !em.IsArchived)
            .Select(em => em.MapToEmployeeFormDto(_dbContext))
            .SingleAsync(em => em.Id == employeeId);
    }

    public async Task<EmployeeDeleteDto> GetForDeleteByIdAsync(int employeeId)
    {
        return await _dbContext.Users
            .Where(em => (!em.IsDeleted || !em.IsArchived) && em.Id == employeeId)
            .Select(em => em.MapToEmployeeDeleteDto(employeeId, _dbContext))
            .SingleAsync(em => em.Id == employeeId);
    }

    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email)
    {
        return await _dbContext.Users
            .Where(em => !em.IsDeleted && !em.IsArchived)
            .Where(em => EF.Functions.ILike(em.FullName, $"%{email}%") ||
                EF.Functions.ILike(em.Email, $"%{email}%"))
            .Select(em => em.MapToEmployeeSelectDto())
            .ToListAsync();
    }

    public async Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int limit, int offset)
    {
        return await _dbContext.Users
            .OrderBy(em => em.Id)
            .Skip(offset)
            .Take(limit )
            .Select(em => em.MapToEmployeeViewDto(_dbContext))
            .ToListAsync();
    }

    public async Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email)
    {
        return await _dbContext.Users
            .Where(em => !em.IsDeleted && !em.IsArchived)
            .Select(em => em.MapToCurrentEmployeeDto())
            .FirstAsync(em => em.Email == email);
    }

    public async Task<int> GetEmployeesSizeAsync() => await _dbContext.Users.CountAsync();
}