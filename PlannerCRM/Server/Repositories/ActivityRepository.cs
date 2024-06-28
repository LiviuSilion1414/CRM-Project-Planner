namespace PlannerCRM.Server.Repositories;

public class ActivityRepository(
        AppDbContext dbContext,
        DtoValidatorUtillity validator,
        Logger<DtoValidatorUtillity> logger) : IRepository<ActivityFormDto, ActivityViewDto>
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly DtoValidatorUtillity _validator = validator;
    private readonly ILogger<DtoValidatorUtillity> _logger = logger;

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

        await _dbContext.EmployeeActivity
            .Where(ea => ea.ActivityId == activityDelete.Id)
            .ForEachAsync(ea =>
                _dbContext.EmployeeActivity
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
                .Where(eaDto => _dbContext.EmployeeActivity
                    .Any(ea => eaDto.EmployeeId == ea.EmployeeId))
                .Select(e => e.MapToEmployeeActivity(dto.Id))
                .ToList();

            employeesToRemove
                .ForEach(item => _dbContext.EmployeeActivity.Remove(item));

            var workOrder = await _dbContext.WorkOrders
                .SingleAsync(wo => wo.Id == dto.WorkOrderId);

            _dbContext.Update(model);
            _dbContext.Update(workOrder);

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ActivityViewDto> GetForViewByIdAsync(int id, int _1, int _2)
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
            .FirstAsync(ac => ac.Id == activityId);
    }

    public async Task<ActivityDeleteDto> GetForDeleteByIdAsync(int id)
    {
        return await _dbContext.Activities
            .Select(ac => ac.MapToActivityDeleteDto(_dbContext, id))
            .SingleAsync(ac => ac.Id == id);
    }

    public async Task<List<ActivityViewDto>> GetActivityByEmployeeId(int employeeId, int limit = 0, int offset = 5)
    {
        return await _dbContext.Activities
            .Skip(limit)
            .Take(offset)
            .Select(ac => ac.MapToActivityViewDto(_dbContext))
            .Where(ac => _dbContext.EmployeeActivity
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