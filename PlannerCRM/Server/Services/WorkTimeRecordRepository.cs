using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Form;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Views;
using PlannerCRM.Server.CustomExceptions;

namespace PlannerCRM.Server.Services;

public class WorkTimeRecordRepository
{
    private readonly AppDbContext _db;

    public WorkTimeRecordRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(WorkTimeRecordFormDto workTimeRecordFormDto) {
        if (workTimeRecordFormDto.GetType() == null) {
            throw new NullReferenceException("Oggetto null.");
        }

        var isNull = workTimeRecordFormDto.GetType().GetProperties()
            .All(prop => prop.GetValue(workTimeRecordFormDto) != null);
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }
        
        var isAlreadyPresent = await _db.Employees
            .SingleOrDefaultAsync(workTimeRec => workTimeRec.Id == workTimeRec.Id);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException("Oggetto già presente");
        }

        await _db.WorkTimeRecords.AddAsync(new WorkTimeRecord {
            Id = workTimeRecordFormDto.Id,
            Date = workTimeRecordFormDto.Date,
            Hours = workTimeRecordFormDto.Hours,
            TotalPrice = workTimeRecordFormDto.TotalPrice,
            ActivityId = workTimeRecordFormDto.ActivityId,
            EmployeeId = workTimeRecordFormDto.EmployeeId,
            Employee = await _db.Employees
                .Where(e => e.Id == workTimeRecordFormDto.EmployeeId)
                .SingleOrDefaultAsync(),
            WorkOrderId = workTimeRecordFormDto.WorkOrderId
        });

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile salvare i dati.");
        }
    }

    public async Task DeleteAsync(int id) {
        var workTimeRecordDelete = await _db.WorkTimeRecords
            .SingleOrDefaultAsync(wtr => wtr.Id == id);
        
        if (workTimeRecordDelete == null) {
            throw new KeyNotFoundException("Oggetto non trovato");
        }
        _db.WorkTimeRecords.Remove(workTimeRecordDelete);
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile salvare i dati.");
        }
    }
    
    public async Task EditAsync(WorkTimeRecordFormDto workTimeRecordFormDto) {
        if (workTimeRecordFormDto == null) {
            throw new NullReferenceException("Oggetto null.");
        }

        var isNull = workTimeRecordFormDto.GetType().GetProperties()
            .All(prop => prop.GetValue(workTimeRecordFormDto) != null);
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }

        var model = await _db.WorkTimeRecords
            .SingleOrDefaultAsync(wtr => wtr.Id == workTimeRecordFormDto.Id);
        if (model == null) {
            throw new KeyNotFoundException("Oggetto non trovato");
        }

        model.Id = workTimeRecordFormDto.Id;
        model.Date = workTimeRecordFormDto.Date;
        model.Hours = workTimeRecordFormDto.Hours;
        model.TotalPrice = workTimeRecordFormDto.TotalPrice;
        model.ActivityId = workTimeRecordFormDto.ActivityId;
        model.WorkOrderId = workTimeRecordFormDto.WorkOrderId;
        model.EmployeeId = workTimeRecordFormDto.EmployeeId;
        model.Employee = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == workTimeRecordFormDto.EmployeeId);

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile proseguire.");
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