using Humanizer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PdfSharp.Pdf.Filters;

namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(FilterDto filter)
    {
        try
        {
            var model = _mapper.Map<Employee>((EmployeeDto)filter.Data);

            if ((await _context.Employees.SingleOrDefaultAsync(em => em.Email == ((EmployeeDto)filter.Data).Email)) == null)
            {
                byte[] salt = new byte[128 / 8];
                string cryptedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: ((EmployeeDto)filter.Data).Password,
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
        catch (Exception)
        {
            throw;
        }
    }

    //public async Task AssignRole(FilterDto filter)
    //{
    //    try
    //    {
    //        if (!(await _context.Roles.Where(x => filter.Roles.Any(y => x.RoleName.Contains(y))).AnyAsync()))
    //        {
    //            foreach (var role in filter.Roles)
    //            {
    //                await _context.Roles.AddAsync(new Role { RoleName = role });
    //            }

    //            await _context.SaveChangesAsync();
    //        }

    //        var roles = new List<Role>();

    //        foreach (var role in filter.Roles)
    //        {
    //            await _context.Roles
    //                          .Include(x => x.EmployeeRoles)
    //                          .Where(x => x.RoleName == role)
    //                          .ToListAsync();
    //        }

    //        foreach (var role in roles)
    //        {
    //            await _context.EmployeeRoles.AddAsync(
    //                new EmployeeRole
    //                {
    //                    RoleName = role.RoleName,
    //                    RoleId = role.Guid,
    //                    EmployeeId = roles.First().EmployeeRoles.First().EmployeeId
    //                }
    //            );
    //        }

    //        await _context.SaveChangesAsync();
    //    } 
    //    catch
    //    {
    //        throw;
    //    }
    //}

    public async Task EditAsync(FilterDto filter)
    {
        try
        {
            var model = _mapper.Map<Employee>((EmployeeDto)filter.Data);

            var existingModel = await _context.Employees.SingleAsync(em => em.Guid == model.Guid);
            existingModel = model;

            _context.Update(existingModel);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(FilterDto filter)
    {
        try
        {
            var employee = await _context.Employees
                                         .Include(e => e.Activities)
                                         .Include(e => e.WorkTimes)
                                         .Include(e => e.Salaries)
                                         .Include(e => e.EmployeeRoles)
                                         .SingleAsync(e => e.Guid == filter.Id);

            _context.Remove(employee);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmployeeDto> GetByIdAsync(FilterDto filter)
    {
        try
        {
            var employee = await _context.Employees
                                         .Include(e => e.Activities)
                                         .Include(e => e.WorkTimes)
                                         .Include(e => e.Salaries)
                                         .Include(e => e.EmployeeRoles)
                                         .SingleAsync(e => e.Guid == filter.Id);

            return _mapper.Map<EmployeeDto>(employee);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> GetWithPagination(FilterDto filter)
    {
        try
        {
            var employees = await _context.Employees
                                          .OrderBy(e => e.Guid)
                                          .Include(e => e.EmployeeRoles)
                                          .Include(e => e.Activities)
                                          .Include(e => e.WorkTimes)
                                          .Include(e => e.Salaries)
                                          .Include(e => e.EmployeeRoles)
                                          .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(employees);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> SearchEmployeeByName(FilterDto filter)
    {
        try
        {
            var foundEmployee = await _context.Employees
                                              .Where(em => EF.Functions.ILike(em.Name, $"{filter.SearchQuery}"))
                                              .Include(e => e.Activities)
                                              .Include(e => e.WorkTimes)
                                              .Include(e => e.Salaries)
                                              .Include(e => e.EmployeeRoles)
                                              .Where(em =>
                                                  EF.Functions.ILike(em.Name, $"{filter.SearchQuery}") ||
                                                  EF.Functions.ILike(em.Email, $"{filter.SearchQuery}"))
                                              .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(foundEmployee);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByEmployeeId(FilterDto filter)
    {
        try
        {
            var foundActivities = await _context.Activities
                                                .Include(ac => ac.EmployeeActivities)
                                                .Include(ac => ac.ActivityWorkTimes)
                                                .Include(ac => ac.Employees)
                                                .Where(ac => ac.Employees.Any(em => em.Guid == filter.Id))
                                                .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(foundActivities);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<WorkTimeDto>> FindAssociatedWorkTimesByActivityIdAndEmployeeId(FilterDto filter)
    {
        try
        {
            var foundWorkTimes = await _context.WorkTimes
                                               .Include(wt => wt.WorkOrder)
                                               .Include(wt => wt.Activity)
                                               .Include(wt => wt.ActivityWorkTimes)
                                               .Include(wt => wt.Employee)
                                               .Where(wt => wt.EmployeeId == filter.Id && wt.ActivityId ==  filter.Guid2) //employeeid and worktimeid
                                               .ToListAsync();

            return _mapper.Map<List<WorkTimeDto>>(foundWorkTimes);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<SalaryDto>> FindAssociatedSalaryDataByEmployeeId(FilterDto filter)
    {
        try
        {
            var foundSalaries = await _context.Salaries
                                              .Include(em => em.EmployeeSalaries)
                                              .Include(em => em.Employee)
                                              .Where(sl => sl.EmployeeId == filter.Id)
                                              .ToListAsync();

            return _mapper.Map<List<SalaryDto>>(foundSalaries);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}