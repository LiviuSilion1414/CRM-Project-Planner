namespace PlannerCRM.Server.Repositories;

public class WorkTimeRecordRepository(
    AppDbContext db,
    DtoValidatorUtillity validator,
    Logger<DtoValidatorUtillity> logger) : IRepository<WorkTimeRecordFormDto, WorkTimeRecordViewDto>
{
    private readonly AppDbContext _dbContext = db;
    private readonly DtoValidatorUtillity _validator = validator;
	private readonly ILogger<DtoValidatorUtillity> _logger = logger;

    public async Task AddAsync(WorkTimeRecordFormDto dto) {
        var isValid = _validator.ValidateWorkTime(dto);

        if (isValid) {
            await _dbContext.WorkTimeRecords.AddAsync(await dto.MapToWorkTimeRecord(_dbContext));
    
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id) {
        var workTimeRecordDelete = await _dbContext.WorkTimeRecords
            .SingleAsync(wtr => wtr.Id == id)
                ?? throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
        
        _dbContext.WorkTimeRecords.Remove(workTimeRecordDelete);
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task EditAsync(WorkTimeRecordFormDto dto) {
        var isValid = _validator.ValidateWorkTime(dto);
        
        if (isValid) {
            var model = await _dbContext.WorkTimeRecords
                .SingleAsync(wtr => wtr.Id == dto.Id);
    
            model = await dto.MapToWorkTimeRecord(_dbContext);
            _dbContext.Update(model);
    
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<WorkTimeRecordViewDto> GetForViewByIdAsync(int workOrderId, int activityId, int employeeId) {              
        return await _dbContext.WorkTimeRecords
            .Select(wtr => wtr.MapToWorkTimeRecordViewDto(_dbContext, workOrderId, activityId, employeeId))
            .OrderByDescending(wtr => wtr.Hours)
            .FirstOrDefaultAsync(wtr => 
                wtr.WorkOrderId == workOrderId && 
                wtr.ActivityId == activityId && 
                wtr.EmployeeId == employeeId);
    }

    public async Task<List<WorkTimeRecordViewDto>> GetPaginatedWorkTimeRecordsAsync(int limit, int offset) {
        return await _dbContext.WorkTimeRecords
            .OrderBy(wtr => wtr.Id)
            .Skip(limit)
            .Take(offset)
            .Select(wtr => wtr.MapToWorkTimeRecordViewDto())
            .ToListAsync();
    }

    public async Task<WorkTimeRecordViewDto> GetAllWorkTimeRecordsByEmployeeIdAsync(int employeeId) {
        return await _dbContext.WorkTimeRecords
            .Select(wtr => wtr.MapToWorkTimeRecordViewDto())
            .SingleAsync(wtr => wtr.EmployeeId == employeeId);
    }
}