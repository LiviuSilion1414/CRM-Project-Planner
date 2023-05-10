using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Form;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Views;

namespace PlannerCRM.Server.Services;

public class WorkTimeRecordRepository
{
    private readonly AppDbContext _db;

    public WorkTimeRecordRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(WorkTimeRecordFormDto entity) {
        _db.WorkTimeRecords.Add(new WorkTimeRecord {
            Id = entity.Id,
            Date = entity.Date,
            Hours = entity.Hours,
            TotalPrice = entity.TotalPrice,
            ActivityId = entity.ActivityId,
            EmployeeId = entity.EmployeeId,
            Employee = await _db.Employees
                .Where(e => e.Id == entity.EmployeeId)
                .SingleOrDefaultAsync(),
            WorkOrderId = entity.WorkOrderId
        });

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var entity = await _db.WorkTimeRecords.SingleOrDefaultAsync(w => w.Id == id);
        
        if (entity == null) {
            return;
        }
        _db.WorkTimeRecords.Remove(entity);
        
        await _db.SaveChangesAsync();
    }
    
    public async Task EditAsync(WorkTimeRecordFormDto entity) {
        var model = await _db.WorkTimeRecords.SingleOrDefaultAsync(w => w.Id == entity.Id);

        model.Id = entity.Id;
        model.Date = entity.Date;
        model.Hours = entity.Hours;
        model.TotalPrice = entity.TotalPrice;
        model.ActivityId = entity.ActivityId;
        model.WorkOrderId = entity.WorkOrderId;
        model.EmployeeId = entity.EmployeeId;
        model.Employee = await _db.Employees
            .Where(e => e.Id == entity.EmployeeId)
            .SingleOrDefaultAsync();

        await _db.SaveChangesAsync();
    }

    public async Task<WorkTimeRecordViewDto> GetAsync(int activityId) {
        return await _db.WorkTimeRecords
            .Select(wo => new WorkTimeRecordViewDto {
                Id = wo.Id,
                Date = wo.Date,
                Hours = wo.Hours,
                TotalPrice = wo.TotalPrice,
                ActivityId = wo.ActivityId,
                EmployeeId = wo.EmployeeId,
                WorkOrderId = wo.WorkOrderId})
            .Where(wtr => wtr.ActivityId == activityId)
            .SingleOrDefaultAsync();
    }

    public async Task<List<WorkTimeRecordViewDto>> GetAllAsync() {
        return await _db.WorkTimeRecords
            .Select(wo => new WorkTimeRecordViewDto {
                Id = wo.Id,
                Date = wo.Date,
                Hours = wo.Hours,
                TotalPrice = wo.TotalPrice,
                ActivityId = wo.ActivityId,
                EmployeeId = wo.EmployeeId})
            .ToListAsync();
    }

    public async Task<List<WorkTimeRecordViewDto>> GetAllAsync(int employeeId) {
        return await _db.WorkTimeRecords
            .Where(wo => wo.EmployeeId == employeeId)
            .Select(wo => new WorkTimeRecordViewDto {
                Id = wo.Id,
                Date = wo.Date,
                Hours = wo.Hours,
                TotalPrice = wo.TotalPrice,
                ActivityId = wo.ActivityId,
                EmployeeId = wo.EmployeeId})
            .ToListAsync();
    }

    public async Task<int> GetWorkTimeRecordsSizeByEmployeeId(int employeeId) {
        return await _db.WorkTimeRecords
            .Where(wtr => wtr.EmployeeId == employeeId)
            .CountAsync();
    }
}