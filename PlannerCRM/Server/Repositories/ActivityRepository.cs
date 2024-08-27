namespace PlannerCRM.Server.Repositories;

public class ActivityRepository(
        AppDbContext dbContext,
        DtoValidatorUtillity validator) : IRepository<ActivityFormDto>, IActivityRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly DtoValidatorUtillity _validator = validator;

    public async Task AddAsync(ActivityFormDto dto)
    {
        var isValid = await _validator.ValidateActivityAsync(dto, OperationType.ADD);

        if (isValid)
        {
            var model = dto.MapToActivity();
            await _dbContext.Activities.AddAsync(model);

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var activityDelete = await _validator.ValidateDeleteActivityAsync(id);

        await _dbContext.EmployeeActivities
            .Where(ea => ea.ActivityId == activityDelete.Id)
            .ForEachAsync(ea =>
                _dbContext.EmployeeActivities
                    .Remove(ea)
            );

        _dbContext.Activities.Remove(activityDelete);

        await _dbContext.SaveChangesAsync();
    }

    public async Task EditAsync(ActivityFormDto dto)
    {
        var isValid = await _validator.ValidateActivityAsync(dto, OperationType.EDIT);

        if (isValid)
        {
            if (dto.DeleteEmployeeActivity.Count > 0) 
            {
                var employeesToRemove = dto.DeleteEmployeeActivity
                    .Where(eaDto => _dbContext.EmployeeActivities
                        .Any(ea => eaDto.EmployeeId == ea.EmployeeId))
                    .Select(e => e.MapToEmployeeActivity(dto.Id))
                    .ToList();

                employeesToRemove
                    .ForEach(item => _dbContext.EmployeeActivities.Remove(item));
            }

            _dbContext.Update(dto.MapToActivity());

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ActivityViewDto> GetForViewByIdAsync(int activityId)
    {
        var activity = await GetEmployeesInvolvedInSingleActivity(activityId);
        var client = (await GetClientByWorkOrderIdAsync(activity.WorkOrderId));

        var mappedActivity = activity.MapToActivityViewDto();
        mappedActivity.ClientName = client.Name;

        return mappedActivity;
    }

    public async Task<ActivityFormDto> GetForEditByIdAsync(int activityId)
    {
        var activity = await GetEmployeesInvolvedInSingleActivity(activityId);
        var client = (await GetClientByWorkOrderIdAsync(activity.WorkOrderId));

        var mappedActivity = activity.MapToActivityFormDto();
        mappedActivity.ClientName = client.Name;
        
        return mappedActivity;
    }

    public async Task<FirmClient> GetClientByWorkOrderIdAsync(int workOrderId)
    {
        var workOrder = await _dbContext.WorkOrders
            .SingleAsync(wo => wo.Id == workOrderId);

        return (await _dbContext.Clients
            .SingleAsync(cl => cl.Id == workOrder.ClientId));
    }

    public async Task<ActivityDeleteDto> GetForDeleteByIdAsync(int activityId)
    {
        var employees = await GetEmployeesInvolvedInActivityAsync(activityId);
        var activity = await _dbContext.Activities
            .SingleAsync(ac => ac.Id == activityId);
        var client = await GetClientByWorkOrderIdAsync(activity.WorkOrderId);

        var mappedActivity = activity.MapToActivityDeleteDto();
        mappedActivity.Employees = employees;
        mappedActivity.Client = client.MapToClientViewDto();

        return mappedActivity;
    }

    private async Task<List<EmployeeSelectDto>> GetEmployeesInvolvedInActivityAsync(int activityId)
    {
        return await _dbContext.EmployeeActivities
            .Where(ea => ea.ActivityId == activityId)
            .Select(ea => ea.Employee.MapToEmployeeSelectDto())
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetActivityByEmployeeId(int employeeId, int limit = 10, int offset = 0)
    {
        var activities = await _dbContext.Activities
            .Skip(offset)
            .Take(limit)
            .Where(ac => _dbContext.EmployeeActivities
                .Any(ea => ea.EmployeeId == employeeId && ac.Id == ea.ActivityId))
            .ToListAsync();

        return activities
            .Select(ac => ac.MapToActivityViewDto())
            .ToList();
    }

    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(int workOrderId)
    {
        var activities = await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();

        return (await GetEmployeesInvolvedInEachActivity(activities))
            .Select(ac => ac.MapToActivityViewDto())
            .ToList();
    }

    private async Task<List<Activity>> GetEmployeesInvolvedInEachActivity(List<Activity> activitiesList)
    {
        var activities = new List<Activity>();

        foreach (var ac in activitiesList)
        {
            activities.Add(await GetEmployeesInvolvedInSingleActivity(ac.Id));
        }
        return activities;
    }

    private async Task<Activity> GetEmployeesInvolvedInSingleActivity(int activityId)
    {
        var activity = await _dbContext.Activities
            .SingleAsync(ac => ac.Id == activityId);
        activity.EmployeeActivity = (await GetEmployeesActivitiesByActivityId(activity.Id)).ToHashSet();
        foreach (var ea in activity.EmployeeActivity)
        {
            ea.Employee = await _dbContext.Users
                .SingleAsync(e => e.Id == ea.EmployeeId);
        }
        return activity;
    }

    private async Task<List<EmployeeActivity>> GetEmployeesActivitiesByActivityId(int activityId)
    {
        return await _dbContext.EmployeeActivities
            .Where(ea => ea.ActivityId == activityId)
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetAllAsync()
    {
        return await _dbContext.Activities
            .Select(ac => ac.MapToActivityViewDto("", ""))
            .ToListAsync();
    }

    public async Task<int> GetCollectionSizeByEmployeeIdAsync(int employeeId) =>
        (await GetActivityByEmployeeId(employeeId)).Count;
}