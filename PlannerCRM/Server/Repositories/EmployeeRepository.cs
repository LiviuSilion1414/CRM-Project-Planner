using Humanizer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PdfSharp.Pdf.Filters;

namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(EmployeeDto dto)
    {
        try
        {
            var model = _mapper.Map<Employee>(dto);

            if ((await _context.Employees.SingleOrDefaultAsync(em => em.Email.ToLower().Trim().Equals(dto.email.ToLower().Trim()) || 
                                                                     em.Name.ToLower().Trim().Equals(dto.email.ToLower().Trim()) ||
                                                                     em.Phone.ToLower().Trim().Equals(dto.phone.ToLower().Trim()))) == null)
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

                _context.EmployeeRoles.AddRange(mappedRoles.Select(x => new EmployeeRole() { EmployeeId = model.Id, RoleId = x.Id }));

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
            var model = _mapper.Map<Employee>(dto);
            
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
                                      .Include(x => x.EmployeeRoles)
                                      .SingleOrDefaultAsync(x => x.Id == filter.employeeId);

            if (existingModel != null)
            {
                if (filter.isRemoveRole)
                {
                    existingModel.EmployeeRoles.Remove(existingModel.EmployeeRoles.Single(x => x.RoleId == filter.roleId));
                }
                else
                {
                    existingModel.EmployeeRoles.Add(new EmployeeRole { RoleId = filter.roleId, RoleName = filter.role.roleName });
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
                                         .Include(e => e.Activities)
                                         .Include(e => e.EmployeeRoles)
                                         .SingleAsync(e => e.Id == filter.employeeId);

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
                                         .Include(e => e.Activities)
                                         .Include(e => e.EmployeeRoles)
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
                                          .OrderBy(e => e.Id)
                                          .Include(e => e.EmployeeRoles)
                                          .Include(e => e.Activities)
                                          .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery)) &&
                                                      (filter.roleId == Guid.Empty || x.EmployeeRoles.Where(y => y.RoleId == filter.roleId).Any()))
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
                                              .Where(em => EF.Functions.ILike(em.Name, $"{filter.searchQuery}"))
                                              .Include(e => e.Activities)
                                              .Include(e => e.EmployeeRoles)
                                              .Where(em =>
                                                  EF.Functions.ILike(em.Name, $"{filter.searchQuery}") ||
                                                  EF.Functions.ILike(em.Email, $"{filter.searchQuery}"))
                                              .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(foundEmployee);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByEmployeeId(EmployeeFilterDto filter)
    {
        try
        {
            var foundActivities = await _context.Activities
                                                .Include(ac => ac.EmployeeActivities)
                                                .Include(ac => ac.Employees)
                                                .Where(ac => ac.Employees.Any(em => em.Id == filter.employeeId))
                                                .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(foundActivities);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}