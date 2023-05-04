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

    public async Task AddAsync(EmployeeAddForm entity) {
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

    public async Task<bool> EditAsync(EmployeeEditForm entity) {
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
            model.Salaries = new List<EmployeeSalary> {
                new EmployeeSalary {
                    EmployeeId = entity.Id,
                    StartDate = entity.StartDate,
                    FinishDate = entity.StartDate,
                    Salary = decimal.Parse(entity.EmployeeSalaries.SingleOrDefault().Salary.ToString())
                }
            };
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<EmployeeViewDTO> GetForViewAsync(int id) {
        return await _db.Employees
            .Select(e => new EmployeeViewDTO {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = $"{e.FirstName} {e.LastName}",
                BirthDay = e.BirthDay,
                StartDate = e.StartDate,
                Email = e.Email,
                Role = e.Role.ToString().Replace('_', ' '), 
                EmployeeSalaries = e.Salaries
                    .Select( ems => new EmployeeSalaryDTO {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems.Salary.ToString())})
                    .ToList(),
                EmployeeActivities = e.EmployeeActivity
                    .Select( ems =>
                        new EmployeeActivityDTO {
                            EmployeeId = ems.Id,
                            Employee = new EmployeeSelectDTO {
                                Id = ems.EmployeeId,
                                FirstName = ems.Employee.FirstName,
                                LastName = ems.Employee.LastName,
                                Email = ems.Employee.Email,
                                Role = ems.Employee.Role, 
                            },
                            ActivityId = ems.ActivityId,
                            Activity = new ActivitySelectDTO {
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

    public async Task<EmployeeEditForm> GetForEditAsync(int id) {
        return await _db.Employees
            .Select(e => new EmployeeEditForm {
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
                    .Select( ems => new EmployeeSalaryDTO {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems.Salary.ToString())})
                    .ToList(),
                EmployeeActivities = e.EmployeeActivity
                    .Select(ems =>
                        new EmployeeActivityDTO {
                            EmployeeId = ems.Id,
                            Employee = new EmployeeSelectDTO {
                                Id = ems.EmployeeId,
                                FirstName = ems.Employee.FirstName,
                                LastName = ems.Employee.LastName,
                                Email = ems.Employee.Email,
                                Role = ems.Employee.Role, 
                            },
                            ActivityId = ems.ActivityId,
                            Activity = new ActivitySelectDTO {
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

    public async Task<EmployeeDeleteDTO> GetForDeleteAsync(int id) {
        return await _db.Employees
            .Select(e => new EmployeeDeleteDTO {
                Id = e.Id,
                FullName = $"{e.FirstName} {e.LastName}",
                Email = e.Email,
                Role = e.Role
                    .ToString()
                    .Replace('_', ' ')
            })
            .SingleOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task<List<EmployeeSelectDTO>> SearchEmployeeAsync(string email) {
        var employees = await GetAllAsync();

        if (!string.IsNullOrEmpty(email)) {
            var foundByUsername = employees.Where(e => e.FullName.Contains(email, StringComparison.InvariantCultureIgnoreCase));
            var foundByEmail = employees.Where(e => e.Email.Contains(email, StringComparison.InvariantCultureIgnoreCase));
            
            if (foundByUsername.Count() != 0) {
                return foundByUsername
                    .Select(e => new EmployeeSelectDTO {
                        Id = e.Id,
                        Email = e.Email,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        EmployeeSalaries = e.EmployeeSalaries
                            .Select( ems => new EmployeeSalaryDTO {
                                EmployeeId = ems.Id,
                                StartDate = ems.StartDate,
                                FinishDate = ems.StartDate,
                                Salary = float.Parse(ems.Salary.ToString())})
                            .ToList(),
                        EmployeeActivities = e.EmployeeActivities
                            .Select(ems =>
                                new EmployeeActivityDTO {
                                    EmployeeId = ems.Id,
                                    Employee = new EmployeeSelectDTO {
                                        Id = ems.EmployeeId,
                                        FirstName = ems.Employee.FirstName,
                                        LastName = ems.Employee.LastName,
                                        Email = ems.Employee.Email,
                                        Role = ems.Employee.Role, 
                                    },
                                    ActivityId = ems.ActivityId,
                                    Activity = new ActivitySelectDTO {
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
                    .Select(e => new EmployeeSelectDTO {
                        Id = e.Id,
                        Email = e.Email,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        EmployeeSalaries = e.EmployeeSalaries
                            .Select( ems => new EmployeeSalaryDTO {
                                EmployeeId = ems.Id,
                                StartDate = ems.StartDate,
                                FinishDate = ems.StartDate,
                                Salary = float.Parse(ems.Salary.ToString())})
                            .ToList(),
                        EmployeeActivities = e.EmployeeActivities
                            .Select(ems =>
                                new EmployeeActivityDTO {
                                    EmployeeId = ems.Id,
                                    Employee = new EmployeeSelectDTO {
                                        Id = ems.EmployeeId,
                                        FirstName = ems.Employee.FirstName,
                                        LastName = ems.Employee.LastName,
                                        Email = ems.Employee.Email,
                                        Role = ems.Employee.Role, 
                                    },
                                    ActivityId = ems.ActivityId,
                                    Activity = new ActivitySelectDTO {
                                        Id = ems.ActivityId,
                                        Name = ems.Activity.Name,
                                        StartDate = ems.Activity.StartDate,
                                        FinishDate = ems.Activity.FinishDate,
                                        WorkOrderId = ems.Activity.WorkOrderId
                                    }
                                }).ToList()
                    }).ToList();                   
            } else {
                return new List<EmployeeSelectDTO>();
            }
        } else {
            return new List<EmployeeSelectDTO>();
        }
    }

    public async Task<EmployeeEditForm> SearchEmployeeCompleteAsync(string email) {
        var employee = await _db.Employees
            .Select(e => new EmployeeEditForm {
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
                    .Select(ea => new EmployeeActivityDTO {
                        Id = ea.Activity.Id,
                        Activity = new ActivitySelectDTO {
                            Id = ea.Activity.Id,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        },
                        ActivityId = ea.Activity.Id,
                        Employee = new EmployeeSelectDTO {
                            Id = e.Id,
                            Email = e.Email
                        },
                        EmployeeId = e.Id
                    }).ToList(),
                EmployeeSalaries = e.Salaries
                    .Select( ems => new EmployeeSalaryDTO {
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

    public async Task<List<EmployeeViewDTO>> GetAllAsync() {
        return await _db.Employees
            .Select(e => new EmployeeViewDTO {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = $"{e.FirstName} {e.LastName}",
                BirthDay = e.BirthDay,
                StartDate = e.StartDate,
                Email = e.Email,
                Role = e.Role.ToString().ToUpper().Replace('_', ' '),
                EmployeeActivities = e.EmployeeActivity
                    .Select(ea => new EmployeeActivityDTO {
                        Id = ea.Activity.Id,
                        Activity = new ActivitySelectDTO {
                            Id = ea.Activity.Id,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        },
                        ActivityId = ea.Activity.Id,
                        Employee = new EmployeeSelectDTO {
                            Id = e.Id,
                            Email = e.Email
                        },
                        EmployeeId = e.Id
                    }).ToList(),
                EmployeeSalaries = e.Salaries
                    .Select( ems => new EmployeeSalaryDTO {
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = float.Parse(ems.Salary.ToString())})
                    .ToList(),
            })
            .ToListAsync();
    }

    public async Task<CurrentEmployee> GetUserIdAsync(string email) {
        return await _db.Employees
            .Select(e => new CurrentEmployee {
                Id = e.Id,
                Email = e.Email})
            .SingleOrDefaultAsync(e => e.Email == email);
    }
}