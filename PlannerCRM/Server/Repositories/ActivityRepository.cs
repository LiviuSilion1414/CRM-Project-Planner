namespace PlannerCRM.Server.Repositories;

public class ActivityRepository
{
    private readonly AppDbContext _dbContext;
    private readonly DtoValidatorUtillity _validator;
    private readonly ILogger<DtoValidatorUtillity> _logger;

    public ActivityRepository(AppDbContext dbContext, DtoValidatorUtillity validator, Logger<DtoValidatorUtillity> logger) {
		_dbContext = dbContext;
		_validator = validator;
		_logger = logger;
	}

    public async Task AddAsync(ActivityFormDto dto) {
        try {
            var isValid = await _validator.ValidateActivityAsync(dto, OperationType.ADD);

            if (isValid) {
                var entity = new Activity {
                    Id = dto.Id,
                    Name = dto.Name,
                    StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                    FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                    WorkOrderId = dto.WorkOrderId ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                    EmployeeActivity = dto.EmployeeActivity
                        .Select(ea => new EmployeeActivity {
                            Id = ea.Id,
                            EmployeeId = ea.EmployeeId,
                            ActivityId = dto.Id,
                        }).ToHashSet(),
                };
        
                await _dbContext.Activities.AddAsync(entity);
        
                var workOrder = await _dbContext.WorkOrders
                    .SingleAsync(wo => wo.Id == dto.WorkOrderId);
                workOrder.Activities.Add(entity);    
        
                _dbContext.Update(workOrder);
                
                var rowsAffected = await _dbContext.SaveChangesAsync();
                if (rowsAffected == 0) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task DeleteAsync(int id) {
        try {
            var activityDelete = await _validator.ValidateDeleteActivityAsync(id);

            await _dbContext.EmployeeActivity
                .Where(ea => ea.ActivityId == activityDelete.Id)
                .ForEachAsync(ea => 
                    _dbContext.EmployeeActivity
                        .Remove(ea)
                );
    
            _dbContext.Activities.Remove(activityDelete);
    
            if (await _dbContext.SaveChangesAsync() == 0) {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task<bool> EditAsync(ActivityFormDto dto) {
        try {
            var isValid = await _validator.ValidateActivityAsync(dto, OperationType.EDIT);

            if (isValid) {
                var model = await _dbContext.Activities
                    .SingleAsync(ac => ac.Id == dto.Id);

                model.Id = dto.Id;
                model.Name = dto.Name;
                model.StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.WorkOrderId = dto.WorkOrderId  ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.EmployeeActivity = dto.EmployeeActivity
                    .Select(ea => new EmployeeActivity {
                        EmployeeId = ea.EmployeeId,     
                        ActivityId = ea.ActivityId,
                    })
                    .ToHashSet();
                
                var employeesToRemove = dto.DeleteEmployeeActivity
                    .Where(eaDto => _dbContext.EmployeeActivity
                        .Any(ea => eaDto.EmployeeId == ea.EmployeeId))
                    .Select(e => 
                        new EmployeeActivity() {
                            Id = e.Id,
                            EmployeeId = e.EmployeeId,
                            ActivityId = dto.Id
                        }
                    )
                    .ToList();
                
                foreach (var item in employeesToRemove) {
                    _dbContext.EmployeeActivity.Remove(item);
                }

                var workOrder = await _dbContext.WorkOrders
                    .SingleAsync(wo => wo.Id == dto.WorkOrderId);
                
                var activity = workOrder.Activities.Find(ac => ac.Id == dto.Id);
                activity.Id = dto.Id;
                activity.Name = dto.Name;
                activity.StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                activity.FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                activity.WorkOrderId = dto.WorkOrderId  ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                activity.EmployeeActivity = dto.EmployeeActivity
                    .Select(ea => new EmployeeActivity {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        ActivityId = ea.ActivityId,
                    }).ToHashSet();

                _dbContext.Update(model);
                _dbContext.Update(workOrder);

                if (await _dbContext.SaveChangesAsync() == 0) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }

                return true;
            }

            return false;
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task<ActivityViewDto> GetForViewAsync(int id) {
        return await _dbContext.Activities
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
                        Employee = _dbContext.Employees
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
                        Activity = _dbContext.Activities
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
        return await _dbContext.Activities
            .Where(ac => ac.Id == activityId)
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
                        Employee = _dbContext.Employees
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
                        Activity = _dbContext.Activities
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
            .FirstAsync(ac => ac.Id == activityId);
    }

    public async Task<ActivityDeleteDto> GetForDeleteAsync(int id) {
        return await _dbContext.Activities
            .Select(ac => new ActivityDeleteDto {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                Employees = _dbContext.EmployeeActivity
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
                                Employee = _dbContext.Employees
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
                                Activity = _dbContext.Activities
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
         return await _dbContext.Activities
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
                        Employee = _dbContext.Employees
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
                        Activity = _dbContext.Activities
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
            .Where(ac => _dbContext.EmployeeActivity
                .Any(ea => ea.EmployeeId == employeeId && ac.Id == ea.ActivityId))
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(int workOrderId) {
        return await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
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
                        Employee = _dbContext.Employees
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
                        Activity = _dbContext.Activities
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
        return await _dbContext.Activities
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