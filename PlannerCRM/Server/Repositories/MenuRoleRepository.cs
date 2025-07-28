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
            await _context.MenuRoles.AddAsync(_mapper.Map<MenuRole>(dto));

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
            var activity = await _context.MenuRoles.SingleAsync(a => a.Id == filter.roleId/* && (bool)a.IsRemoveable*/);

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
            var roles = await _context.MenuRoles
                                      .AsNoTracking()
                                      .AsSplitQuery()
                                      .Include(x => x.FkIdRoleNavigation)
                                      .Include(x => x.FkIdMenuNavigation)
                                      .OrderBy(x => x.Id)
                                      //.Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery.ToLower().Trim())))
                                      .ToListAsync();

            return _mapper.Map<List<MenuRoleDto>>(roles);
        } catch (Exception)
        {
            throw;
        }
    }
}
