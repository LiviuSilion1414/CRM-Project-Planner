using Humanizer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PlannerCRM.Server.Models;
using PlannerCRM.Shared.Dtos;

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

            if (await _context.Employees.AnyAsync(em => em.Username.ToLower().Trim().Equals(dto.email.ToLower().Trim())))
            {
                byte[] salt = new byte[128 / 8];
                string cryptedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: dto.password,
                                                                                salt: salt,
                                                                                prf: KeyDerivationPrf.HMACSHA256,
                                                                                iterationCount: 10000,
                                                                                numBytesRequested: 256 / 8));

                model.PasswordHash = cryptedPwd;

                await _context.Employees.AddAsync(model);

                var mappedRoles = _mapper.Map<List<Role>>(dto.roles);

                _context.EmployeesRoles.AddRange(mappedRoles.Select(x => new EmployeesRole() { FkIdEmployee  = model.Id, FkIdRole = x.Id }));

                await _context.SaveChangesAsync();
            }
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task Update(EmployeeDto dto)
    {
        try
        {
            var model = _mapper.Map<EmployeeDto, Employee>(dto);

            _context.Employees.Update(model);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateEmployeeRole(EmployeeFilterDto filter)
    {
        try
        {
            var existingModel = await _context.Employees
                                      .Include(x => x.EmployeesRoles)
                                      .FirstOrDefaultAsync(x => x.Id == filter.employeeId);

            if (existingModel != null)
            {
                if (filter.isRemoveRole && existingModel.EmployeesRoles.Any(x => x.FkIdRole == filter.roleId))
                {
                    _context.EmployeesRoles.Remove(existingModel.EmployeesRoles.Where(x => x.FkIdRole == filter.roleId).FirstOrDefault());
                } else
                {
                    _context.EmployeesRoles.Add(new EmployeesRole { FkIdRole = filter.roleId, /*Name = filter.role.roleName */ FkIdEmployee = filter.employeeId });
                }
                await _context.SaveChangesAsync();
            }
        }
        catch
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
                                         .SingleAsync(e => e.Id == filter.employeeId /*&& e.IsRemoveable*/);

            _context.Remove(employee);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
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
                                         .Include(e => e.EmployeesRoles)
                                         .SingleAsync(e => e.Id == filter.employeeId);

            return _mapper.Map<EmployeeDto>(employee);
        } 
        catch (Exception)
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
                                                      (filter.roleId == Guid.Empty || x.EmployeesRoles.Where(y => y.FkIdRole == filter.roleId).Any()))
                                          .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(employees);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> Search(EmployeeFilterDto filter)
    {
        try
        {
            var foundEmployee = await _context.Employees
                                              .AsNoTracking()
                                              .AsSplitQuery()
                                              .Where(em => EF.Functions.Like(em.Name, $"{filter.searchQuery}"))
                                              .Include(e => e.EmployeeActivities)
                                              .Include(e => e.EmployeesRoles)
                                              .Where(em =>
                                                  EF.Functions.Like(em.Name, $"{filter.searchQuery}") ||
                                                  EF.Functions.Like(em.Username, $"{filter.searchQuery}"))
                                              .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(foundEmployee);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeActivityDto>> FindAssociatedActivitiesByEmployeeId(EmployeeFilterDto filter)
    {
        try
        {
            var foundActivities = await _context.EmployeeActivities
                                                .AsNoTracking()
                                                .AsSplitQuery()
                                                .Include(ac => ac.FkIdActivityNavigation)
                                                .Include(ac => ac.FkIdEmployeeNavigation)
                                                .Where(x => x.FkIdEmployee == filter.employeeId)
                                                .ToListAsync();

            return _mapper.Map<List<EmployeeActivityDto>>(foundActivities);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}