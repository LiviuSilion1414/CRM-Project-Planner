using PlannerCRM.Server.Services.Interfaces;
using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace PlannerCRM.Server.Services.ConcreteClasses;

class WorkOrderRepository : IRepository<WorkOrder>
{
    private readonly AppDbContext _db;

    public WorkOrderRepository(AppDbContext db) {
        _db = db;
    }
    public async Task Add(WorkOrder entity) {
        _db.WorkOrders.Add(entity);

        await _db.SaveChangesAsync();
    }

    public async Task Delete(int id) {
        var entity = await _db.WorkOrders.SingleOrDefaultAsync(w => w.Id == id);
        
        if (entity == null) {
            return;
        }
        _db.WorkOrders.Remove(entity);
        
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Edit(int id, WorkOrder entity) {
        var model = await _db.WorkOrders.SingleOrDefaultAsync(w => w.Id == id);
        
        if (model == null) {
            return false;
        }

        model.Id = entity.Id;
        model.Name = entity.Name;
        model.StartDate = entity.StartDate;
        model.Activities = entity.Activities; 

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<WorkOrder> Get(int id) {
        return await _db.WorkOrders
            .Include(wo => wo.Activities)
            .ThenInclude(a => a.EmployeeActivity)
            .SingleOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<WorkOrder>> GetAll() {
        return await _db.WorkOrders
            .Include(a => a.Activities)
            .ToListAsync();
    }
}