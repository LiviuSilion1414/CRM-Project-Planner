using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;

namespace PlannerCRM.Server.Services;

public class ActivityRepository
{
    private readonly AppDbContext _db;

    public ActivityRepository(AppDbContext db) {
        _db = db;
    }
    public async Task AddAsync(ActivityForm entity) {
        _db.Activities.Add(new Activity {
            Id = entity.Id,
            Name = entity.Name,
            StartDate = entity.StartDate,
            FinishDate = entity.FinishDate,
            WorkOrderId = entity.WorkorderId
        });
        
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

    public async Task<bool> EditAsync(ActivityForm entity) {
        var model = await _db.Activities.SingleOrDefaultAsync(a => a.Id == entity.Id);
        if (model == null) {
            return false;
        }

        model.Id = entity.Id;
        model.Name = entity.Name;
        model.StartDate = entity.StartDate;
        model.FinishDate = entity.FinishDate;
        model.WorkOrderId = entity.WorkorderId;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<ActivityViewDTO> GetAsync(int id) {
        return await _db.Activities
            .Select(e => new ActivityViewDTO {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                FinishDate = e.FinishDate,
                WorkOrderId = e.WorkOrderId
            })
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<ActivityViewDTO>> GetAllAsync() {
        return await _db.Activities
            .Select(e => new ActivityViewDTO {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                FinishDate = e.FinishDate,
                WorkOrderId = e.WorkOrderId
            })
            .ToListAsync();
    }
}
