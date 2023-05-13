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

    public async Task AddAsync(WorkTimeRecordFormDto workTimeRecordFormDto) {
        _db.WorkTimeRecords.Add(new WorkTimeRecord {
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

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var workTimeRecordDelete = await _db.WorkTimeRecords.SingleOrDefaultAsync(w => w.Id == id);
        
        if (workTimeRecordDelete == null) {
            return;
        }
        _db.WorkTimeRecords.Remove(workTimeRecordDelete);
        
        await _db.SaveChangesAsync();
    }
    
    public async Task EditAsync(WorkTimeRecordFormDto workTimeRecordFormDto) {
        var model = await _db.WorkTimeRecords.SingleOrDefaultAsync(w => w.Id == workTimeRecordFormDto.Id);

        model.Id = workTimeRecordFormDto.Id;
        model.Date = workTimeRecordFormDto.Date;
        model.Hours = workTimeRecordFormDto.Hours;
        model.TotalPrice = workTimeRecordFormDto.TotalPrice;
        model.ActivityId = workTimeRecordFormDto.ActivityId;
        model.WorkOrderId = workTimeRecordFormDto.WorkOrderId;
        model.EmployeeId = workTimeRecordFormDto.EmployeeId;
        model.Employee = await _db.Employees
            .Where(e => e.Id == workTimeRecordFormDto.EmployeeId)
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