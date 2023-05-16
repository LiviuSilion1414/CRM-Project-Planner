using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Server.CustomExceptions;

namespace PlannerCRM.Server.Services;

public class EmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(EmployeeAddFormDto employeeAddFormDto) {
        if (employeeAddFormDto == null) {
            throw new NullReferenceException("Oggetto null");
        }

        var isNull = employeeAddFormDto.GetType().GetProperties()
            .All(prop => prop.GetValue(employeeAddFormDto) != null);
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }
        
        var isAlreadyPresent = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == employeeAddFormDto.Id);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException("Oggetto già presente");
        }

        await _db.Employees.AddAsync(new Employee {
            Email = employeeAddFormDto.Email,
            FirstName = employeeAddFormDto.FirstName,
            LastName = employeeAddFormDto.LastName,
            BirthDay = employeeAddFormDto.BirthDay ?? throw new NullReferenceException(),
            StartDate = employeeAddFormDto.StartDate ?? throw new NullReferenceException(),
            Password = employeeAddFormDto.Password,
            NumericCode = employeeAddFormDto.NumericCode,
            Role = employeeAddFormDto.Role ?? throw new NullReferenceException(),
            Salaries = employeeAddFormDto.EmployeeSalaries
                .Select(ems =>
                    new EmployeeSalary {
                        EmployeeId = ems.EmployeeId,
                        StartDate = ems.StartDate,
                        FinishDate = ems.FinishDate,
                        Salary = decimal.Parse(ems.Salary.ToString())
                    })
                .ToList()
        });

        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile proseguire.");
        }
    }

    public async Task DeleteAsync(int id) {
        var employeeDelete = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == id);

        if (employeeDelete == null) {
            throw new KeyNotFoundException("Impossibile eliminare l'elemento");
        }

        _db.Employees.Remove(employeeDelete);
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile proseguire.");
        }
    }

    public async Task EditAsync(EmployeeEditFormDto employeeEditFormDto) {
        if (employeeEditFormDto == null) {
            throw new NullReferenceException("Oggetto null.");
        }

        var isNull = employeeEditFormDto.GetType().GetProperties()
            .All(prop => prop.GetValue(employeeEditFormDto) != null);
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }

        var model = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == employeeEditFormDto.Id);
        
        if (model == null) {
            throw new KeyNotFoundException("Oggetto non trovato");
        }

        model.Id = employeeEditFormDto.Id;
        model.FirstName = employeeEditFormDto.FirstName;
        model.LastName = employeeEditFormDto.LastName;
        model.BirthDay = employeeEditFormDto.BirthDay;
        model.StartDate = employeeEditFormDto.StartDate;
        model.Email = employeeEditFormDto.Email;
        model.Role = employeeEditFormDto.Role;
        model.NumericCode = employeeEditFormDto.NumericCode;
        model.Salaries = employeeEditFormDto.EmployeeSalaries
            .Select(ems => 
                new EmployeeSalary {
                    EmployeeId = ems.Id,
                    StartDate = ems.StartDate,
                    FinishDate = ems.FinishDate,
                    Salary = decimal.Parse(ems.Salary.ToString())
                }
            ).ToList();
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile proseguire.");
        }
    }

    public async Task<EmployeeViewDto> GetForViewAsync(int id) {
        return await _db.Employees
            .Select(em => new EmployeeViewDto {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = $"{em.FirstName} {em.LastName}",
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                Email = em.Email,
                Role = em.Role
                    .ToString()
                    .Replace('_', ' '), 
                HourlyRate = em.Salaries.Count() != 0 
                    ? float.Parse(em.Salaries
                        .SingleOrDefault()
                        .Salary
                        .ToString())
                    : 0.0F,
                EmployeeSalaries = em.Salaries
                    .Select( ems => new EmployeeSalaryDto {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems
                            .Salary
                            .ToString())})
                    .ToList(),
                EmployeeActivities = em.EmployeeActivity
                    .Select( ea =>
                        new EmployeeActivityDto {
                            EmployeeId = ea.Id,
                            Employee = new EmployeeSelectDto {
                                Id = ea.EmployeeId,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                Email = ea.Employee.Email,
                                Role = ea.Employee.Role, 
                            },
                            ActivityId = ea.ActivityId,
                            Activity = new ActivitySelectDto {
                                Id = ea.ActivityId,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            }
                        })
                    .ToList()
                })   
            .SingleOrDefaultAsync(em => em.Id == id);
    }

    public async Task<EmployeeEditFormDto> GetForEditAsync(int id) {
        return await _db.Employees
            .Select(em => new EmployeeEditFormDto {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                Email = em.Email,
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                Role = em.Role,
                NumericCode = em.NumericCode,
                Password = em.Password,
                HourlyRate = em.Salaries.Count() != 0 
                    ? float.Parse(em.Salaries
                        .SingleOrDefault()
                        .Salary
                        .ToString())
                    : 0.0F,
                StartDateHourlyRate = em.Salaries.SingleOrDefault().StartDate,
                FinishDateHourlyRate = em.Salaries.SingleOrDefault().FinishDate,
                EmployeeSalaries = em.Salaries
                    .Select( ems => new EmployeeSalaryDto {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems.Salary.ToString())})
                    .ToList(),
                EmployeeActivities = em.EmployeeActivity
                    .Select(ems =>
                        new EmployeeActivityDto {
                            EmployeeId = ems.Id,
                            Employee = new EmployeeSelectDto {
                                Id = ems.EmployeeId,
                                FirstName = ems.Employee.FirstName,
                                LastName = ems.Employee.LastName,
                                Email = ems.Employee.Email,
                                Role = ems.Employee.Role, 
                            },
                            ActivityId = ems.ActivityId,
                            Activity = new ActivitySelectDto {
                                Id = ems.ActivityId,
                                Name = ems.Activity.Name,
                                StartDate = ems.Activity.StartDate,
                                FinishDate = ems.Activity.FinishDate,
                                WorkOrderId = ems.Activity.WorkOrderId
                            }
                        })
                    .ToList()
                })
            .SingleOrDefaultAsync(em => em.Id == id);
    }

    public async Task<EmployeeDeleteDto> GetForDeleteAsync(int id) {
        return await _db.Employees
            .Select(em => new EmployeeDeleteDto {
                Id = em.Id,
                FullName = $"{em.FirstName} {em.LastName}",
                Email = em.Email,
                Role = em.Role
                    .ToString()
                    .Replace('_', ' ')
            })
            .SingleOrDefaultAsync(em => em.Id == id);
    }
    
    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email) {
        var employees = await GetAllAsync();

        if (!string.IsNullOrEmpty(email)) {
            var foundByUsername = employees
                .Where(em => em.FullName
                    .Contains(email, StringComparison.InvariantCultureIgnoreCase));
            
            var foundByEmail = employees
                .Where(em => em.Email
                    .Contains(email, StringComparison.InvariantCultureIgnoreCase));
            
            if (foundByUsername.Count() != 0) {
                return foundByUsername
                    .Select(em => new EmployeeSelectDto {
                        Id = em.Id,
                        Email = em.Email,
                        FirstName = em.FirstName,
                        LastName = em.LastName,
                        EmployeeSalaries = em.EmployeeSalaries
                            .Select( ems => new EmployeeSalaryDto {
                                EmployeeId = ems.Id,
                                StartDate = ems.StartDate,
                                FinishDate = ems.StartDate,
                                Salary = float.Parse(ems
                                    .Salary
                                    .ToString())})
                            .ToList(),
                        EmployeeActivities = em.EmployeeActivities
                            .Select(ems =>
                                new EmployeeActivityDto {
                                    EmployeeId = ems.Id,
                                    Employee = new EmployeeSelectDto {
                                        Id = ems.EmployeeId,
                                        FirstName = ems.Employee.FirstName,
                                        LastName = ems.Employee.LastName,
                                        Email = ems.Employee.Email,
                                        Role = ems.Employee.Role, 
                                    },
                                    ActivityId = ems.ActivityId,
                                    Activity = new ActivitySelectDto {
                                        Id = ems.ActivityId,
                                        Name = ems.Activity.Name,
                                        StartDate = ems.Activity.StartDate,
                                        FinishDate = ems.Activity.FinishDate,
                                        WorkOrderId = ems.Activity.WorkOrderId
                                    }
                                }).ToList()
                            }).ToList();
            } else if (foundByEmail.Count() != 0) {
                return foundByEmail
                    .Select(em => new EmployeeSelectDto {
                        Id = em.Id,
                        Email = em.Email,
                        FirstName = em.FirstName,
                        LastName = em.LastName,
                        EmployeeSalaries = em.EmployeeSalaries
                            .Select( ems => new EmployeeSalaryDto {
                                EmployeeId = ems.Id,
                                StartDate = ems.StartDate,
                                FinishDate = ems.StartDate,
                                Salary = float.Parse(ems
                                    .Salary
                                    .ToString())})
                            .ToList(),
                        EmployeeActivities = em.EmployeeActivities
                            .Select(ems =>
                                new EmployeeActivityDto {
                                    EmployeeId = ems.Id,
                                    Employee = new EmployeeSelectDto {
                                        Id = ems.EmployeeId,
                                        FirstName = ems.Employee.FirstName,
                                        LastName = ems.Employee.LastName,
                                        Email = ems.Employee.Email,
                                        Role = ems.Employee.Role, 
                                    },
                                    ActivityId = ems.ActivityId,
                                    Activity = new ActivitySelectDto {
                                        Id = ems.ActivityId,
                                        Name = ems.Activity.Name,
                                        StartDate = ems.Activity.StartDate,
                                        FinishDate = ems.Activity.FinishDate,
                                        WorkOrderId = ems.Activity.WorkOrderId
                                    }
                                }).ToList()
                    }).ToList();                   
            } else {
                return new List<EmployeeSelectDto>();
            }
        } else {
            return new List<EmployeeSelectDto>();
        }
    }

    public async Task<EmployeeEditFormDto> SearchEmployeeCompleteAsync(string email) {
        var employee = await _db.Employees
            .Select(em => new EmployeeEditFormDto {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                Email = em.Email,
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                Role = em.Role,
                NumericCode = em.NumericCode,
                Password = em.Password,
                EmployeeActivities = em.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Activity.Id,
                        Activity = new ActivitySelectDto {
                            Id = ea.Activity.Id,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        },
                        ActivityId = ea.Activity.Id,
                        Employee = new EmployeeSelectDto {
                            Id = em.Id,
                            Email = em.Email
                        },
                        EmployeeId = em.Id
                    }).ToList(),
                EmployeeSalaries = em.Salaries
                    .Select( ems => new EmployeeSalaryDto {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems
                            .Salary
                            .ToString())})
                    .ToList(),
            })
            .Where(em => EF.Functions.Like(email, $"%{email}%"))
            .FirstAsync();
        return employee;
    }

    public async Task<List<EmployeeViewDto>> GetAllAsync() {
        return await _db.Employees
            .Select(em => new EmployeeViewDto {
                Id = em.Id,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = $"{em.FirstName} {em.LastName}",
                BirthDay = em.BirthDay,
                StartDate = em.StartDate,
                Email = em.Email,
                Role = em.Role
                    .ToString()
                    .ToUpper()
                    .Replace('_', ' '),
                HourlyRate = em.Salaries.Count() != 0 
                    ? float.Parse(em.Salaries
                        .SingleOrDefault()
                        .Salary
                        .ToString())
                    : 0.0F,
                EmployeeActivities = em.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto {
                        Id = ea.Activity.Id,
                        Activity = new ActivitySelectDto {
                            Id = ea.Activity.Id,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        },
                        ActivityId = ea.Activity.Id,
                        Employee = new EmployeeSelectDto {
                            Id = em.Id,
                            Email = em.Email
                        },
                        EmployeeId = em.Id
                    }).ToList(),
                EmployeeSalaries = em.Salaries
                    .Select( ems => new EmployeeSalaryDto {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems
                            .Salary
                            .ToString())})
                    .ToList(),
            })
            .ToListAsync();
    }

    public async Task<CurrentEmployeeDto> GetUserIdAsync(string email) {
        return await _db.Employees
            .Select(em => new CurrentEmployeeDto {
                Id = em.Id,
                Email = em.Email})
            .FirstOrDefaultAsync(em => em.Email == email);
    }
}