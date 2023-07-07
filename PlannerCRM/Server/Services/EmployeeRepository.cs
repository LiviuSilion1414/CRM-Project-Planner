using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Server.DataAccess;
using PlannerCRM.Shared.Constants;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Services;

public class EmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(EmployeeFormDto dto) {
        if (dto is null) 
            throw new NullReferenceException(ExceptionsMessages.NULL_OBJECT);

        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) is null);
        if (HasPropertiesNull)
            throw new ArgumentNullException(ExceptionsMessages.NULL_PARAM);
        
        var isAlreadyPresent = await _db.Employees
            .SingleOrDefaultAsync((Employee em) => EF.Functions.ILike(em.Email, $"%{dto.Email}%"));
        if (isAlreadyPresent != null)
            throw new DuplicateElementException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);

        await _db.Employees.AddAsync(new Employee {
            Id = dto.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            FullName = $"{dto.FirstName} {dto.LastName}",
            BirthDay = dto.BirthDay ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
            StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
            Password = dto.Password,
            NumericCode = dto.NumericCode,
            Role = dto.Role ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
            CurrentHourlyRate = dto.CurrentHourlyRate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
            Salaries = dto.EmployeeSalaries
                .Select(ems =>
                    new EmployeeSalary {
                        EmployeeId = ems.EmployeeId,
                        StartDate = ems.StartDate,
                        FinishDate = ems.FinishDate,
                        Salary = ems.Salary
                    })
                .ToList()
        });
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0)
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
    }

    public async Task DeleteAsync(int id) {
        var employeeDelete = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == id);

        if (employeeDelete is null)
            throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);
        
        employeeDelete.IsDeleted = true;

        _db.Update(employeeDelete);

        await _db.SaveChangesAsync();
    }

    public async Task EditAsync(EmployeeFormDto dto) {
        if (dto is null)
            throw new NullReferenceException(ExceptionsMessages.NULL_OBJECT);

        var model = await _db.Employees
            .Where(em => !em.IsDeleted)
            .SingleOrDefaultAsync(em => em.Id == dto.Id);
        
        if (model is null)
            throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);

        model.Id = dto.Id;
        model.FirstName = dto.FirstName;
        model.LastName = dto.LastName;
        model.FullName = $"{dto.FirstName + dto.LastName}";
        model.BirthDay = dto.BirthDay ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
        model.StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
        model.Email = dto.Email;
        model.Role = dto.Role ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
        model.NumericCode = dto.NumericCode;
        model.CurrentHourlyRate = dto.CurrentHourlyRate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
        model.Salaries = dto.EmployeeSalaries
            .Where(ems => _db.Employees
                .Any(em => em.Id == ems.EmployeeId))
            .Select(ems => 
                new EmployeeSalary {
                    EmployeeId = dto.Id,
                    StartDate = ems.StartDate,
                    FinishDate = ems.FinishDate,
                    Salary = ems.Salary
                }
            ).ToList();
        
        _db.Employees.Update(model);
        
        var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0)
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
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
                NumericCode = em.NumericCode,
                Password = em.Password,
                StartDateHourlyRate = em.Salaries.Single().StartDate,
                FinishDateHourlyRate = em.Salaries.Single().FinishDate,
                Email = em.Email,
                IsDeleted = em.IsDeleted,
                Role = em.Role, 
                CurrentHourlyRate = em.CurrentHourlyRate,
                EmployeeSalaries = em.Salaries
                    .Select( ems => new EmployeeSalaryDto {
                        Id = ems.Id,
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary})
                    .ToList(),
                EmployeeActivities = em.EmployeeActivity
                    .Select( ea =>
                        new EmployeeActivityDto {
                            EmployeeId = ea.Id,
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
            .SingleOrDefaultAsync(em => em.Id == id);
    }

    public async Task<EmployeeFormDto> GetForEditAsync(int id) { 
        return await _db.Employees
            .Where(em => !em.IsDeleted)
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
                    .Where(ems => _db.Employees
                        .Any(em => em.Id == ems.EmployeeId))
                    .Select(ems => new EmployeeSalaryDto {
                        Id = ems.Id,
                        EmployeeId = em.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary})
                    .ToList()
                })
            .SingleOrDefaultAsync(em => em.Id == id);
    }

    public async Task<EmployeeDeleteDto> GetForDeleteAsync(int id) {
        return await _db.Employees
            .Where(em => !em.IsDeleted)
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
            .Where(em => !em.IsDeleted)
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

    public async Task<List<EmployeeViewDto>> GetPaginatedEmployees(int skip, int take) {
        return await _db.Employees
            .OrderBy(em => em.Id)
            .Skip(skip)
            .Take(take)
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
                        Id = ems.Id,
                        EmployeeId = ems.Id,
                        StartDate = ems.StartDate,
                        FinishDate = ems.StartDate,
                        Salary = ems.Salary})
                    .ToList(),
            })
            .ToListAsync();
    }

    public async Task<CurrentEmployeeDto> GetUserIdAsync(string email) {
        return await _db.Employees
            .Where(em => !em.IsDeleted)
            .Select(em => new CurrentEmployeeDto {
                Id = em.Id,
                Email = em.Email})
            .FirstOrDefaultAsync(em => em.Email == email);
    }

    public async Task<int> GetEmployeesSize() {
        return await _db.Employees.CountAsync();
    }
}