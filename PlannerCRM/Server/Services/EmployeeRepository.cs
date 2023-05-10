using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;

namespace PlannerCRM.Server.Services;

public class EmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(EmployeeAddFormDto entity) {
        _db.Employees.Add(new Employee {
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            BirthDay = entity.BirthDay ?? throw new NullReferenceException(),
            StartDate = entity.StartDate ?? throw new NullReferenceException(),
            Password = entity.Password,
            NumericCode = entity.NumericCode,
            Role = entity.Role ?? throw new NullReferenceException(),
            Salaries = entity.EmployeeSalaries
                .Select(ems =>
                    new EmployeeSalary {
                        EmployeeId = ems.EmployeeId,
                        StartDate = ems.StartDate,
                        FinishDate = ems.FinishDate,
                        Salary = decimal.Parse(ems.Salary.ToString())
                    })
                .ToList()
        });

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var entity = await _db.Employees.SingleOrDefaultAsync(e => e.Id == id);
        _db.Employees.Remove(entity);

        await _db.SaveChangesAsync();
    }

    public async Task<bool> EditAsync(EmployeeEditFormDto entity) {
        var model = await _db.Employees.SingleOrDefaultAsync(e => e.Id == entity.Id);
        
        if (model == null) {
            return false;
        } else {
            model.Id = entity.Id;
            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.BirthDay = entity.BirthDay;
            model.StartDate = entity.StartDate;
            model.Email = entity.Email;
            model.Role = entity.Role;
            model.NumericCode = entity.NumericCode;
            model.Salaries = entity.EmployeeSalaries
                .Select(ems => 
                    new EmployeeSalary {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.FinishDate,
                        Salary = decimal.Parse(ems.Salary.ToString())
                    }
                ).ToList();
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<EmployeeViewDto> GetForViewAsync(int id) {
        return await _db.Employees
            .Select(e => new EmployeeViewDto {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = $"{e.FirstName} {e.LastName}",
                BirthDay = e.BirthDay,
                StartDate = e.StartDate,
                Email = e.Email,
                Role = e.Role.ToString().Replace('_', ' '), 
                HourlyRate = e.Salaries.Count() != 0 
                    ? float.Parse(e.Salaries.SingleOrDefault().Salary.ToString())
                    : 0.0F,
                EmployeeSalaries = e.Salaries
                    .Select( ems => new EmployeeSalaryDto {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems.Salary.ToString())})
                    .ToList(),
                EmployeeActivities = e.EmployeeActivity
                    .Select( ems =>
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
                        }
                    ).ToList()
                })
                
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<EmployeeEditFormDto> GetForEditAsync(int id) {
        return await _db.Employees
            .Select(e => new EmployeeEditFormDto {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                BirthDay = e.BirthDay,
                StartDate = e.StartDate,
                Role = e.Role,
                NumericCode = e.NumericCode,
                Password = e.Password,
                HourlyRate = e.Salaries.Count() != 0 
                    ? float.Parse(e.Salaries.SingleOrDefault().Salary.ToString())
                    : 0.0F,
                StartDateHourlyRate = e.Salaries.SingleOrDefault().StartDate,
                FinishDateHourlyRate = e.Salaries.SingleOrDefault().FinishDate,
                EmployeeSalaries = e.Salaries
                    .Select( ems => new EmployeeSalaryDto {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems.Salary.ToString())})
                    .ToList(),
                EmployeeActivities = e.EmployeeActivity
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
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<EmployeeDeleteDto> GetForDeleteAsync(int id) {
        return await _db.Employees
            .Select(e => new EmployeeDeleteDto {
                Id = e.Id,
                FullName = $"{e.FirstName} {e.LastName}",
                Email = e.Email,
                Role = e.Role
                    .ToString()
                    .Replace('_', ' ')
            })
            .SingleOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email) {
        var employees = await GetAllAsync();

        if (!string.IsNullOrEmpty(email)) {
            var foundByUsername = employees.Where(e => e.FullName.Contains(email, StringComparison.InvariantCultureIgnoreCase));
            var foundByEmail = employees.Where(e => e.Email.Contains(email, StringComparison.InvariantCultureIgnoreCase));
            
            if (foundByUsername.Count() != 0) {
                return foundByUsername
                    .Select(e => new EmployeeSelectDto {
                        Id = e.Id,
                        Email = e.Email,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        EmployeeSalaries = e.EmployeeSalaries
                            .Select( ems => new EmployeeSalaryDto {
                                EmployeeId = ems.Id,
                                StartDate = ems.StartDate,
                                FinishDate = ems.StartDate,
                                Salary = float.Parse(ems.Salary.ToString())})
                            .ToList(),
                        EmployeeActivities = e.EmployeeActivities
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
                    .Select(e => new EmployeeSelectDto {
                        Id = e.Id,
                        Email = e.Email,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        EmployeeSalaries = e.EmployeeSalaries
                            .Select( ems => new EmployeeSalaryDto {
                                EmployeeId = ems.Id,
                                StartDate = ems.StartDate,
                                FinishDate = ems.StartDate,
                                Salary = float.Parse(ems.Salary.ToString())})
                            .ToList(),
                        EmployeeActivities = e.EmployeeActivities
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
            .Select(e => new EmployeeEditFormDto {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                BirthDay = e.BirthDay,
                StartDate = e.StartDate,
                Role = e.Role,
                NumericCode = e.NumericCode,
                Password = e.Password,
                EmployeeActivities = e.EmployeeActivity
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
                            Id = e.Id,
                            Email = e.Email
                        },
                        EmployeeId = e.Id
                    }).ToList(),
                EmployeeSalaries = e.Salaries
                    .Select( ems => new EmployeeSalaryDto {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems.Salary.ToString())})
                    .ToList(),
            })
            .Where(e => EF.Functions.Like(email, $"%{email}%"))
            .FirstAsync();
        return employee;
    }

    public async Task<List<EmployeeViewDto>> GetAllAsync() {
        return await _db.Employees
            .Select(e => new EmployeeViewDto {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = $"{e.FirstName} {e.LastName}",
                BirthDay = e.BirthDay,
                StartDate = e.StartDate,
                Email = e.Email,
                Role = e.Role.ToString().ToUpper().Replace('_', ' '),
                HourlyRate = e.Salaries.Count() != 0 
                    ? float.Parse(e.Salaries.SingleOrDefault().Salary.ToString())
                    : 0.0F,
                EmployeeActivities = e.EmployeeActivity
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
                            Id = e.Id,
                            Email = e.Email
                        },
                        EmployeeId = e.Id
                    }).ToList(),
                EmployeeSalaries = e.Salaries
                    .Select( ems => new EmployeeSalaryDto {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems.Salary.ToString())})
                    .ToList(),
            })
            .ToListAsync();
    }

    public async Task<CurrentEmployeeDto> GetUserIdAsync(string email) {
        return await _db.Employees
            .Select(e => new CurrentEmployeeDto {
                Id = e.Id,
                Email = e.Email})
            .Where(e => e.Email == email)
            .FirstOrDefaultAsync();
    }
}