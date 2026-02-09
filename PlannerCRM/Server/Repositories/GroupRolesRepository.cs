using Humanizer;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class GroupRolesRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(GroupRoleDto dto)
    {
        try
        {
            var mappedResult = _mapper.Map<GroupRole>(dto);

            await _context.GroupRoles.AddAsync(mappedResult);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Update(GroupRoleDto dto)
    {
        try
        {
            var model = await _context.GroupRoles.FirstOrDefaultAsync(x => x.Id == dto.id);

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

    public async Task Delete(GroupRoleDto dto)
    {
        try
        {
            var activity = await _context.GroupRoles.FirstOrDefaultAsync(x => x.Id == dto.id);

            _context.Remove(activity);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }

    public async Task<GroupRoleDto> Get(GroupRolesFilterDto filter)
    {
        try
        {
            var role = await _context.GroupRoles
                                     .AsNoTracking()
                                     .AsSplitQuery()
                                     .Include(x => x.Roles)
                                     .FirstAsync(a => a.Id == filter.id);

            return _mapper.Map<GroupRoleDto>(role);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<GroupRoleDto>> List(GroupRolesFilterDto filter)
    {
        try
        {
            var roles = await _context.GroupRoles
                                      .AsNoTracking()
                                      .AsSplitQuery()
                                      .Include(x => x.Roles)
                                      .ToListAsync();

            return _mapper.Map<List<GroupRoleDto>>(roles);
        } catch (Exception)
        {
            throw;
        }
    }
}
