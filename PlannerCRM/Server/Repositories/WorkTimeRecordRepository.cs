namespace PlannerCRM.Server.Repositories;

public class WorkTimeRecordRepository(
    AppDbContext db,
    DtoValidatorUtillity validator) : IRepository<WorkTimeRecordFormDto>, IWorkTimeRecordRepository
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
            model.Employee = await _dbContext.Users
                .Where(em => !em.IsDeleted || !em.IsArchived && em.Id == dto.EmployeeId)
                .SingleAsync();

            _dbContext.Update(model);

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<WorkTimeRecordViewDto> GetForViewByIdAsync(int workOrderId, int activityId, int employeeId)
    {
        return await _dbContext.WorkTimeRecords
            .Select(wtr => wtr.MapToWorkTimeRecordViewDto())
            .OrderByDescending(wtr => wtr.Hours)
            .FirstOrDefaultAsync(wtr =>
                wtr.WorkOrderId == workOrderId &&
                wtr.ActivityId == activityId &&
                wtr.EmployeeId == employeeId);
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