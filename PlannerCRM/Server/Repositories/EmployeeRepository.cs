using Humanizer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.JSInterop.Infrastructure;
using PlannerCRM.Server.Models;
using PlannerCRM.Shared.Dtos;
using static PlannerCRM.Shared.DtoShared;

namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(EmployeeDto dto)
    {
        try
        {
            var model = _mapper.Map<Employee>(dto);

            if (!await _context.Employees.AnyAsync(em => em.Username.ToLower().Trim().Equals(dto.username.ToLower().Trim())))
            {
                model.PasswordHash = CryptPassword(dto.newPassword);
                model.LastSeen = DateTime.Now;
                model.CreationDate = DateTime.Now;

                await _context.Employees.AddAsync(model);
                await _context.SaveChangesAsync();

                var userRole = new EmployeesRole()
                {
                    FkIdRole = Guid.Parse(BaseRoles.User.GetDescription()),
                    FkIdEmployee = model.Id,
                    IsRemoveable = false
                };

                await _context.EmployeesRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
            }
        } catch (Exception)
        {
            throw;
        }
    }

    private static string CryptPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        string cryptedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password,
                                                                        salt: salt,
                                                                        prf: KeyDerivationPrf.HMACSHA256,
                                                                        iterationCount: 10000,
                                                                        numBytesRequested: 256 / 8));
        return cryptedPwd;
    }

    public async Task Update(EmployeeDto dto)
    {
        try
        {
            var model = await _context.Employees
                                      .FirstOrDefaultAsync(x => x.Id == (Guid)dto.id);

            if (model == null) return;

            model.Name = dto.name;
            model.Surname = dto.surname;
            model.Username = dto.username;
            model.IsRemoveable = (bool)dto.isRemoveable;

            if (!string.IsNullOrEmpty(dto.newPassword))
            {
                model.PasswordHash = CryptPassword(dto.newPassword);
            }

            _context.Update(model);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {
            throw;
        }
    }


    public async Task Delete(EmployeeFilterDto filter)
    {
        try
        {
            var employee = await _context.Employees
                                         .Include(e => e.EmployeeActivities)
                                         .Include(e => e.EmployeesRoles)
                                         .Include(e => e.WorkTimes)
                                         .FirstOrDefaultAsync(e => e.Id == filter.employeeId);

            _context.Remove(employee);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmployeeDto> Get(EmployeeFilterDto filter)
    {
        try
        {
            var employee = await _context.Employees
                                         .AsNoTracking()
                                         .AsSplitQuery()
                                         .Include(e => e.EmployeeActivities)
                                         .Include(e => e.EmployeesRoles).ThenInclude(x => x.FkIdRoleNavigation)
                                         .SingleAsync(e => e.Id == filter.employeeId);

            return _mapper.Map<EmployeeDto>(employee);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> List(EmployeeFilterDto filter)
    {
        try
        {
            var employees = await _context.Employees
                                          .AsNoTracking()
                                          .AsSplitQuery()
                                          .OrderBy(e => e.Id)
                                          .Include(e => e.EmployeesRoles)
                                          .Include(e => e.EmployeeActivities)
                                          .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery)) &&
                                                      (filter.roleId == null || filter.roleId == Guid.Empty) || (filter.employeeId == x.Id))
                                          .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(employees);
        } catch (Exception)
        {
            throw;
        }
    }
}