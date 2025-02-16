using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(EmployeeDto dto)
    {
        var model = _mapper.Map<Employee>(dto);

        if ((await _context.Employees.SingleOrDefaultAsync(em => em.Email == dto.Email)) == null)
        {
            byte[] salt = new byte[128 / 8];
            string cryptedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: dto.Password,
                                                                            salt: salt,
                                                                            prf: KeyDerivationPrf.HMACSHA256,
                                                                            iterationCount: 10000,
                                                                            numBytesRequested: 256 / 8));

            model.PasswordHash = cryptedPwd;
            
            await _context.Employees.AddAsync(model);
            await _context.SaveChangesAsync();
            
            if (model.EmployeeRoles is not null && model.Salaries is not null)
            {
                foreach (var role in model.EmployeeRoles)
                {
                    await _context.EmployeeRoles.AddAsync(
                        new EmployeeRole()
                        {
                            RoleId = role.Guid,
                            EmployeeId = model.Guid
                        }
                    );
                }

                foreach (var salary in model.Salaries)
                {
                    await _context.EmployeeSalaries.AddAsync(
                        new EmployeeSalary()
                        {
                            SalaryId = salary.Guid,
                            EmployeeId = model.Guid
                        }
                    );
                }
            }
        }


        await _context.SaveChangesAsync();
    }

    public async Task AssignRole(EmployeeDto dto, string role)
    {
        try
        {
            if (!(await _context.Roles.AnyAsync(x => x.RoleName.Contains(role))))
            {
                var roleCreated = new Role()
                {
                    RoleName = role,
                };
                
                await _context.Roles.AddAsync(roleCreated);

                await _context.SaveChangesAsync();

                await _context.EmployeeRoles.AddAsync(
                    new EmployeeRole 
                    {
                        RoleName = role,
                        RoleId = roleCreated.Guid,
                        EmployeeId = dto.Guid
                    }
                );

                await _context.SaveChangesAsync();
            }
        } catch
        {
            throw;
        }
    }
    public async Task EditAsync(EmployeeDto dto)
    {
        var model = _mapper.Map<Employee>(dto);

        var existingModel = await _context.Employees.SingleAsync(em => em.Guid == model.Guid);
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
            .Include(e => e.EmployeeRoles)
            .SingleAsync(e => e.Guid == dto.Guid);

        _context.Remove(employee);

        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeDto> GetByIdAsync(Guid id)
    {
        var employee = await _context.Employees
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.EmployeeRoles)
            .SingleAsync(e => e.Guid == id);

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<List<EmployeeDto>> GetWithPagination(int limit, int offset)
    {
        var employees = await _context.Employees
            .OrderBy(e => e.Guid)
            .Skip(offset)
            .Take(limit)
            .Include(e => e.EmployeeRoles)
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.EmployeeRoles)
            .ToListAsync();

        var mapped = _mapper.Map<List<EmployeeDto>>(employees);

        return mapped;
    }

    public async Task<List<EmployeeDto>> SearchEmployeeByName(string employeeName)
    {
        var foundEmployee = await _context.Employees
            .Where(em => EF.Functions.ILike(em.Name, $"{employeeName}"))
            .Include(e => e.Activities)
            .Include(e => e.WorkTimes)
            .Include(e => e.Salaries)
            .Include(e => e.EmployeeRoles)
            .ToListAsync();

        return _mapper.Map<List<EmployeeDto>>(foundEmployee);
    }

    public async Task<List<EmployeeLoginRecoveryDto>> SearchEmployeeByName(string employeeName, string email = "", string phone="")
    {
        var foundEmployee = await _context.Employees
            .Where(em => 
                EF.Functions.ILike(em.Name, $"{employeeName}") || 
                EF.Functions.ILike(em.Email, $"{email}"))
            .ToListAsync();

        return _mapper.Map<List<EmployeeLoginRecoveryDto>>(foundEmployee);
    }

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByEmployeeId(Guid employeeId)
    {
        var foundActivities = await _context.Activities
            .Include(ac => ac.EmployeeActivities)
            .Include(ac => ac.ActivityWorkTimes)
            .Include(ac => ac.Employees)
            .Where(ac => ac.Employees.Any(em => em.Guid == employeeId))
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(foundActivities);
    }

    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesByActivityIdAndEmployeeId(Guid employeeId, Guid activityId)
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

    public async Task<List<SalaryDto>> FindAssociatedSalaryDataByEmployeeId(Guid employeeId)
    {
        var foundSalaries = await _context.Salaries
            .Include(em => em.EmployeeSalaries)
            .Include(em => em.Employee)
            .Where(sl => sl.EmployeeId == employeeId)
            .ToListAsync();

        return _mapper.Map<List<SalaryDto>>(foundSalaries);
    }
}