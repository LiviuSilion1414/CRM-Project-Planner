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
            StartDate = entity.StartDate ?? throw new NullReferenceException(),
            FinishDate = entity.FinishDate ?? throw new NullReferenceException(),
            WorkOrderId = entity.WorkOrderId ?? throw new NullReferenceException(),
            EmployeeActivity = (entity.EmployeesActivities
                .Select(e => new EmployeeActivity {
                    Id = e.Id,
                    ActivityId = e.ActivityId,
                    Activity = new Activity {
                        Id = entity.Id,
                        Name = entity.Name,
                        StartDate = entity.StartDate ?? throw new NullReferenceException(),
                        FinishDate = entity.FinishDate ?? throw new NullReferenceException(),
                        WorkOrderId = entity.WorkOrderId ?? throw new NullReferenceException() 
                    },
                    EmployeeId = e.EmployeeId,
                    Employee = new Employee {
                            Email = e.Employee.Email,
                            FirstName = e.Employee.FirstName,
                            LastName = e.Employee.LastName,
                            Role = e.Employee.Role,
                            Salaries = e.Employee.EmployeeSalaries
                                .Select(ems => new List<EmployeeSalary> {
                                    new EmployeeSalary {
                                        Id = ems.Id,
                                        EmployeeId = ems.EmployeeId,
                                        StartDate = ems.StartDate,
                                        FinishDate = ems.FinishDate,
                                        Salary = decimal.Parse(ems.Salary.ToString()),
                                    }})
                                .ToList()
                                .First(),
                    }
                })
                
            .ToList())
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
        model.StartDate = entity.StartDate ?? throw new NullReferenceException();
        model.FinishDate = entity.FinishDate ?? throw new NullReferenceException();
        model.WorkOrderId = entity.WorkOrderId ?? throw new NullReferenceException();
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
                WorkOrderId = e.WorkOrderId,
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
                    .Select(ea => new EmployeeActivityDTO {
                        Id = ea.Id,
                        EmployeeId = ea.Employee.Id,
                        Employee = new EmployeeSelectDTO {
                            Id = ea.Employee.Id,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            Role = ea.Employee.Role
                        },
                        ActivityId = ea.ActivityId,
                        Activity = new ActivitySelectDTO {
                            Id = ea.Activity.Id,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        }
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
                    .Select(ea => new EmployeeActivityDTO {
                        Id = ea.EmployeeId,
                        EmployeeId = ea.Employee.Id,
                        Employee = new EmployeeSelectDTO {
                            Id = ea.Employee.Id,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            Role = ea.Employee.Role
                        },
                        ActivityId = ea.ActivityId,
                        Activity = new ActivitySelectDTO {
                            Id = ea.Activity.Id,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        }
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
                    .Select(ea => new EmployeeActivityDTO {
                        Id = ea.Id,
                        EmployeeId = ea.Employee.Id,
                        Employee = new EmployeeSelectDTO {
                            Id = ea.Employee.Id,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            Role = ea.Employee.Role
                        },
                        ActivityId = ea.ActivityId,
                        Activity = new ActivitySelectDTO {
                            Id = ea.Activity.Id,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        }
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
