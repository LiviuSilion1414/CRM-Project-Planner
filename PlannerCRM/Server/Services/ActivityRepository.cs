using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Server.CustomExceptions;

namespace PlannerCRM.Server.Services;

public class ActivityRepository
{
    private readonly AppDbContext _db;

    public ActivityRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(ActivityFormDto activityFormDto) {
        if (activityFormDto.GetType() == null) {
            throw new NullReferenceException("Oggetto null.");
        }

        var innerPropertiesAreNull = activityFormDto.GetType().GetProperties()
            .All(prop => prop.GetValue(activityFormDto) != null);
        if (innerPropertiesAreNull) {
            throw new ArgumentNullException("Parametri null");
        }
        
        var isAlreadyPresent = await _db.Activities
            .SingleOrDefaultAsync(ac => ac.Id == activityFormDto.Id);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException("Oggetto già presente");
        }

        await _db.Activities.AddAsync(new Activity {
            Id = activityFormDto.Id,
            Name = activityFormDto.Name,
            StartDate = activityFormDto.StartDate ?? throw new NullReferenceException(),
            FinishDate = activityFormDto.FinishDate ?? throw new NullReferenceException(),
            WorkOrderId = activityFormDto.WorkOrderId ?? throw new NullReferenceException(),
            EmployeeActivity = activityFormDto.EmployeesActivities
                .Select(ea => new EmployeeActivity {
                    Id = ea.Id,
                    ActivityId = ea.ActivityId,
                    Activity = new Activity {
                        Id = activityFormDto.Id,
                        Name = activityFormDto.Name,
                        StartDate = activityFormDto.StartDate ?? throw new NullReferenceException(),
                        FinishDate = activityFormDto.FinishDate ?? throw new NullReferenceException(),
                        WorkOrderId = activityFormDto.WorkOrderId ?? throw new NullReferenceException() 
                    },
                    EmployeeId = ea.EmployeeId,
                    Employee = new Employee {
                        Email = ea.Employee.Email,
                        FirstName = ea.Employee.FirstName,
                        LastName = ea.Employee.LastName,
                        Role = ea.Employee.Role,
                        Salaries = ea.Employee.EmployeeSalaries
                            .Select(ems => new List<EmployeeSalary> {
                                new EmployeeSalary {
                                    Id = ems.Id,
                                    EmployeeId = ems.EmployeeId,
                                    StartDate = ems.StartDate,
                                    FinishDate = ems.FinishDate,
                                    Salary = decimal.Parse(ems.Salary.ToString()),
                                }})
                            .First(),
                    }
                })
            .ToList()
        });

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile salvare i dati.");
        }
    }

    public async Task DeleteAsync(int id) {
        var activityDelete = await _db.Activities
            .SingleOrDefaultAsync(ac => ac.Id == id);

        if (activityDelete == null) {
            throw new InvalidOperationException("Impossibile eliminare l'elemento");
        }
        _db.Activities.Remove(activityDelete);

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile salvare i dati.");
        }
    }

    public async Task EditAsync(ActivityFormDto activityFormDto) {
        if (activityFormDto == null) {
            throw new NullReferenceException("Oggetto null.");
        }

        var isNull = activityFormDto.GetType().GetProperties()
            .All(prop => prop.GetValue(activityFormDto) != null);
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }
        
        var model = await _db.Activities
            .SingleOrDefaultAsync(ac => ac.Id == activityFormDto.Id);

        if (model == null) {
            throw new KeyNotFoundException("Oggetto non trovato");
        }

        model.Id = activityFormDto.Id;
        model.Name = activityFormDto.Name;
        model.StartDate = activityFormDto.StartDate ?? throw new NullReferenceException();
        model.FinishDate = activityFormDto.FinishDate ?? throw new NullReferenceException();
        model.WorkOrderId = activityFormDto.WorkOrderId ?? throw new NullReferenceException();
        model.EmployeeActivity = activityFormDto.EmployeesActivities
            .Where(ea => 
                model.EmployeeActivity
                    .Any(ea => ea.EmployeeId != ea.Id))
            .Select(ea => new EmployeeActivity {
                EmployeeId = ea.Id,
                ActivityId = activityFormDto.Id
            })
            .ToList();
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile salvare i dati.");
        }
    }

    public async Task<ActivityViewDto> GetForViewAsync(int id) {
        return await _db.Activities
            .Select(ac => new ActivityViewDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
            })
            .SingleOrDefaultAsync(ac => ac.Id == id) ?? throw new Exception() ?? throw new FormatException();
    }

    public async Task<ActivityFormDto> GetForEditAsync(int id) {
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
            .SingleOrDefaultAsync(ac => ac.Id == id);
    }

    public async Task<ActivityDeleteDto> GetForDeleteAsync(int id) {
        return await _db.Activities
            .Select(ac => new ActivityDeleteDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate
            })
            .SingleOrDefaultAsync(ac => ac.Id == id);
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

    public async Task<List<ActivityFormDto>> GetActivitiesPerWorkOrderAsync(int workOrderId) {
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
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetAllAsync() {
        return await _db.Activities
            .Select(ac => new ActivityViewDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId
            })
            .ToListAsync();
    }
}
