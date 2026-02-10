using Humanizer;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class MenuRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(MenuDto dto)
    {
        try
        {
            var mappedResult = _mapper.Map<Menu>(dto);

            await _context.Menus.AddAsync(mappedResult);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Update(MenuDto dto)
    {
        try
        {
            var model = await _context.Menus.FirstOrDefaultAsync(x => x.Id == dto.id);

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

    public async Task Delete(MenuDto dto)
    {
        try
        {
            var menuRoles = await _context.MenuRoles.FirstOrDefaultAsync(x => x.FkIdMenu == dto.id);

            _context.Remove(menuRoles);
            await _context.SaveChangesAsync();

            var menu = await _context.Menus.FirstOrDefaultAsync(x => x.Id == dto.id);

            _context.Remove(menu);
            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }

    public async Task<MenuDto> Get(MenuFilterDto filter)
    {
        try
        {
            var role = await _context.Menus
                                     .AsNoTracking()
                                     .AsSplitQuery()
                                     .Include(x => x.MenuRoles).ThenInclude(x => x.FkIdRoleNavigation)
                                     .FirstAsync(a => a.Id == filter.id);

            return _mapper.Map<MenuDto>(role);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<MenuDto>> List(MenuFilterDto filter)
    {
        try
        {
            var roles = await _context.Menus
                                      .AsNoTracking()
                                      .AsSplitQuery()
                                      .Include(x => x.MenuRoles).ThenInclude(x => x.FkIdRoleNavigation)
                                      .Where(x => (string.IsNullOrEmpty(filter.title) || x.Title.ToLower().Trim().Contains(filter.title.ToLower().Trim())))
                                      .ToListAsync();

            return _mapper.Map<List<MenuDto>>(roles);
        } catch (Exception)
        {
            throw;
        }
    }
}
