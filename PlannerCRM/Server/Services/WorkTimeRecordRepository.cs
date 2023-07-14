namespace PlannerCRM.Server.Services;

public class WorkTimeRecordRepository
{
    private readonly AppDbContext _db;
    private readonly DtoValidatorService _validator;
	private readonly Logger<DtoValidatorService> _logger;

    public WorkTimeRecordRepository(AppDbContext db, DtoValidatorService validator, Logger<DtoValidatorService> logger) {
		_db = db;
		_validator = validator;
		_logger = logger;
	}

    public async Task AddAsync(WorkTimeRecordFormDto dto) {
        try {
            _validator.ValidateWorkTime(dto, out var isValid);

            if (isValid) {
                await _db.WorkTimeRecords.AddAsync(
                    new WorkTimeRecord {
                        Id = dto.Id,
                        Date = dto.Date,
                        Hours = dto.Hours,
                        TotalPrice = dto.TotalPrice + dto.Hours,
                        ActivityId = dto.ActivityId,
                        EmployeeId = dto.EmployeeId,
                        Employee = _db.Employees
                            .Where(em => !em.IsDeleted)
                            .Single(e => e.Id == dto.EmployeeId),
                        WorkOrderId = await _db.WorkOrders
                            .AnyAsync(wo => !wo.IsDeleted && !wo.IsCompleted)
                                ? dto.WorkOrderId
                                : throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_ADD)
                    }
                );
        
				if (await _db.SaveChangesAsync() == 0) {
					throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
				}
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            
            throw;
        }
    }

    public async Task DeleteAsync(int id) {
        var workTimeRecordDelete = await _db.WorkTimeRecords
            .SingleOrDefaultAsync(wtr => wtr.Id == id)
                ?? throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
        
        _db.WorkTimeRecords.Remove(workTimeRecordDelete);
        
        if (await _db.SaveChangesAsync() == 0) {
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
        }
    }
    
    public async Task EditAsync(WorkTimeRecordFormDto dto) {
        try {
            _validator.ValidateWorkTime(dto, out var isValid);
            
            if (isValid) {
                var model = await _db.WorkTimeRecords
                    .SingleOrDefaultAsync(wtr => wtr.Id == dto.Id);
        
                model.Id = dto.Id;
                model.Date = dto.Date;
                model.Hours = dto.Hours;
                model.TotalPrice = dto.TotalPrice;
                model.ActivityId = dto.ActivityId;
                model.WorkOrderId = dto.WorkOrderId;
                model.EmployeeId = dto.EmployeeId;
                model.Employee = await _db.Employees
                    .Where(em => !em.IsDeleted)
                    .SingleAsync(em => em.Id == dto.EmployeeId);
        
                _db.Update(model);
        
				if (await _db.SaveChangesAsync() == 0) {
					throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
				}
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);
            
            throw;
        }
    }

    public async Task<WorkTimeRecordViewDto> GetAsync(int workOrderId, int activityId, int employeeId) {
        var hasElements = await _db.WorkTimeRecords
            .AnyAsync(wtr => wtr.ActivityId == activityId && wtr.EmployeeId == employeeId);
        return hasElements 
            ? await _db.WorkTimeRecords
                .Select(wtr => new WorkTimeRecordViewDto {
                    Id = wtr.Id,
                    Date = wtr.Date,
                    Hours = _db.WorkTimeRecords
                        .Where(wtr => wtr.WorkOrderId == workOrderId && wtr.ActivityId == activityId && wtr.EmployeeId == employeeId)
                        .Distinct()
                        .Sum(wtrSum => wtrSum.Hours),
                    TotalPrice = wtr.TotalPrice,
                    ActivityId = wtr.ActivityId,
                    WorkOrderId = wtr.WorkOrderId,
                    EmployeeId = wtr.EmployeeId,
                })
                .OrderByDescending(wtr => wtr.Hours)
                .FirstAsync(wtr => wtr.WorkOrderId == workOrderId && wtr.ActivityId == activityId && wtr.EmployeeId == employeeId)
            : null;
    }

    public async Task<List<WorkTimeRecordViewDto>> GetAllAsync() {
        return await _db.WorkTimeRecords
            .Select(wtr => new WorkTimeRecordViewDto {
                Id = wtr.Id,
                Date = wtr.Date,
                Hours = wtr.Hours,
                TotalPrice = wtr.TotalPrice,
                ActivityId = wtr.ActivityId,
                EmployeeId = wtr.EmployeeId
            })
            .ToListAsync();
    }

    public async Task<WorkTimeRecordViewDto> GetByEmployeeIdAsync(int employeeId) {
        return await _db.WorkTimeRecords
            .Select(wtr => 
                new WorkTimeRecordViewDto {
                    Id = wtr.Id,
                    Date = wtr.Date,
                    Hours = _db.WorkTimeRecords
                        .Sum(wtrSum => wtrSum.Hours),
                    TotalPrice = wtr.TotalPrice,
                    ActivityId = wtr.ActivityId,
                    EmployeeId = wtr.EmployeeId 
                })
            .SingleAsync(wtr => wtr.EmployeeId == employeeId);
    }
}