using Humanizer;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class SystemLogRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(SystemLogDto dto)
    {
        try
        {
            var mappedResult = _mapper.Map<SystemLog>(dto);

            await _context.SystemLogs.AddAsync(mappedResult);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Update(SystemLogDto dto)
    {
        try
        {
            var model = await _context.SystemLogs.FirstOrDefaultAsync(x => x.Id == dto.id);

            if (model == null) return;

            model.Endpoint = dto.endpoint;
            model.Reason = dto.reason;
            model.Stacktrace = dto.stacktrace;
            model.Date = dto.date;
            model.Username = dto.username;
            model.Request = dto.request;
            model.FkIdProject = dto.fkIdProject;

            _context.Update(model);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Delete(SystemLogDto dto)
    {
        try
        {
            var model = await _context.SystemLogs.FirstOrDefaultAsync(x => x.Id == dto.id);

            if (model == null) return;

            _context.Remove(model);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }


    public async Task DeleteMultiple(List<Guid?> idList)
    {
        try
        {
            var itemsList = await _context.SystemLogs
                                         .AsNoTracking()
                                         .Where(a => idList.Contains(a.Id))
                                         .ToListAsync();

            _context.RemoveRange(itemsList);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }


    public async Task<SystemLogDto> Get(SystemLogFilterDto filter)
    {
        try
        {
            var role = await _context.SystemLogs
                                     .AsNoTracking()
                                     .FirstAsync(a => a.Id == filter.id);

            return _mapper.Map<SystemLogDto>(role);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<SystemLogDto>> List(SystemLogFilterDto filter)
    {
        try
        {
            var settings = await _context.SystemLogs
                                      .AsNoTracking()
                                      .AsSplitQuery()
                                      .OrderByDescending(x => x.Date)
                                      .Where(x => (string.IsNullOrEmpty(filter.username) || x.Username.ToLower().Trim().Contains(filter.username.ToLower().Trim())) &&
                                                  (filter.date == null || ((DateTime)filter.date).Date == ((DateTime)x.Date).Date) &&
                                                  (filter.fkIdProject == null || filter.fkIdProject == x.FkIdProject))
                                      .ToListAsync();

            return _mapper.Map<List<SystemLogDto>>(settings);

        } catch (Exception)
        {
            throw;
        }
    }
}
