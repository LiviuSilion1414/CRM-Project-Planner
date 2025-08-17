using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class MenuRoleRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(MenuRoleDto dto)
    {
        try
        {
            var mappedResult = _mapper.Map<MenuRole>(dto);
            mappedResult.FkIdRoleNavigation = null;

            await _context.MenuRoles.AddAsync(mappedResult);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Update(MenuRoleDto dto)
    {
        try
        {
            var model = await _context.MenuRoles.FindAsync((Guid)dto.id);

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

    public async Task Delete(MenuRoleFilterDto filter)
    {
        try
        {
            var activity = await _context.MenuRoles.SingleAsync(a => a.Id == filter.id/* && (bool)a.IsRemoveable*/);

            _context.Remove(activity);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }

    public async Task<MenuRoleDto> Get(MenuRoleFilterDto filter)
    {
        try
        {
            var role = await _context.MenuRoles
                                     .AsNoTracking()
                                     .AsSplitQuery()
                                     .Include(x => x.FkIdRoleNavigation)
                                     .Include(x => x.FkIdMenuNavigation)
                                     .SingleAsync(a => a.Id == filter.roleId);

            return _mapper.Map<MenuRoleDto>(role);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<MenuRoleDto>> List(MenuRoleFilterDto filter)
    {
        try
        {
            var menuRoles = await _context.MenuRoles
                                      .AsSplitQuery()
                                      .Include(x => x.FkIdMenuNavigation)
                                      .Include(x => x.FkIdRoleNavigation)
                                      .OrderBy(x => x.Id)
                                      .ToListAsync();
            var mappedResult = _mapper.Map<List<MenuRoleDto>>(menuRoles);

            var employeesRoles = await _context.EmployeesRoles
                                          .AsNoTracking()
                                          .AsSplitQuery()
                                          .Include(x => x.FkIdEmployeeNavigation)
                                          .Where(x => menuRoles.Select(r => r.FkIdRole).Contains(x.FkIdRole))
                                          .ToListAsync();
            var mappedEmployeeRoles = _mapper.Map<List<EmployeesRoleDto>>(employeesRoles);

            return mappedResult.Select(x =>
            {
                x.employeesRoles = mappedEmployeeRoles.Where(y => y.fkIdRole == x.fkIdRole).ToList();

                return x;
            })
            .ToList();
        } catch (Exception)
        {
            throw;
        }
    }
}
