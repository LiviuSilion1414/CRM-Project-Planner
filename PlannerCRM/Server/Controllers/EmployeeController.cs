using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Models;
using PlannerCRM.Server.Services.Interfaces;
using PlannerCRM.Shared.DTOs;
using PlannerCRM.Shared.DTOs.Abstract;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController: ControllerBase
{
    private readonly IRepository<Employee> _repo;

    public EmployeeController(IRepository<Employee> repo) {
        _repo = repo;
    }

    [HttpPost("add")]
    public async Task AddUser(Employee employeeAdd) {
        await _repo.AddAsync(employeeAdd);
    }

    [HttpPut("edit")]
    public async Task EditUser(Employee employeeEdit) {
        await _repo.EditAsync(employeeEdit.Id, employeeEdit);
    }


    [HttpDelete("delete/{id}")]
    public async Task DeleteUser(int id) {
        await _repo.DeleteAsync(id);
    }

    [HttpGet("get/{id}")]
    public async Task<Employee> GetById(int id) {
       return await _repo.GetAsync(id);
    }
    
    [HttpGet("get/all")]
    public async Task<List<Employee>> GetAll() {
        return await _repo.GetAllAsync();
    }
}