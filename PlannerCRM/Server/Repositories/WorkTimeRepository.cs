using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class WorkTimeRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(WorkTimeDto dto)
    {
        try
        {
            var checkWorkTimeByDate = await context.WorkTimes.Where(x => x.CreationDate == DateOnly.FromDateTime(dto.startDate) && x.Id == dto.id).AnyAsync();


            if (!checkWorkTimeByDate && dto.hours > 0 &&  dto.hours <= 8)
            {
                var model = _mapper.Map<WorkTime>(dto);

                model.FkIdWorkOrderNavigation = await _context.WorkOrders.FindAsync(dto.workOrderId);
                model.FkIdActivityNavigation = await _context.Activities.FindAsync(dto.activityId);
                // model.employee = await _context.Employees.Where(x);
                await _context.AddAsync(model);

                await context.SaveChangesAsync();
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
            var existingModel = await context.WorkTimes.FirstOrDefaultAsync(x => x.CreationDate == DateOnly.FromDateTime(dto.startDate) && x.Id == dto.id);

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
            var existingModel = await context.WorkTimes.FirstOrDefaultAsync(x => x.CreationDate >= DateOnly.FromDateTime(filter.startDate) && x.Id == filter.id);

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
                                             .Include(x => x.EmployeeWorkTimes)
                                             .Include(x => x.FkIdWorkOrderNavigation)
                                             .Include(x => x.FkIdActivityNavigation)
                                             .FirstOrDefaultAsync(x => x.CreationDate >= DateOnly.FromDateTime(filter.startDate) && x.Id == filter.id);

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
                                             .Where(x => x.CreationDate >= DateOnly.FromDateTime(filter.startDate))
                                             .Include(x => x.EmployeeWorkTimes)
                                             .Include(x => x.FkIdWorkOrderNavigation)
                                             .Include(x => x.FkIdActivityNavigation)
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
                                             .Include(x => x.EmployeeWorkTimes)
                                             .Include(x => x.FkIdWorkOrderNavigation)
                                             .Include(x => x.FkIdActivityNavigation)
                                             .Where(x => x.CreationDate >= DateOnly.FromDateTime(filter.startDate) && x.EmployeeWorkTimes.Any(y => y.FkIdEmployee == filter.employeeId))
                                             .ToListAsync();

            return _mapper.Map<List<WorkTimeDto>>(existingModel);
        } catch (Exception ex)
        {
            throw;
        }
    }
}

