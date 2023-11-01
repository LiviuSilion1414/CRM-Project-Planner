namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository
{
    private readonly AppDbContext _dbContext;
    private readonly DtoValidatorUtillity _validator;
    private readonly ILogger<DtoValidatorUtillity> _logger;

    public EmployeeRepository(
        AppDbContext dbContext, 
        DtoValidatorUtillity validator, 
        Logger<DtoValidatorUtillity> logger) 
    {
        _dbContext = dbContext;
        _validator = validator;
        _logger = logger;
    }

    public async Task AddAsync(EmployeeFormDto dto) {
        try {
            var isValid = await _validator.ValidateEmployeeAsync(dto, OperationType.ADD);
            
            if (isValid) {
                await _dbContext.Employees.AddAsync(
                    new Employee {
                        Id = dto.Id,
                        Email = dto.Email,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        FullName = $"{dto.FirstName} {dto.LastName}",
                        BirthDay = dto.BirthDay 
                            ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                        StartDate = dto.StartDate  
                            ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                        Password = dto.Password,
                        NumericCode = dto.NumericCode,
                        Role = dto.Role 
                            ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                        CurrentHourlyRate = dto.CurrentHourlyRate,
                        Salaries = dto.EmployeeSalaries
                            .Select(ems =>
                                new EmployeeSalary {
                                    EmployeeId = ems.EmployeeId,
                                    StartDate = ems.StartDate,
                                    FinishDate = ems.FinishDate,
                                    Salary = ems.Salary
                                }
                            )
                            .ToList()
                    }
                );
                
                if (await _dbContext.SaveChangesAsync() == 0) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_DELETE);
            }
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task ArchiveAsync(string employeeId) {
        try {
            var employee = await _validator.ValidateDeleteEmployeeAsync(employeeId);

            if (employee is not null) {
                employee.IsArchived = true;
                _dbContext.Update(employee);

                if (await _dbContext.SaveChangesAsync() == 0) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
            }
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }
    
    public async Task RestoreAsync(string employeeId) {
        try {
            var employee = await _validator.ValidateDeleteEmployeeAsync(employeeId);

            if (employee is not null) {
                employee.IsArchived = false;

                _dbContext.Update(employee);

                if (await _dbContext.SaveChangesAsync() == 0) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
            }
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task DeleteAsync(string employeeId) {
        try {
            var employeeDelete = await _validator.ValidateDeleteEmployeeAsync(employeeId);

            if (employeeDelete is not null) {
                _dbContext.Remove(employeeDelete);
            
                if (await _dbContext.SaveChangesAsync() == 0) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_DELETE);
            }
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task EditAsync(EmployeeFormDto dto) {
        try {
            var isValid = await _validator.ValidateEmployeeAsync(dto, OperationType.EDIT);
            
            if (isValid) {
                var model = await _dbContext.Employees
                    .SingleAsync(em => !em.IsDeleted && !em.IsArchived && em.Id == dto.Id);
                
                model.Id = dto.Id;
                model.FirstName = dto.FirstName;
                model.LastName = dto.LastName;
                model.FullName = $"{dto.FirstName + dto.LastName}";
                model.BirthDay = dto.BirthDay 
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.StartDate = dto.StartDate 
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.Email = dto.Email;
                model.Role = dto.Role 
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.NumericCode = dto.NumericCode;
                model.CurrentHourlyRate = dto.CurrentHourlyRate;
                
                var isContainedModifiedHourlyRate = await _dbContext.Employees
                    .AnyAsync(em => em.Id != dto.Id && 
                        em.Salaries
                            .Any(s => s.Salary != dto.CurrentHourlyRate));
                
                if (!isContainedModifiedHourlyRate) {
                    model.Salaries = dto.EmployeeSalaries
                        .Where(ems => _dbContext.Employees
                            .Any(em => em.Id == ems.EmployeeId))
                        .Select(ems => 
                            new EmployeeSalary {
                                EmployeeId = dto.Id,
                                StartDate = ems.StartDate,
                                FinishDate = ems.FinishDate,
                                Salary = ems.Salary
                            }
                        ).ToList();
                }    
                
                _dbContext.Employees.Update(model);
                
                if (await _dbContext.SaveChangesAsync() == 0) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_EDIT);
            }
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);
            
            throw;
        }
    }

    public async Task<EmployeeViewDto> GetForViewByIdAsync(string employeeId) {
        return await _dbContext.Employees
            .Select(em => new EmployeeViewDto {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = $"{em.FirstName} {em.LastName}",
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                NumericCode = em.NumericCode,
                Password = em.Password,
                StartDateHourlyRate = em.Salaries.Single().StartDate,
                FinishDateHourlyRate = em.Salaries.Single().FinishDate,
                Email = em.Email,
                IsDeleted = em.IsDeleted,
                IsArchived = em.IsArchived,
                Role = em.Role, 
                CurrentHourlyRate = em.CurrentHourlyRate,
                EmployeeSalaries = em.Salaries
                    .Select(ems => new EmployeeSalaryDto {
                        Id = ems.Id,
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary})
                    .ToList(),
                EmployeeActivities = em.EmployeeActivity
                    .Select(ea =>
                        new EmployeeActivityDto {
                            EmployeeId = ea.EmployeeId,
                            Employee = new EmployeeSelectDto {
                                Id = ea.Employee.Id,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
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
            .SingleAsync(em => em.Id == employeeId);
    }

    public async Task<EmployeeSelectDto> GetForRestoreAsync(string employeeId) {
        return await _dbContext.Employees
            .Select(em => new EmployeeSelectDto {
                Id = em.Id,
                Email = em.Email,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = em.FullName,
                Role = em.Role
            })
            .SingleOrDefaultAsync(em => em.Id == employeeId);
    }

    public async Task<EmployeeFormDto> GetForEditByIdAsync(string employeeId) { 
        return await _dbContext.Employees
            .Where(em => !em.IsDeleted && !em.IsArchived)
            .Select(em => new EmployeeFormDto {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                Email = em.Email,
                OldEmail = em.Email,
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                Role = em.Role,
                NumericCode = em.NumericCode,
                Password = em.Password,
                CurrentHourlyRate = em.CurrentHourlyRate,
                IsDeleted = em.IsDeleted,
                StartDateHourlyRate = em.Salaries.Single().StartDate,
                FinishDateHourlyRate = em.Salaries.Single().FinishDate,
                EmployeeSalaries = em.Salaries
                    .Where(ems => _dbContext.Employees
                        .Any(em => em.Id == ems.EmployeeId))
                    .Select(ems => new EmployeeSalaryDto {
                        Id = ems.Id,
                        EmployeeId = em.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary})
                    .ToList()
                })
            .SingleOrDefaultAsync(em => em.Id == employeeId);
    }

    public async Task<EmployeeDeleteDto> GetForDeleteByIdAsync(string employeeId) {
        return await _dbContext.Employees
            .Where(em => (!em.IsDeleted || !em.IsArchived) && em.Id == employeeId)
            .Select(em => new EmployeeDeleteDto {
                Id = em.Id,
                FullName = $"{em.FirstName} {em.LastName}",
                Email = em.Email,
                Role = em.Role
                    .ToString()
                    .Replace('_', ' '),
                EmployeeActivities = _dbContext.EmployeeActivity
                    .Where(ea => ea.EmployeeId == employeeId)
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _dbContext.Employees
                            .Where(e => e.Id == ea.EmployeeId)
                            .Select(_ => new EmployeeSelectDto {
                                Id = ea.Employee.Id,
                                Email = ea.Employee.Email,
                                FullName = ea.Employee.FullName,
                            })
                            .Single(),
                        ActivityId = ea.ActivityId,
                        Activity = _dbContext.Activities
                            .Where(ac => ac.Id == ea.ActivityId)
                            .Select(_ => new ActivitySelectDto {
                                Id = ea.Activity.Id,
                                Name = ea.Activity.Name,
                                WorkOrderId = ea.Activity.WorkOrderId,
                            })
                            .Single()
                    })
                    .ToList()
                
            })
            .SingleOrDefaultAsync(em => em.Id == employeeId)
                ?? new();
    }
    
    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email) {
        return await _dbContext.Employees
            .Where(em => !em.IsDeleted && !em.IsArchived)
            .Select(em => new EmployeeSelectDto {
                Id = em.Id,
                Email = em.Email,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = em.FullName,
                Role = em.Role
            })
            .Where(em => EF.Functions.ILike(em.FullName, $"%{email}%") || 
                EF.Functions.ILike(em.Email, $"%{email}%"))
            .ToListAsync();
    }

    public async Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int limit, int offset) {
        return await _dbContext.Employees
            .OrderBy(em => em.Id)
            .Skip(limit)
            .Take(offset)
            .Select(em => new EmployeeViewDto {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = $"{em.FirstName} {em.LastName}",
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                Email = em.Email,
                Role = em.Role,
                CurrentHourlyRate = em.CurrentHourlyRate,
                IsDeleted = em.IsDeleted,
                IsArchived = em.IsArchived,
                EmployeeActivities = em.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.ActivityId,
                        EmployeeId = em.Id,
                        Employee = _dbContext.Employees
                            .Select(em => new EmployeeSelectDto {
                                Id = em.Id,
                                Email = em.Email,
                                FirstName = em.FirstName,
                                LastName = em.LastName,
                                Role = em.Role
                            })
                            .Single(em => em.Id == ea.EmployeeId),
                        ActivityId = ea.ActivityId,
                        Activity = _dbContext.Activities
                            .Select(ac => new ActivitySelectDto {
                            Id = ac.Id,
                            Name = ac.Name,
                            StartDate = ac.StartDate,
                            FinishDate = ac.FinishDate,
                            WorkOrderId = ac.WorkOrderId
                        })
                        .Single(ac => ac.Id == ea.ActivityId),
                    }).ToList(),
                EmployeeSalaries = em.Salaries
                    .Select(ems => new EmployeeSalaryDto {
                        Id = ems.Id,
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary})
                    .ToList(),
            })
            .ToListAsync();
    }

    public async Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email) {
        return await _dbContext.Employees
            .Where(em => !em.IsDeleted && !em.IsArchived)
            .Select(em => new CurrentEmployeeDto {
                Id = em.Id,
                Email = em.Email})
            .FirstAsync(em => em.Email == email);
    }

    public async Task<int> GetEmployeesSizeAsync() => 
        await _dbContext.Employees
            .CountAsync();
}