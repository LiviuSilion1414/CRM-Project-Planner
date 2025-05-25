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
            await _context.Roles.AddAsync(_mapper.Map<Role>(dto));

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
            var model = await _context.Roles.FindAsync((Guid)dto.id);

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
            var activity = await _context.Roles.SingleAsync(a => a.Id == filter.roleId/* && (bool)a.IsRemoveable*/);

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
                                     .SingleAsync(a => a.Id == filter.roleId);

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
                                      .Include(x => x.EmployeesRoles)
                                      .OrderBy(x => x.Id)
                                      .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery.ToLower().Trim())))
                                      .ToListAsync();

            return _mapper.Map<List<RoleDto>>(roles);
        } catch (Exception)
        {
            throw;
        }
    }
}
