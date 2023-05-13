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

    public async Task AddAsync(ActivityFormDto activityFormDto) {
        _db.Activities.Add(new Activity {
            Id = activityFormDto.Id,
            Name = activityFormDto.Name,
            StartDate = activityFormDto.StartDate ?? throw new NullReferenceException(),
            FinishDate = activityFormDto.FinishDate ?? throw new NullReferenceException(),
            WorkOrderId = activityFormDto.WorkOrderId ?? throw new NullReferenceException(),
            EmployeeActivity = (activityFormDto.EmployeesActivities
                .Select(e => new EmployeeActivity {
                    Id = e.Id,
                    ActivityId = e.ActivityId,
                    Activity = new Activity {
                        Id = activityFormDto.Id,
                        Name = activityFormDto.Name,
                        StartDate = activityFormDto.StartDate ?? throw new NullReferenceException(),
                        FinishDate = activityFormDto.FinishDate ?? throw new NullReferenceException(),
                        WorkOrderId = activityFormDto.WorkOrderId ?? throw new NullReferenceException() 
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
        var activityDelete = await _db.Activities.SingleOrDefaultAsync(a => a.Id == id);

        if (activityDelete == null) {
            return;
        }
        _db.Activities.Remove(activityDelete);

        await _db.SaveChangesAsync();
    }

    public async Task EditAsync(ActivityFormDto activityFormDto) {
        var model = await _db.Activities.SingleOrDefaultAsync(a => a.Id == activityFormDto.Id);

        model.Id = activityFormDto.Id;
        model.Name = activityFormDto.Name;
        model.StartDate = activityFormDto.StartDate ?? throw new NullReferenceException();
        model.FinishDate = activityFormDto.FinishDate ?? throw new NullReferenceException();
        model.WorkOrderId = activityFormDto.WorkOrderId ?? throw new NullReferenceException();
        model.EmployeeActivity = activityFormDto.EmployeesActivities
            .Where(es => 
                model.EmployeeActivity
                    .Any(ea => ea.EmployeeId != es.Id))
            .ToList()
            .Select(es => new EmployeeActivity {
                EmployeeId = es.Id,
                ActivityId = activityFormDto.Id
            })
            .ToList();
        
        await _db.SaveChangesAsync();
    }

    public async Task<ActivityViewDto> GetForViewAsync(int id) {
        return await _db.Activities
            .Select(e => new ActivityViewDto {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                FinishDate = e.FinishDate,
                WorkOrderId = e.WorkOrderId,
            })
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<ActivityFormDto> GetForEditAsync(int id) {
        return await _db.Activities
            .Select(e => new ActivityFormDto {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                FinishDate = e.FinishDate,
                WorkOrderId = e.WorkOrderId,
                EmployeesActivities = e.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Id,
                        EmployeeId = ea.Employee.Id,
                        Employee = new EmployeeSelectDto {
                            Id = ea.Employee.Id,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            Role = ea.Employee.Role
                        },
                        ActivityId = ea.ActivityId,
                        Activity = new ActivitySelectDto {
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

    public async Task<ActivityDeleteDto> GetForDeleteAsync(int id) {
        return await _db.Activities
            .Select(e => new ActivityDeleteDto {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                FinishDate = e.FinishDate
            })
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<ActivityFormDto>> GetActivityByEmployeeId(int employeeId) {
         return await _db.Activities
            .Select(ac => new ActivityFormDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeesActivities = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.EmployeeId,
                        EmployeeId = ea.Employee.Id,
                        Employee = new EmployeeSelectDto {
                            Id = ea.Employee.Id,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            Role = ea.Employee.Role
                        },
                        ActivityId = ea.ActivityId,
                        Activity = new ActivitySelectDto {
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

    public async Task<List<ActivityFormDto>> GetActivitiesPerWorkOrderAsync(int workorderId) {
        return await _db.Activities
            .Select(ac => new ActivityFormDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeesActivities = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Id,
                        EmployeeId = ea.Employee.Id,
                        Employee = new EmployeeSelectDto {
                            Id = ea.Employee.Id,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            Role = ea.Employee.Role
                        },
                        ActivityId = ea.ActivityId,
                        Activity = new ActivitySelectDto {
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

    public async Task<List<ActivityViewDto>> GetAllAsync() {
        return await _db.Activities
            .Select(e => new ActivityViewDto {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                FinishDate = e.FinishDate,
                WorkOrderId = e.WorkOrderId
            })
            .ToListAsync();
    }
}
