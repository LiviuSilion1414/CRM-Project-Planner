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

                var mappedRoles = _mapper.Map<List<Role>>(dto.employeesRoles);

                if (!mappedRoles.Where(x => x.Id == Guid.Parse(BaseRoles.User.GetDescription())).Any())
                {
                    mappedRoles.Add(new Role()
                    {
                        Id = Guid.Parse(BaseRoles.User.GetDescription()),
                        Name = BaseRoles.User.ToString(),
                        IsRemoveable = false
                    });
                }


                _context.EmployeesRoles.AddRange(mappedRoles.Select(x => new EmployeesRole() { FkIdEmployee  = model.Id, FkIdRole = x.Id }));

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

            if (model != null)
            {
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
            }
        } catch (Exception)
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
                if ((bool)filter.isRemoveRole && existingModel.EmployeesRoles.Any(x => x.FkIdRole == filter.role.id))
                {
                    _context.EmployeesRoles.Remove(existingModel.EmployeesRoles.Where(x => x.FkIdRole == filter.role.id).FirstOrDefault());
                } else
                {
                    _context.EmployeesRoles.Add(new EmployeesRole { FkIdRole = (Guid)filter.role.id, FkIdEmployee = (Guid)filter.employeeId });
                }
                await _context.SaveChangesAsync();
            }
        } catch
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
                                                      (filter.roleId == null || filter.roleId == Guid.Empty || x.EmployeesRoles.Where(y => y.FkIdRole == filter.roleId).Any()))
                                          .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(employees);
        } catch (Exception)
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
        } catch (Exception)
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
                                                .Include(ac => ac.FkIdActivityNavigation).ThenInclude(x => x.FkIdWorkOrderNavigation)
                                                .Include(ac => ac.FkIdEmployeeNavigation).ThenInclude(x => x.EmployeesRoles)
                                                .Where(x => x.FkIdEmployee == filter.id)
                                                .ToListAsync();
            var config = _mapper.ConfigurationProvider;
            config.AssertConfigurationIsValid();
            return _mapper.Map<List<EmployeeActivityDto>>(foundActivities);
        } catch (Exception)
        {
            throw;
        }
    }
}