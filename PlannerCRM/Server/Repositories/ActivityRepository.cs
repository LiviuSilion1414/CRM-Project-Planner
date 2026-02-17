using Humanizer;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class ActivityRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ResultDto> Insert(ActivityDto dto)
    {
        try
        {
            var model = _mapper.Map<Activity>(dto);

            model.CreationDate = DateTime.Now;

            await _context.Activities.AddAsync(model);

            await _context.SaveChangesAsync();

            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch
        {
            throw;
        }
    }

    public async Task<ResultDto> Update(ActivityDto dto)
    {
        try
        {
            var activityUpd = await _context.Activities.FirstOrDefaultAsync(x => x.Id == dto.id);

            activityUpd.StartDate = dto.startDate;
            activityUpd.EndDate = dto.endDate;
            activityUpd.Name = dto.name;
            activityUpd.FkIdWorkOrder = dto.fkIdWorkOrder;

            _context.Activities.Update(activityUpd);

            await _context.SaveChangesAsync();

            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch
        {
            throw;
        }
    }

    public async Task Delete(ActivityDto dto)
    {
        try
        {
            var activity = await _context.Activities
                                         .AsNoTracking()
                                         .AsSplitQuery()
                                         .Include(a => a.EmployeeActivities)
                                         .FirstOrDefaultAsync(a => a.Id == dto.id);

            _context.Remove(activity);

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
            var itemsList = await _context.Activities
                                         .AsNoTracking()
                                         .AsSplitQuery()
                                         .Include(a => a.EmployeeActivities)
                                         .Include(a => a.WorkTimes)
                                         .Where(a => idList.Contains(a.Id))
                                         .ToListAsync();

            _context.RemoveRange(itemsList);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }

    public async Task<ActivityDto> Get(ActivityFilterDto filter)
    {
        try
        {
            var activity = await _context.Activities
                                         .AsNoTracking()
                                         .AsSplitQuery()
                                         .Include(a => a.FkIdWorkOrderNavigation).ThenInclude(x => x.FkIdFirmClientNavigation)
                                         .Include(a => a.EmployeeActivities).ThenInclude(x => x.FkIdEmployeeNavigation)
                                         .FirstAsync(a => a.Id == filter.activityId);

            return _mapper.Map<ActivityDto>(activity);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> List(ActivityFilterDto filter)
    {
        try
        {
            var activities = await _context.Activities
                                           .AsNoTracking()
                                           .AsSplitQuery()
                                           .OrderByDescending(a => a.CreationDate)
                                           .Include(a => a.EmployeeActivities).ThenInclude(x => x.FkIdEmployeeNavigation)
                                           .Include(a => a.FkIdWorkOrderNavigation).ThenInclude(x => x.FkIdFirmClientNavigation)
                                           .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery)) &&
                                                       (filter.workOrderId == null || x.FkIdWorkOrder == filter.workOrderId))
                                           .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(activities);
        } catch (Exception)
        {
            throw;
        }
    }
}