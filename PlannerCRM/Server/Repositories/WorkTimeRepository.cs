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
            var checkWorkTimeByDate = await context.WorkTimes.Where(x => x.CreationDate == dto.creationDate && x.Id == dto.id).AnyAsync();
            var maxDailyHours = 8;

            if (!checkWorkTimeByDate && dto.hours > 0 &&  dto.hours <= maxDailyHours)
            {
                var model = _mapper.Map<WorkTime>(dto);

                model.FkIdActivityNavigation = await _context.Activities.FindAsync(dto.fkIdActivity);
                // model.employee = await _context.Employees.Where(x);
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
            }
            else
            {
                return new ResultDto()
                {
                    id = null,
                    data = null,
                    hasCompleted = false,
                    message = "Excessive working hours",
                    messageType = MessageType.Error,
                    statusCode = HttpStatusCode.BadRequest
                };
            }
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
                existingModel = _mapper.Map<WorkTime>(dto);

                _context.Update(existingModel);

                await context.SaveChangesAsync();
            }
        } catch (Exception ex)
        {
            throw;
        }
    }
    public async Task Delete(WorkTimeFilterDto filter)
    {
        try
        {
            var existingModel = await context.WorkTimes.FirstOrDefaultAsync(x => x.CreationDate >= filter.startDate && x.Id == filter.id);

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
                                             .FirstOrDefaultAsync(x => x.CreationDate >= filter.startDate && x.Id == filter.id);

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

    public async Task<List<WorkTimeDto>> FindAssociatedWorktimesByEmployeeId(WorkTimeFilterDto filter)
    {
        try
        {
            var existingModel = await context.WorkTimes
                                             .AsNoTracking()
                                             .AsSplitQuery()
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

