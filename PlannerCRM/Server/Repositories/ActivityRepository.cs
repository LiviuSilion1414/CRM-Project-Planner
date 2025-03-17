namespace PlannerCRM.Server.Repositories;

public class ActivityRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ResultDto> Insert(ActivityDto dto)
    {
        try
        {
            var model = _mapper.Map<Activity>(dto);

            var existingWorkOrder = await _context.WorkOrders.FindAsync(model.WorkOrder.Id);
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

            model.WorkOrder = existingWorkOrder;

            var workOrderActivity = new WorkOrderActivity
            {
                Activity = model,
                WorkOrderId = model.WorkOrder.Id
            };

            await _context.Activities.AddAsync(model);
            await _context.WorkOrderActivities.AddAsync(workOrderActivity);

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
        } 
        catch
        {
            throw;
        }
    }

    public async Task<ResultDto> Update(ActivityDto dto)
    {
        try
        {
            var existingWorkOrder = await _context.WorkOrders.FindAsync(dto.workOrderId);
            var model = _mapper.Map<ActivityDto, Activity>(dto);

            model.WorkOrder = existingWorkOrder;
            model.Employees = [];
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
        } 
        catch
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
        } 
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<ActivityDto> Get(ActivityFilterDto filter)
    {
        try
        {
            var activity = await _context.Activities
                .Include(a => a.WorkOrder).ThenInclude(x => x.FirmClient)
                .Include(a => a.EmployeeActivities)
                .SingleAsync(a => a.Id == filter.activityId);

            return _mapper.Map<ActivityDto>(activity);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> List(ActivityFilterDto filter)
    {
        try
        {
            var activities = await _context.Activities
                                           .OrderBy(a => a.Id)
                                           .Include(a => a.EmployeeActivities)
                                           .Include(a => a.WorkOrder).ThenInclude(x => x.FirmClient)
                                           .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery)) &&
                                                       (filter.clientId == Guid.Empty || x.WorkOrder.FirmClientId == filter.clientId) &&
                                                       (filter.workOrderId == Guid.Empty || x.WorkOrderId == filter.workOrderId))
                                           .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(activities);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> Search(ActivityFilterDto filter)
    {
        try
        {
            var foundItem = await _context.Activities
                                          .Where(ac => EF.Functions.ILike(ac.Name, $"%{filter.searchQuery}%"))
                                          .Include(ac => ac.WorkOrder)
                                          .ThenInclude(wo => wo.FirmClient)
                                          .ToListAsync();

            return _mapper?.Map<List<ActivityDto>>(foundItem);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> FindAssociatedEmployeesWithinActivity(ActivityFilterDto filter)
    {
        try
        {
            var foundEmployees = await _context.Employees
                                               .Include(em => em.Activities)
                                               .Where(em => em.Activities.Any(ac => ac.Id == filter.activityId))
                                               .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(foundEmployees);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<WorkOrderDto> FindAssociatedWorkOrderByActivityId(ActivityFilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                                               .Include(wo => wo.Activities)
                                               .Include(wo => wo.FirmClient)
                                               .SingleAsync(em => em.Activities.Any(ac => ac.Id == filter.activityId));

            return _mapper.Map<WorkOrderDto>(foundWorkOrder);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}