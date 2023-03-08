using PlannerCRM.Server.Services.Interfaces;
using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace PlannerCRM.Server.Services.ConcreteClasses;

public class EmployeeRepository : IRepository<Employee>
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db) {
        _db = db;
    }

    public async Task AddAsync(Employee entity) {
        _db.Employees.Add(entity);

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var entity = await _db.Employees.SingleOrDefaultAsync(e => e.Id == id);
        _db.Employees.Remove(entity);

        await _db.SaveChangesAsync();
    }

    public async Task<bool> EditAsync(int id, Employee entity) {
        var model = await _db.Employees.SingleOrDefaultAsync(e => e.Id == id);
        
        if (model == null) {
            return false;
        }

        model.Id = entity.Id;
        model.FirstName = entity.FirstName;
        model.LastName = entity.LastName;
        model.Birthday = entity.Birthday;
        model.Email = entity.Email;
        model.Role = entity.Role;
        model.StartDate = entity.StartDate;
        model.NumericCode = entity.NumericCode;
        model.Salaries = entity.Salaries;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Employee> GetAsync(int id) {
        return await _db.Employees
            .Include(e => e.Salaries)
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Employee>> GetAllAsync() {
        return await _db.Employees
            .Include(e => e.Salaries)
            .ToListAsync();
    }
}