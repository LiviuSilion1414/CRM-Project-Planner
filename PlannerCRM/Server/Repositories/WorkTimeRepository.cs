namespace PlannerCRM.Server.Repositories;

public class WorkTimeRepository(
    AppDbContext db,
    DtoValidatorUtillity validator) : IRepository<WorkTimeRecordFormDto>, IWorkTimeRepository
{
    private readonly AppDbContext _dbContext = db;
    private readonly DtoValidatorUtillity _validator = validator;

    public async Task AddAsync(WorkTimeRecordFormDto dto)
    {
        var isValid = _validator.ValidateWorkTime(dto);

        if (isValid)
        {
            await _dbContext.WorkTimeRecords.AddAsync(dto.MapToWorkTimeRecord());

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var workTimeRecordDelete = await _dbContext.WorkTimeRecords
            .SingleAsync(wtr => wtr.Id == id)
                ?? throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);

        _dbContext.WorkTimeRecords.Remove(workTimeRecordDelete);

        await _dbContext.SaveChangesAsync();
    }

    public async Task EditAsync(WorkTimeRecordFormDto dto)
    {
        var isValid = _validator.ValidateWorkTime(dto);

        if (isValid)
        {
            var model = await _dbContext.WorkTimeRecords
                .SingleAsync(wtr => wtr.Id == dto.Id);

            model = dto.MapToWorkTimeRecord();

            _dbContext.Update(model);

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<WorkTimeRecordViewDto> GetForViewByIdAsync(int workOrderId, int activityId, int employeeId)
    {
        var workTimeRecord = await _dbContext.WorkTimeRecords
            .OrderByDescending(wtr => wtr.Hours)
            .SingleOrDefaultAsync(wtr =>
                wtr.WorkOrderId == workOrderId &&
                wtr.ActivityId == activityId &&
                wtr.EmployeeId == employeeId);

        return workTimeRecord?.MapToWorkTimeRecordViewDto();
    }

    public async Task<List<WorkTimeRecordViewDto>> GetPaginatedWorkTimeRecordsAsync(int limit, int offset)
    {
        return await _dbContext.WorkTimeRecords
            .OrderBy(wtr => wtr.Id)
            .Skip(limit)
            .Take(offset)
            .Select(wtr => wtr.MapToWorkTimeRecordViewDto())
            .ToListAsync();
    }

    public async Task<WorkTimeRecordViewDto> GetAllWorkTimeRecordsByEmployeeIdAsync(int employeeId)
    {
        return await _dbContext.WorkTimeRecords
            .Select(wtr => wtr.MapToWorkTimeRecordViewDto())
            .SingleAsync(wtr => wtr.EmployeeId == employeeId);
    }
}