using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace PlannerCRM.Server.Services;

public class ActivityRepository
{
    private readonly AppDbContext _db;

    public ActivityRepository(AppDbContext db) {
        _db = db;
    }
    public async Task AddAsync(Activity entity) {
        _db.Activities.Add(entity);
        
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var entity = await _db.Activities.SingleOrDefaultAsync(a => a.Id == id);

        if (entity == null) {
            return;
        }
        _db.Activities.Remove(entity);

        await _db.SaveChangesAsync();
    }

    public async Task<bool> EditAsync(Activity entity) {
        var model = await _db.Activities.SingleOrDefaultAsync(a => a.Id == entity.Id);
        if (model == null) {
            return false;
        }

        model.Id = entity.Id;
        model.Name = entity.Name;
        model.StartDate = entity.StartDate;
        model.FinishDate = entity.FinishDate;
        model.EmployeeActivity = entity.EmployeeActivity;
        model.WorkOrderId = entity.WorkOrderId;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Activity> GetAsync(int id) {
        return await _db.Activities.SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Activity>> GetAllAsync() {
        return await _db.Activities.ToListAsync();
    }
}
