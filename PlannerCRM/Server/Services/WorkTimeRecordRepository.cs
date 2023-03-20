using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace PlannerCRM.Server.Services.ConcreteClasses;

public class WorkTimeRecordRepository
{
    private readonly AppDbContext _db;

    public WorkTimeRecordRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(WorkTimeRecord entity) {
        _db.WorkTimeRecords.Add(entity);
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
    public async Task<bool> EditAsync(WorkTimeRecord entity) {
        var model = await _db.WorkTimeRecords.SingleOrDefaultAsync(w => w.Id == entity.Id);
        
        if (model == null) {
            return false;
        }

        model.Id = entity.Id;
        model.Date = entity.Date;
        model.Hours = entity.Hours;
        model.ActivityId = entity.ActivityId;
        model.WorkOrderId = entity.WorkOrderId;
        model.EmployeeId = entity.EmployeeId;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<WorkTimeRecord> GetAsync(int id) {
        return await _db.WorkTimeRecords
            .SingleOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<WorkTimeRecord>> GetAllAsync() {
        return await _db.WorkTimeRecords
            .Include(wtr => wtr.Employee)
            .ThenInclude(e => e.Salaries)
            .ToListAsync();
    }

    public async Task<List<WorkTimeRecord>> GetAllAsync(int workOrderId) {
        return await _db.WorkTimeRecords
            .Where(wtr => wtr.WorkOrderId == workOrderId)
            .Include(wtr => wtr.Employee)
            .ThenInclude(e => e.Salaries)
            .ToListAsync();
    }
}