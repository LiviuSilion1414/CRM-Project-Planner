using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;

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
            WorkOrderId = entity.WorkOrderId,
            EmployeeActivity = entity.EmployeesActivities
                .Select(e => new EmployeeActivity {
                    EmployeeId = e.Id,
                    ActivityId = entity.Id,
                }).ToList()
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

    public async Task EditAsync(ActivityForm entity) {
        var model = await _db.Activities.SingleOrDefaultAsync(a => a.Id == entity.Id);

        model.Id = entity.Id;
        model.Name = entity.Name;
        model.StartDate = entity.StartDate;
        model.FinishDate = entity.FinishDate;
        model.WorkOrderId = entity.WorkOrderId;
        model.EmployeeActivity = entity.EmployeesActivities
            .Where(es => 
                model.EmployeeActivity
                    .Any(ea => ea.EmployeeId != es.Id))
            .ToList()
            .Select(es => new EmployeeActivity {
                EmployeeId = es.Id,
                ActivityId = entity.Id
            })
            .ToList();
        
        await _db.SaveChangesAsync();
    }

    public async Task<ActivityViewDTO> GetForViewAsync(int id) {
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

    public async Task<ActivityForm> GetForEditAsync(int id) {
        return await _db.Activities
            .Select(e => new ActivityForm {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                FinishDate = e.FinishDate,
                WorkOrderId = e.WorkOrderId,
                EmployeesActivities = e.EmployeeActivity
                    .Select(ea => new EmployeeSelectDTO {
                        Id = ea.EmployeeId,
                        Email = ea.Employee.Email
                    })
                    .ToList()   
            })
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<ActivityDeleteDTO> GetForDeleteAsync(int id) {
        return await _db.Activities
            .Select(e => new ActivityDeleteDTO {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                FinishDate = e.FinishDate
            })
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<ActivityForm>> GetActivityByJuniorEmployeeId(int employeeId) {
         return await _db.Activities
            .Select(ac => new ActivityForm {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeesActivities = ac.EmployeeActivity
                    .Select(ea => new EmployeeSelectDTO {
                        Id = ea.EmployeeId,
                        Email = ea.Employee.Email
                    })
                    .ToList()   
            })
            .Where(ac => _db.EmployeeActivity
                .Any(ea => ea.EmployeeId == employeeId && ac.Id == ea.ActivityId))
            .ToListAsync();
    }

    public async Task<List<ActivityForm>> GetActivitiesPerWorkOrderAsync(int workorderId) {
        return await _db.Activities
            .Select(ac => new ActivityForm {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeesActivities = ac.EmployeeActivity
                    .Select(ea => new EmployeeSelectDTO {
                        Id = ea.EmployeeId,
                        Email = ea.Employee.Email
                    })
                    .ToList()   
            })
            .Where(ac => ac.WorkOrderId == workorderId)
            .ToListAsync();
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
