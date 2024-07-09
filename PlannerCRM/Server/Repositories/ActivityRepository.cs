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

            var workOrder = await _dbContext.WorkOrders
                .SingleAsync(wo => wo.Id == dto.WorkOrderId);

            workOrder.Activities.Add(model);

            _dbContext.Update(workOrder);

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
            .Select(ac => ac.MapToActivityViewDto(_dbContext))
            .SingleAsync(ac => ac.Id == id);
    }

    public async Task<ActivityFormDto> GetForEditByIdAsync(int activityId)
    {
        return await _dbContext.Activities
            .Where(ac => ac.Id == activityId &&
                _dbContext.WorkOrders
                    .Any(wo => wo.Id == ac.WorkOrderId && !wo.IsDeleted || !wo.IsInvoiceCreated))
            .Select(ac => ac.MapToActivityFormDto(_dbContext))
            .SingleAsync(ac => ac.Id == activityId);
    }

    public async Task<ActivityDeleteDto> GetForDeleteByIdAsync(int id)
    {
        return await _dbContext.Activities
            .Select(ac => ac.MapToActivityDeleteDto(_dbContext, id))
            .SingleAsync(ac => ac.Id == id);
    }

    public async Task<List<ActivityViewDto>> GetActivityByEmployeeId(int employeeId, int limit = 10, int offset = 0)
    {
        return await _dbContext.Activities
            .Skip(offset)
            .Take(limit)
            .Select(ac => ac.MapToActivityViewDto(_dbContext))
            .Where(ac => _dbContext.EmployeeActivities
                .Any(ea => ea.EmployeeId == employeeId && ac.Id == ea.ActivityId))
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(int workOrderId)
    {
        return await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .Select(ac => ac.MapToActivityViewDto(_dbContext))
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetAllAsync()
    {
        return await _dbContext.Activities
            .Select(ac => ac.MapToActivityViewDto(_dbContext))
            .ToListAsync();
    }

    public async Task<int> GetCollectionSizeByEmployeeIdAsync(int employeeId) =>
        (await GetActivityByEmployeeId(employeeId)).Count;
}