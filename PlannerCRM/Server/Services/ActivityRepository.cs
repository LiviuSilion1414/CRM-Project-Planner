using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Server.DataAccess;
using PlannerCRM.Shared.Constants;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Services;

public class ActivityRepository
{
    private readonly AppDbContext _db;

    public ActivityRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(ActivityAddFormDto dto) {
        if (dto.GetType() is null) 
            throw new NullReferenceException(ExceptionsMessages.NULL_OBJECT);

        var innerPropertiesAreNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) is null);
        if (innerPropertiesAreNull) 
            throw new ArgumentNullException(ExceptionsMessages.NULL_PARAM);
        
        var isAlreadyPresent = await _db.Activities
            .SingleOrDefaultAsync(ac => ac.Id == dto.Id);
        if (isAlreadyPresent != null) 
            throw new DuplicateElementException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);

        if (dto.EmployeeActivity is null || !dto.EmployeeActivity.Any())
            throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
        
        var entity = new Activity {
            Id = dto.Id,
            Name = dto.Name,
            StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
            FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
            WorkOrderId = dto.WorkOrderId ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
            EmployeeActivity = dto.EmployeeActivity
                .Select(ea => new EmployeeActivity {
                    Id = ea.Id,
                    EmployeeId = ea.EmployeeId,
                    ActivityId = dto.Id,
                }).ToHashSet(),
        };

        await _db.Activities.AddAsync(entity);

        var workOrder = await _db.WorkOrders
            .SingleAsync(wo => wo.Id == dto.WorkOrderId);
        workOrder.Activities.Add(entity);    

        _db.Update(workOrder);
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0)
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
    }

    public async Task DeleteAsync(int id) {
        var activityDelete = await _db.Activities
            .SingleOrDefaultAsync(ac => ac.Id == id);

        if (activityDelete is null)
            throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_DELETE);
        
        await _db.EmployeeActivity
            .Where(ea => ea.ActivityId == activityDelete.Id)
            .ForEachAsync(ea => 
                _db.EmployeeActivity
                    .Remove(ea)
            );

        _db.Activities.Remove(activityDelete);

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) 
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
    }

    public async Task EditAsync(ActivityEditFormDto dto) {
        if (dto is null)
            throw new NullReferenceException(ExceptionsMessages.NULL_OBJECT);

        if (dto.EmployeeActivity is null || !dto.ViewEmployeeActivity.Any())
            throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
        
        var model = await _db.Activities
            .SingleOrDefaultAsync(ac => ac.Id == dto.Id);

        if (model is null)
            throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);

        model.Id = dto.Id;
        model.Name = dto.Name;
        model.StartDate = dto.StartDate;
        model.FinishDate = dto.FinishDate;
        model.WorkOrderId = dto.WorkOrderId;
        model.EmployeeActivity = dto.EmployeeActivity
            .Where(eaDto => _db.EmployeeActivity
                .Any(ea => eaDto.EmployeeId != ea.EmployeeId))
            .Select(ea => new EmployeeActivity {
                EmployeeId = ea.EmployeeId,     
                ActivityId = ea.ActivityId,
            })
            .ToHashSet();

        var workOrder = await _db.WorkOrders
            .SingleAsync(wo => wo.Id == dto.WorkOrderId);
        
        var activity = workOrder.Activities.Find(ac => ac.Id == dto.Id);
        activity.Id = dto.Id;
        activity.Name = dto.Name;
        activity.StartDate = dto.StartDate;
        activity.FinishDate = dto.FinishDate;
        activity.WorkOrderId = dto.WorkOrderId;
        activity.EmployeeActivity = dto.EmployeeActivity
            .Select(ea => new EmployeeActivity {
                Id = ea.Id,
                EmployeeId = ea.EmployeeId,
                ActivityId = ea.ActivityId,
            }).ToHashSet();

        _db.Update(model);
        _db.Update(workOrder);

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0)
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
    }

    public async Task<ActivityViewDto> GetForViewAsync(int id) {
        return await _db.Activities
            .Select(ac => new ActivityViewDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _db.Employees
                            .Select(em => new EmployeeSelectDto {
                                Id = ea.EmployeeId,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                FullName = ea.Employee.FullName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                EmployeeSalaries = ea.Employee.Salaries
                                    .Select(sal => new EmployeeSalaryDto {
                                        Id = sal.Id,
                                        EmployeeId = ea.EmployeeId,
                                        StartDate = sal.StartDate,
                                        FinishDate = sal.FinishDate,
                                        Salary = sal.Salary,
                                    }).ToList()
                            })
                            .Single(em => em.Id == ea.EmployeeId), 
                        ActivityId = ea.ActivityId,
                        Activity = _db.Activities
                            .Select(a => new ActivitySelectDto {
                                Id = ea.ActivityId,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            })
                            .Single(ac => ac.Id == ea.ActivityId) 
                    }).ToHashSet()
            })
            .SingleOrDefaultAsync(ac => ac.Id == id);
    }

    public async Task<ActivityEditFormDto> GetForEditAsync(int activityId) {
        return await _db.Activities
            .Select(ac => new ActivityEditFormDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeeActivity = new(),
                ViewEmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _db.Employees
                            .Select(em => new EmployeeSelectDto {
                                Id = ea.EmployeeId,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                FullName = ea.Employee.FullName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                EmployeeSalaries = ea.Employee.Salaries
                                    .Select(sal => new EmployeeSalaryDto {
                                        Id = sal.Id,
                                        EmployeeId = ea.EmployeeId,
                                        StartDate = sal.StartDate,
                                        FinishDate = sal.FinishDate,
                                        Salary = sal.Salary,
                                    }).ToList()
                            })
                            .Single(em => em.Id == ea.EmployeeId), 
                        ActivityId = ea.ActivityId,
                        Activity = _db.Activities
                            .Select(a => new ActivitySelectDto {
                                Id = ea.ActivityId,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            })
                            .Single(ac => ac.Id == ea.ActivityId)
                    })
                    .ToHashSet()
            })
            .SingleAsync(ac => ac.Id == activityId);
    }

    public async Task<ActivityDeleteDto> GetForDeleteAsync(int id) {
        return await _db.Activities
            .Select(ac => new ActivityDeleteDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                Employees = _db.EmployeeActivity
                    .Where(ea => ea.ActivityId == id)
                    .Select(ea => new EmployeeSelectDto() {
                        Id = ea.EmployeeId,
                        Email = ea.Employee.Email,
                        FirstName = ea.Employee.FirstName,
                        LastName = ea.Employee.LastName,
                        FullName = ea.Employee.FullName,
                        Role = ea.Employee.Role,
                        CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                        EmployeeSalaries = ea.Employee.Salaries
                            .Select(sal => new EmployeeSalaryDto {
                                Id = sal.Id,
                                EmployeeId = ea.EmployeeId,
                                StartDate = sal.StartDate,
                                FinishDate = sal.FinishDate,
                                Salary = sal.Salary,
                            }).ToList(),
                        EmployeeActivities = ac.EmployeeActivity
                            .Select(ea => new EmployeeActivityDto {
                                Id = ea.Id,
                                EmployeeId = ea.EmployeeId,
                                Employee = _db.Employees
                                    .Select(em => new EmployeeSelectDto {
                                        Id = ea.EmployeeId,
                                        Email = ea.Employee.Email,
                                        FirstName = ea.Employee.FirstName,
                                        LastName = ea.Employee.LastName,
                                        FullName = ea.Employee.FullName,
                                        Role = ea.Employee.Role,
                                        CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                        EmployeeSalaries = ea.Employee.Salaries
                                            .Select(sal => new EmployeeSalaryDto {
                                                Id = sal.Id,
                                                EmployeeId = ea.EmployeeId,
                                                StartDate = sal.StartDate,
                                                FinishDate = sal.FinishDate,
                                                Salary = sal.Salary,
                                            }).ToList()
                                    })
                                    .Single(em => em.Id == ea.EmployeeId),  
                                ActivityId = ea.ActivityId,
                                Activity = _db.Activities
                                    .Select(a => new ActivitySelectDto {
                                        Id = ea.ActivityId,
                                        Name = ea.Activity.Name,
                                        StartDate = ea.Activity.StartDate,
                                        FinishDate = ea.Activity.FinishDate,
                                        WorkOrderId = ea.Activity.WorkOrderId
                                    })
                                    .Single(ac => ac.Id == ea.ActivityId)
                            }).ToList()
                    })
                    .ToHashSet()
            })
            .SingleOrDefaultAsync(ac => ac.Id == id);
    }

    public async Task<List<ActivityEditFormDto>> GetActivityByEmployeeId(int employeeId) {
         return await _db.Activities
            .Select(ac => new ActivityEditFormDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _db.Employees
                            .Select(em => new EmployeeSelectDto {
                                Id = ea.EmployeeId,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                FullName = ea.Employee.FullName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                EmployeeSalaries = ea.Employee.Salaries
                                    .Select(sal => new EmployeeSalaryDto {
                                        Id = sal.Id,
                                        EmployeeId = ea.EmployeeId,
                                        StartDate = sal.StartDate,
                                        FinishDate = sal.FinishDate,
                                        Salary = sal.Salary,
                                    }).ToList()
                            })
                            .Single(em => em.Id == ea.EmployeeId), 
                        ActivityId = ea.ActivityId,
                        Activity = _db.Activities
                            .Select(a => new ActivitySelectDto {
                                Id = ea.ActivityId,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            })
                            .Single(ac => ac.Id == ea.ActivityId)
                    }).ToHashSet()   
            })
            .Where(ac => _db.EmployeeActivity
                .Any(ea => ea.EmployeeId == employeeId && ac.Id == ea.ActivityId))
            .ToListAsync();
    }

    public async Task<List<ActivityEditFormDto>> GetActivitiesPerWorkOrderAsync(int workOrderId) {
        return await _db.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .Select(ac => new ActivityEditFormDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _db.Employees
                            .Select(em => new EmployeeSelectDto {
                                Id = ea.EmployeeId,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                FullName = ea.Employee.FullName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                EmployeeSalaries = ea.Employee.Salaries
                                    .Select(sal => new EmployeeSalaryDto {
                                        Id = sal.Id,
                                        EmployeeId = ea.EmployeeId,
                                        StartDate = sal.StartDate,
                                        FinishDate = sal.FinishDate,
                                        Salary = sal.Salary,
                                    }).ToList()
                            })
                            .Single(em => em.Id == ea.EmployeeId),
                        ActivityId = ea.ActivityId,
                        Activity = _db.Activities
                            .Select(a => new ActivitySelectDto {
                                Id = ea.ActivityId,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            })
                            .Single(ac => ac.Id == ea.ActivityId) 
                    }).ToHashSet()   
            })
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetAllAsync() {
        return await _db.Activities
            .Select(ac => new ActivityViewDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = new EmployeeSelectDto {
                            Id = ea.EmployeeId,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            FullName = ea.Employee.FullName,
                            Role = ea.Employee.Role,
                            CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                            EmployeeSalaries = ea.Employee.Salaries
                                .Select(sal => new EmployeeSalaryDto {
                                    Id = sal.Id,
                                    EmployeeId = ea.EmployeeId,
                                    StartDate = sal.StartDate,
                                    FinishDate = sal.FinishDate,
                                    Salary = sal.Salary,
                                }).ToList()
                        }, 
                        ActivityId = ea.ActivityId,
                        Activity =  new ActivitySelectDto {
                            Id = ea.ActivityId,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        }
                    }).ToHashSet()   
            })
            .ToListAsync();
    }
}