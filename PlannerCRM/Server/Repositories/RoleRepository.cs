using Humanizer;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class RoleRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(RoleDto dto)
    {
        try
        {
            var mappedResult = _mapper.Map<Role>(dto);
            mappedResult.CreationDate = DateTime.Now;

            await _context.Roles.AddAsync(mappedResult);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Update(RoleDto dto)
    {
        try
        {
            var model = await _context.Roles.FirstOrDefaultAsync(x => x.Id == dto.id);

            if (model != null)
            {
                _mapper.Map(dto, model);

                _context.Update(model);

                await _context.SaveChangesAsync();
            }
        } catch
        {
            throw;
        }
    }

    public async Task Delete(RoleFilterDto filter)
    {
        try
        {
            var activity = await _context.Roles.FirstOrDefaultAsync(x => x.Id == filter.id);

            _context.Remove(activity);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }

    public async Task<RoleDto> Get(RoleFilterDto filter)
    {
        try
        {
            var role = await _context.Roles
                                     .AsNoTracking()
                                     .AsSplitQuery()
                                     .Include(x => x.EmployeesRoles)
                                     .FirstAsync(a => a.Id == filter.id);

            return _mapper.Map<RoleDto>(role);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<RoleDto>> List(RoleFilterDto filter)
    {
        try
        {
            var roles = await _context.Roles
                                      .AsNoTracking()
                                      .AsSplitQuery()
                                      .Include(x => x.EmployeesRoles).ThenInclude(x => x.FkIdEmployeeNavigation)
                                      .Include(x => x.MenuRoles).ThenInclude(x => x.FkIdMenuNavigation)
                                      .OrderBy(x => x.Id)
                                      .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery.ToLower().Trim())))
                                      .ToListAsync();

            var mappedResult = _mapper.Map<List<RoleDto>>(roles);


            var menuRoles = await _context.MenuRoles
                                      .AsSplitQuery()
                                      .Include(x => x.FkIdMenuNavigation)
                                      .Include(x => x.FkIdRoleNavigation)
                                      .OrderBy(x => x.Id)
                                      .Where(x => roles.Select(r => r.Id).Contains(x.FkIdRole))
                                      .ToListAsync();
            
            var mappedMenuRoles = _mapper.Map<List<MenuRoleDto>>(menuRoles);

            var employeesRoles = await _context.EmployeesRoles
                                          .AsNoTracking()
                                          .AsSplitQuery()
                                          .Include(x => x.FkIdEmployeeNavigation)
                                          .Where(x => menuRoles.Select(r => r.FkIdRole).Contains(x.FkIdRole))
                                          .ToListAsync();
            var mappedEmployeeRoles = _mapper.Map<List<EmployeesRoleDto>>(employeesRoles);

            return mappedResult.Select(x =>
            {
                x.menuRoleList = mappedMenuRoles.Where(mr => mr.fkIdRole == x.id).Select(y =>
                {
                    y.employeesRoles = mappedEmployeeRoles.Where(k => k.fkIdRole == y.id).ToList();

                    return y;
                }).ToList();

                x.employeeRolesList = mappedEmployeeRoles.Where(y => y.fkIdRole == x.id).ToList();

                return x;
            }).ToList();

        } catch (Exception)
        {
            throw;
        }
    }
}
