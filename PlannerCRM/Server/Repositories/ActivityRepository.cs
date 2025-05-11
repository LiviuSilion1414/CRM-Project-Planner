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

            var existingWorkOrder = await _context.WorkOrders.FindAsync(model.FkIdWorkOrderNavigation.Id);
            if (existingWorkOrder == null)
            {
                return new ResultDto()
                {
                    id = null,
                    data = null,
                    hasCompleted = false,
                    message = "Operation failed",
                    messageType = MessageType.Error,
                    statusCode = HttpStatusCode.BadRequest
                };
            }

            model.FkIdWorkOrderNavigation = existingWorkOrder;

            var workOrderActivity = new WorkOrdersActivity
            {
                FkIdActivityNavigation = model,
                FkIdWorkOrder = model.FkIdWorkOrderNavigation.Id
            };

            await _context.Activities.AddAsync(model);
            await _context.WorkOrdersActivities.AddAsync(workOrderActivity);

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
            var existingWorkOrder = await _context.WorkOrders.FindAsync(dto.fkIdWorkOrder);
            var model = _mapper.Map<ActivityDto, Activity>(dto);

            model.FkIdWorkOrderNavigation = existingWorkOrder;
            model.EmployeeWorkTimes = [];
            model.EmployeeActivities = [];

            _context.Activities.Update(model);

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

    public async Task Delete(ActivityFilterDto filter)
    {
        try
        {
            var activity = await _context.Activities
                                         .Include(a => a.EmployeeActivities)
                                         .SingleAsync(a => a.Id == filter.activityId);

            _context.Remove(activity);

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
                                         .Include(a => a.EmployeeActivities)
                                         .SingleAsync(a => a.Id == filter.activityId);

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
                                           .OrderBy(a => a.Id)
                                           .Include(a => a.EmployeeActivities).ThenInclude(x => x.FkIdEmployeeNavigation)
                                           .Include(a => a.FkIdWorkOrderNavigation).ThenInclude(x => x.FkIdFirmClientNavigation)
                                           .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery)) &&
                                                       (filter.firmClientId == Guid.Empty || x.FkIdWorkOrderNavigation.FkIdFirmClient == filter.firmClientId) &&
                                                       (filter.workOrderId == Guid.Empty || x.FkIdWorkOrderNavigation.Id == filter.workOrderId))
                                           .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(activities);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> Search(ActivityFilterDto filter)
    {
        try
        {
            var foundItem = await _context.Activities
                                          .Where(ac => EF.Functions.Like(ac.Name, $"%{filter.searchQuery}%"))
                                          .Include(ac => ac.FkIdWorkOrderNavigation)
                                          .ThenInclude(wo => wo.FkIdFirmClientNavigation)
                                          .ToListAsync();

            return _mapper?.Map<List<ActivityDto>>(foundItem);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> FindAssociatedEmployeesWithinActivity(ActivityFilterDto filter)
    {
        try
        {
            var foundEmployees = await _context.Employees
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .Include(em => em.EmployeeActivities)
                                               .Where(em => em.EmployeeActivities.Any(ac => ac.FkIdActivity == filter.activityId))
                                               .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(foundEmployees);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<WorkOrderDto> FindAssociatedWorkOrderByActivityId(ActivityFilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .Include(wo => wo.Activities)
                                               .Include(wo => wo.FkIdFirmClientNavigation)
                                               .SingleAsync(em => em.Activities.Any(ac => ac.Id == filter.activityId));

            return _mapper.Map<WorkOrderDto>(foundWorkOrder);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task AssignActivityToEmployee(ActivityFilterDto filter)
    {
        try
        {
            var employeeActivity = await _context.EmployeeActivities
                                                 .Where(x => x.FkIdActivity == filter.activityId && x.FkIdEmployee == filter.employeeId)
                                                 .SingleOrDefaultAsync();

            if (employeeActivity != null)
            {
                return;
            }

            await _context.EmployeeActivities.AddAsync(
                new EmployeeActivity { 
                    FkIdActivity = (Guid)filter.activityId, 
                    FkIdEmployee = (Guid)filter.employeeId,
                    FkIdWorkOrder = (Guid)filter.workOrderId,
                    FkIdFirmClient = (Guid)filter.firmClientId
                }
            );

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task RemoveAssignedEmployeeFromActivity(ActivityFilterDto filter)
    {
        try
        {
            var employeeActivity = await _context.EmployeeActivities
                                                 .Where(x => x.FkIdActivity == filter.activityId && x.FkIdEmployee == filter.employeeId)
                                                 .SingleOrDefaultAsync();

            if (employeeActivity == null)
            {
                return;
            }

            _context.EmployeeActivities.Remove(employeeActivity);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }
}