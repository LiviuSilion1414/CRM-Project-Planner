using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Server.CustomExceptions;
using static PlannerCRM.Shared.Constants.ExceptionsMessages;

namespace PlannerCRM.Server.Services;

public class ActivityRepository
{
    private readonly AppDbContext _db;

    public ActivityRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(ActivityFormDto dto) {
        if (dto.GetType() == null) {
            throw new NullReferenceException(NULL_OBJECT);
        }

        var innerPropertiesAreNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        if (innerPropertiesAreNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }
        
        var isAlreadyPresent = await _db.Activities
            .SingleOrDefaultAsync(ac => ac.Id == dto.Id);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException(OBJECT_ALREADY_PRESENT);
        }

        await _db.Activities.AddAsync(new Activity {
            Id = dto.Id,
            Name = dto.Name,
            StartDate = dto.StartDate ?? throw new NullReferenceException(NULL_PROP),
            FinishDate = dto.FinishDate ?? throw new NullReferenceException(NULL_PROP),
            WorkOrderId = dto.WorkOrderId ?? throw new NullReferenceException(NULL_PROP),
            EmployeeActivity = dto.EmployeesActivities
                .Select(ea => new EmployeeActivity {
                    Id = ea.Id,
                    ActivityId = ea.ActivityId,
                    Activity = new Activity {
                        Id = dto.Id,
                        Name = dto.Name,
                        StartDate = dto.StartDate ?? throw new NullReferenceException(NULL_PROP),
                        FinishDate = dto.FinishDate ?? throw new NullReferenceException(NULL_PROP),
                        WorkOrderId = dto.WorkOrderId ?? throw new NullReferenceException(NULL_PROP)
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
            throw new DbUpdateException(IMPOSSIBLE_SAVE_CHANGES);
        }
    }

    public async Task DeleteAsync(int id) {
        var activityDelete = await _db.Activities
            .SingleOrDefaultAsync(ac => ac.Id == id);

        if (activityDelete == null) {
            throw new InvalidOperationException(IMPOSSIBLE_DELETE);
        }
        _db.Activities.Remove(activityDelete);

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBLE_SAVE_CHANGES);
        }
    }

    public async Task EditAsync(ActivityFormDto dto) {
        if (dto == null) {
            throw new NullReferenceException(NULL_OBJECT);
        }

        var isNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        if (isNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }
        
        var model = await _db.Activities
            .SingleOrDefaultAsync(ac => ac.Id == dto.Id);

        if (model == null) {
            throw new KeyNotFoundException(OBJECT_NOT_FOUND);
        }

        model.Id = dto.Id;
        model.Name = dto.Name;
        model.StartDate = dto.StartDate ?? throw new NullReferenceException(NULL_PROP);
        model.FinishDate = dto.FinishDate ?? throw new NullReferenceException(NULL_PROP);
        model.WorkOrderId = dto.WorkOrderId ?? throw new NullReferenceException(NULL_PROP);
        model.EmployeeActivity = dto.EmployeesActivities
            .Where(ea => 
                model.EmployeeActivity
                    .Any(ea => ea.EmployeeId != ea.Id))
            .Select(ea => new EmployeeActivity {
                EmployeeId = ea.Id,
                ActivityId = dto.Id
            })
            .ToList();
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBLE_SAVE_CHANGES);
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
            .SingleOrDefaultAsync(ac => ac.Id == id);
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
