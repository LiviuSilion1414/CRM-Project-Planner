using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Form;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Views;
using PlannerCRM.Shared.CustomExceptions;
using static PlannerCRM.Shared.Constants.ExceptionsMessages;

namespace PlannerCRM.Server.Services;

public class WorkTimeRecordRepository
{
    private readonly AppDbContext _db;

    public WorkTimeRecordRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(WorkTimeRecordFormDto dto) {
        if (dto.GetType() == null) {
            throw new NullReferenceException(NULL_OBJECT);
        }

        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        if (HasPropertiesNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }
        
        var isAlreadyPresent = await _db.Employees
            .SingleOrDefaultAsync(workTimeRec => workTimeRec.Id == workTimeRec.Id);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException(OBJECT_ALREADY_PRESENT);
        }

        await _db.WorkTimeRecords.AddAsync(new WorkTimeRecord {
            Id = dto.Id,
            Date = dto.Date,
            Hours = dto.Hours,
            TotalPrice = dto.TotalPrice,
            ActivityId = dto.ActivityId,
            EmployeeId = dto.EmployeeId,
            Employee = await _db.Employees
                .Where(e => e.Id == dto.EmployeeId)
                .SingleOrDefaultAsync(),
            WorkOrderId = dto.WorkOrderId
        });

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBLE_SAVE_CHANGES);
        }
    }

    public async Task DeleteAsync(int id) {
        var workTimeRecordDelete = await _db.WorkTimeRecords
            .SingleOrDefaultAsync(wtr => wtr.Id == id);
        
        if (workTimeRecordDelete == null) {
            throw new KeyNotFoundException(OBJECT_NOT_FOUND);
        }
        _db.WorkTimeRecords.Remove(workTimeRecordDelete);
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBLE_SAVE_CHANGES);
        }
    }
    
    public async Task EditAsync(WorkTimeRecordFormDto dto) {
        if (dto == null) {
            throw new NullReferenceException(NULL_OBJECT);
        }

        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        if (HasPropertiesNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }

        var model = await _db.WorkTimeRecords
            .SingleOrDefaultAsync(wtr => wtr.Id == dto.Id);
        if (model == null) {
            throw new KeyNotFoundException(OBJECT_NOT_FOUND);
        }

        model.Id = dto.Id;
        model.Date = dto.Date;
        model.Hours = dto.Hours;
        model.TotalPrice = dto.TotalPrice;
        model.ActivityId = dto.ActivityId;
        model.WorkOrderId = dto.WorkOrderId;
        model.EmployeeId = dto.EmployeeId;
        model.Employee = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == dto.EmployeeId);

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBILE_GOING_FORWARD);
        }
    }

    public async Task<WorkTimeRecordViewDto> GetAsync(int activityId) {
        return await _db.WorkTimeRecords
            .Select(wtr => new WorkTimeRecordViewDto {
                Id = wtr.Id,
                Date = wtr.Date,
                Hours = wtr.Hours,
                TotalPrice = wtr.TotalPrice,
                ActivityId = wtr.ActivityId,
                EmployeeId = wtr.EmployeeId,
                WorkOrderId = wtr.WorkOrderId})
            .SingleOrDefaultAsync(wtr => wtr.ActivityId == activityId);
    }

    public async Task<List<WorkTimeRecordViewDto>> GetAllAsync() {
        return await _db.WorkTimeRecords
            .Select(wtr => new WorkTimeRecordViewDto {
                Id = wtr.Id,
                Date = wtr.Date,
                Hours = wtr.Hours,
                TotalPrice = wtr.TotalPrice,
                ActivityId = wtr.ActivityId,
                EmployeeId = wtr.EmployeeId})
            .ToListAsync();
    }

    public async Task<List<WorkTimeRecordViewDto>> GetAllAsync(int employeeId) {
        return await _db.WorkTimeRecords
            .Where(wtr => wtr.EmployeeId == employeeId)
            .Select(wtr => new WorkTimeRecordViewDto {
                Id = wtr.Id,
                Date = wtr.Date,
                Hours = wtr.Hours,
                TotalPrice = wtr.TotalPrice,
                ActivityId = wtr.ActivityId,
                EmployeeId = wtr.EmployeeId})
            .ToListAsync();
    }

    public async Task<int> GetWorkTimeRecordsSizeByEmployeeId(int employeeId) {
        return await _db.WorkTimeRecords
            .Where(wtr => wtr.EmployeeId == employeeId)
            .CountAsync();
    }
}