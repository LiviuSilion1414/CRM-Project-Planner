namespace PlannerCRM.Server.Services;

public class ActivityRepository
{
    private readonly AppDbContext _db;
    private readonly DtoValidatorService _validator;
    private readonly Logger<DtoValidatorService> _logger;

    public ActivityRepository(AppDbContext db, DtoValidatorService validator, Logger<DtoValidatorService> logger) {
		_db = db;
		_validator = validator;
		_logger = logger;
	}

    public async Task AddAsync(ActivityFormDto dto) {
        try {
            _validator.ValidateActivity(dto, OperationType.ADD, out var isValid);

            if (isValid) {
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
                if (rowsAffected == 0) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            throw;
        }
    }

    public async Task DeleteAsync(int id) {
        try {
            var activityDelete = await _validator.ValidateDeleteActivityAsync(id);

            await _db.EmployeeActivity
                .Where(ea => ea.ActivityId == activityDelete.Id)
                .ForEachAsync(ea => 
                    _db.EmployeeActivity
                        .Remove(ea)
                );
    
            _db.Activities.Remove(activityDelete);
    
            if (await _db.SaveChangesAsync() == 0) {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            throw;
        }
    }

    public async Task EditAsync(ActivityFormDto dto) {
        try {
            _validator.ValidateActivity(dto, OperationType.EDIT, out var isValid);

            if (isValid) {
                var model = await _db.Activities
                    .SingleAsync(ac => ac.Id == dto.Id);

                model.Id = dto.Id;
                model.Name = dto.Name;
                model.StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
                model.FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
                model.WorkOrderId = dto.WorkOrderId ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
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
                activity.StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
                activity.FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
                activity.WorkOrderId = dto.WorkOrderId ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
                activity.EmployeeActivity = dto.EmployeeActivity
                    .Select(ea => new EmployeeActivity {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        ActivityId = ea.ActivityId,
                    }).ToHashSet();

                _db.Update(model);
                _db.Update(workOrder);

                if (await _db.SaveChangesAsync() == 0) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            throw;
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

    public async Task<ActivityFormDto> GetForEditAsync(int activityId) {
        return await _db.Activities
            .Select(ac => new ActivityFormDto {
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
            .SingleAsync(ac => ac.Id == id);
    }

    public async Task<List<ActivityFormDto>> GetActivityByEmployeeId(int employeeId) {
         return await _db.Activities
            .Select(ac => new ActivityFormDto {
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

    public async Task<List<ActivityFormDto>> GetActivitiesPerWorkOrderAsync(int workOrderId) {
        return await _db.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .Select(ac => new ActivityFormDto {
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