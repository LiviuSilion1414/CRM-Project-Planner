using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class SettingRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(SettingDto dto)
    {
        try
        {
            var mappedResult = _mapper.Map<Setting>(dto);

            await _context.Settings.AddAsync(mappedResult);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Update(SettingDto dto)
    {
        try
        {
            var model = await _context.Settings.FindAsync((Guid)dto.id);

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

    public async Task Delete(SettingFilterDto filter)
    {
        try
        {
            var activity = await _context.Settings.SingleAsync(a => a.Id == filter.id/* && (bool)a.IsRemoveable*/);

            _context.Remove(activity);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }

    public async Task<SettingDto> Get(SettingFilterDto filter)
    {
        try
        {
            var role = await _context.Settings
                                     .AsNoTracking()
                                     .SingleAsync(a => a.Id == filter.id);

            return _mapper.Map<SettingDto>(role);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<SettingDto>> List(SettingFilterDto filter)
    {
        try
        {
            var settings = await _context.Settings
                                      .AsNoTracking()
                                      .AsSplitQuery()
                                      .OrderBy(x => x.Id)
                                      .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Title.ToLower().Trim().Contains(filter.searchQuery.ToLower().Trim())))
                                      .ToListAsync();

            return _mapper.Map<List<SettingDto>>(settings);

        } catch (Exception)
        {
            throw;
        }
    }
}
