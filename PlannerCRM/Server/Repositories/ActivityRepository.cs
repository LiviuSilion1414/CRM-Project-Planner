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
            var model = await _dbContext.Activities
                .SingleAsync(ac => ac.Id == dto.Id);

            model = dto.MapToActivity();

            var employeesToRemove = dto.DeleteEmployeeActivity
                .Where(eaDto => _dbContext.EmployeeActivities
                    .Any(ea => eaDto.EmployeeId == ea.EmployeeId))
                .Select(e => e.MapToEmployeeActivity(dto.Id))
                .ToList();

            employeesToRemove
                .ForEach(item => _dbContext.EmployeeActivities.Remove(item));

            var workOrder = await _dbContext.WorkOrders
                .SingleAsync(wo => wo.Id == dto.WorkOrderId);

            _dbContext.Update(model);
            _dbContext.Update(workOrder);

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ActivityViewDto> GetForViewByIdAsync(int id)
    {
        return await _dbContext.Activities
            .Select(ac => ac.MapToActivityViewDto())
            .SingleAsync(ac => ac.Id == id);
    }

    public async Task<ActivityFormDto> GetForEditByIdAsync(int activityId)
    {
        var activity = await _dbContext.Activities
            .Where(ac => ac.Id == activityId &&
                _dbContext.WorkOrders
                    .Any(wo => wo.Id == ac.WorkOrderId && !wo.IsDeleted || !wo.IsInvoiceCreated))
            .Select(ac => ac.MapToActivityFormDto())
            .SingleAsync();
        activity.ClientName = await GetClientNameByActivityAsync(activity);

        return activity;
    }

    public async Task<string> GetClientNameByActivityAsync(ActivityFormDto activity)
    {
        return (await _dbContext.Clients
            .SingleAsync(cl => cl.WorkOrders
                    .Any(wo => wo.Id == activity.WorkOrderId)))
            .Name;
    }

    public async Task<ActivityDeleteDto> GetForDeleteByIdAsync(int activityId)
    {
        var activity = await _dbContext.Activities
            .Select(ac => ac.MapToActivityDeleteDto())
            .SingleAsync(ac => ac.Id == activityId);
        var employeesInvolved = await GetEmployeesInvolvedInActivityAsync(activityId);
        activity.Employees = employeesInvolved;

        return activity;
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
        return await _dbContext.Activities
            .Skip(offset)
            .Take(limit)
            .Select(ac => ac.MapToActivityViewDto())
            .Where(ac => _dbContext.EmployeeActivities
                .Any(ea => ea.EmployeeId == employeeId && ac.Id == ea.ActivityId))
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(int workOrderId)
    {
        var activities = await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();

        return (await GetEmployeesInvolvedInActivity(activities))
            .Select(ac => ac.MapToActivityViewDto())
            .ToList();
    }

    private async Task<List<Activity>> GetEmployeesInvolvedInActivity(List<Activity> activitiesList)
    {
        var activities = new List<Activity>(activitiesList);

        foreach (var ac in activities)
        {
            ac.EmployeeActivity = (await GetEmployeesActivitiesByActivityId(ac.Id)).ToHashSet();
            foreach (var ea in ac.EmployeeActivity)
            {
                ea.Employee = await _dbContext.Users
                    .SingleAsync(e => e.Id == ea.EmployeeId);
            }
        }
        return activities;
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
            .Select(ac => ac.MapToActivityViewDto())
            .ToListAsync();
    }

    public async Task<int> GetCollectionSizeByEmployeeIdAsync(int employeeId) =>
        (await GetActivityByEmployeeId(employeeId)).Count;
}