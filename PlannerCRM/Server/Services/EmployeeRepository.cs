using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs;

namespace PlannerCRM.Server.Services;

public class EmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(EmployeeAddDTO entity) {
        _db.Employees.Add(
            new Employee {
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Birthday = DateTime.Parse(entity.BirthDay),
                StartDate = DateTime.Parse(entity.StartDate),
                Password = entity.Password,
                NumericCode = entity.NumericCode,
                Role = entity.Role,
                Salaries = new List<EmployeeSalaries> {
                    new EmployeeSalaries {
                        EmployeeId = entity.Id,
                        StartDate = DateTime.Parse(entity.StartDate),
                        FinishDate = DateTime.Parse(entity.StartDate),
                        Salary = entity.HourPay
                    }
                }
        });

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var entity = await _db.Employees.SingleOrDefaultAsync(e => e.Id == id);
        _db.Employees.Remove(entity);

        await _db.SaveChangesAsync();
    }

    public async Task<bool> EditAsync(EmployeeEditDTO entity) {
        var model = await _db.Employees.SingleOrDefaultAsync(e => e.Id == entity.Id);
        
        if (model == null) {
            return false;
        } else {
            model.Id = entity.Id;
            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.Birthday = DateTime.Parse(entity.BirthDay);
            model.StartDate = DateTime.Parse(entity.StartDate);
            model.Email = entity.Email;
            model.Role = entity.Role;
            model.NumericCode = entity.NumericCode;
            model.Salaries = new List<EmployeeSalaries> {
                new EmployeeSalaries {
                    EmployeeId = entity.Id,
                    StartDate = DateTime.Parse(entity.StartDate),
                    FinishDate = DateTime.Parse(entity.StartDate),
                    Salary = entity.HourPay
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
                Username = e.Email,
                FullName = $"{e.FirstName} {e.LastName}",
                Birthday = e.Birthday.ToShortDateString(),
                Email = e.Email,
                Role = e.Role.ToString().Replace('_', ' ')})
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<EmployeeEditDTO> GetForEditAsync(int id) {
        return await _db.Employees
            .Select(e => new EmployeeEditDTO {
                Id = e.Id,
                Username = e.Email,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                BirthDay = e.Birthday.ToString(),
                StartDate = e.StartDate.ToString(),
                Role = e.Role,
                NumericCode = e.NumericCode,
                Password = e.Password})
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<EmployeeViewDTO>> GetAllAsync() {
        return await _db.Employees
            .Select(e => new EmployeeViewDTO {
                Id = e.Id,
                Username = e.Email,
                FullName = $"{e.FirstName} {e.LastName}",
                Birthday = e.Birthday.ToShortDateString(),
                Email = e.Email,
                Role = e.Role.ToString().Replace('_', ' ')})
            .ToListAsync();
    }
}