using Humanizer;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class WorkTimeRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ResultDto> Insert(WorkTimeDto dto)
    {
        try
        {
            var model = _mapper.Map<WorkTime>(dto);

            await _context.AddAsync(model);

            await context.SaveChangesAsync();

            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch (Exception ex)
        {
            throw;
        }
    }

    public async Task Update(WorkTimeDto dto)
    {
        try
        {
            var existingModel = await context.WorkTimes.FirstOrDefaultAsync(x => x.CreationDate == dto.creationDate && x.Id == dto.id);

            if (existingModel != null && dto.hours > 0 && dto.hours <= 8)
            {
                existingModel.Hours = dto.hours;
                existingModel.FkIdActivity = dto.fkIdActivity;
                existingModel.FkIdEmployee = dto.fkIdEmployee;

                _context.Update(existingModel);

                await context.SaveChangesAsync();
            }
        } catch (Exception ex)
        {
            throw;
        }
    }
    public async Task Delete(WorkTimeDto dto)
    {
        try
        {
            var existingModel = await context.WorkTimes.FirstOrDefaultAsync(x => x.Id == dto.id);

            _context.Remove(existingModel);

            await _context.SaveChangesAsync();
        } catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<WorkTimeDto> Get(WorkTimeFilterDto filter)
    {
        try
        {
            var existingModel = await context.WorkTimes
                                             .AsNoTracking()
                                             .AsSplitQuery()
                                             .Include(x => x.FkIdActivityNavigation)
                                             .FirstOrDefaultAsync(x => x.Id == filter.id);

            return _mapper.Map<WorkTimeDto>(existingModel);
        } catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<WorkTimeDto>> List(WorkTimeFilterDto filter)
    {
        try
        {
            var existingModel = await context.WorkTimes
                                             .AsNoTracking()
                                             .AsSplitQuery()
                                             .Where(x => x.CreationDate >= filter.startDate)
                                             .Include(x => x.FkIdActivityNavigation)
                                             .Where(x => (filter.employeeId == null || x.FkIdEmployee == filter.employeeId) &&
                                                         (filter.activityId == null || x.FkIdActivity == filter.activityId))
                                             .ToListAsync();

            return _mapper.Map<List<WorkTimeDto>>(existingModel);
        } catch (Exception ex)
        {
            throw;
        }
    }
}

