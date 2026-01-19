using Humanizer;
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
            var model = await _context.MenuRoles.FirstOrDefaultAsync(x => x.Id == dto.id);

            if (model == null) return;
            _mapper.Map(dto, model);

            _context.Update(model);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Delete(MenuRoleFilterDto filter)
    {
        try
        {
            var activity = await _context.MenuRoles.FirstOrDefaultAsync(x => x.Id == filter.menuId);

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
                                     .SingleAsync(a => a.Id == filter.id);

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

            return mappedResult;
        } catch (Exception)
        {
            throw;
        }
    }
}
