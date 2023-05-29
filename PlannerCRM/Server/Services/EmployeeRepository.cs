using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.CustomExceptions;
using static PlannerCRM.Shared.Constants.ExceptionsMessages;

namespace PlannerCRM.Server.Services;

public class EmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(EmployeeAddFormDto dto) {
        if (dto == null) {
            throw new NullReferenceException(NULL_OBJECT);
        }

        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        if (HasPropertiesNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }
        
        var isAlreadyPresent = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == dto.Id);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException(OBJECT_ALREADY_PRESENT);
        }

        await _db.Employees.AddAsync(new Employee {
            Id = dto.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            FullName = $"{dto.FirstName + dto.LastName}",
            BirthDay = dto.BirthDay ?? throw new NullReferenceException(NULL_PROP),
            StartDate = dto.StartDate ?? throw new NullReferenceException(NULL_PARAM),
            Password = dto.Password,
            NumericCode = dto.NumericCode,
            Role = dto.Role ?? throw new NullReferenceException(NULL_PARAM),
            Salaries = dto.EmployeeSalaries
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
            throw new DbUpdateException(IMPOSSIBLE_GOING_FORWARD);
        }
    }

    public async Task DeleteAsync(int id) {
        var employeeDelete = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == id);

        if (employeeDelete == null) {
            throw new KeyNotFoundException(IMPOSSIBLE_DELETE);
        }

        _db.Employees.Remove(employeeDelete);
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBLE_GOING_FORWARD);
        }
    }

    public async Task EditAsync(EmployeeEditFormDto dto) {
        if (dto == null) {
            throw new NullReferenceException(NULL_OBJECT);
        }

       // var HasPropertiesNull = dto.GetType().GetProperties()  //throws exception
       //     .Any(prop => prop.GetValue(dto) == null);
       // if (HasPropertiesNull) {
       //     throw new ArgumentNullException(NULL_PARAM);
       // }

        var model = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == dto.Id);
        
        if (model == null) {
            throw new KeyNotFoundException(OBJECT_NOT_FOUND);
        }

        model.Id = dto.Id;
        model.FirstName = dto.FirstName;
        model.LastName = dto.LastName;
        model.FullName = $"{dto.FirstName + dto.LastName}";
        model.BirthDay = dto.BirthDay;
        model.StartDate = dto.StartDate;
        model.Email = dto.Email;
        model.Role = dto.Role;
        model.NumericCode = dto.NumericCode;
        model.Salaries = dto.EmployeeSalaries
            .Select(ems => 
                new EmployeeSalary {
                    EmployeeId = ems.Id,
                    StartDate = ems.StartDate,
                    FinishDate = ems.FinishDate,
                    Salary = decimal.Parse(ems.Salary.ToString())
                }
            ).ToList();
        _db.Employees.Update(model);
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBLE_GOING_FORWARD);
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
                            Employee = _db.Employees
                            .Select(em => new EmployeeSelectDto {
                                Id = em.Id,
                                Email = em.Email,
                                FirstName = em.FirstName,
                                LastName = em.LastName,
                                Role = em.Role
                            })
                            .Single(em => em.Id == ea.EmployeeId),
                            ActivityId = ea.ActivityId,
                            Activity = _db.Activities
                                .Select(ac => new ActivitySelectDto {
                                    Id = ac.Id,
                                    Name = ac.Name,
                                    StartDate = ac.StartDate,
                                    FinishDate = ac.FinishDate,
                                    WorkOrderId = ac.WorkOrderId
                                })
                                .Single(ac => ac.Id == ea.ActivityId)
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
                OldEmail = em.Email,
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
        return await _db.Employees
            .Select(em => new EmployeeSelectDto {
                Id = em.Id,
                Email = em.Email,
                FirstName = em.FirstName,
                LastName = em.LastName,
                FullName = em.FullName,
                Role = em.Role
            })
            .Where(em => EF.Functions.Like(em.FullName, $"%{email}%") || EF.Functions.Like(em.Email, $"%{email}%"))
            .ToListAsync();
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
                        Id = ea.ActivityId,
                        EmployeeId = em.Id,
                        Employee = _db.Employees
                            .Select(em => new EmployeeSelectDto {
                                Id = em.Id,
                                Email = em.Email,
                                FirstName = em.FirstName,
                                LastName = em.LastName,
                                Role = em.Role
                            })
                            .Single(em => em.Id == ea.EmployeeId),
                        ActivityId = ea.ActivityId,
                        Activity = _db.Activities
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