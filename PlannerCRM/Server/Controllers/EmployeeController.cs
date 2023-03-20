using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Models;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs;
using PlannerCRM.Shared.DTOs.Abstract;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController: ControllerBase
{
    private readonly EmployeeRepository _repo;

    public EmployeeController(EmployeeRepository repo) {
        _repo = repo;
    }

    [HttpPost("add")]
    public async Task AddUser(EmployeeAddDTO employeeAdd) {
        await _repo.AddAsync(employeeAdd);
    }

    [HttpPut("edit")]
    public async Task EditUser(EmployeeEditDTO employeeEdit) {
        await _repo.EditAsync(employeeEdit);
    }

    [HttpDelete("delete/{id}")]
    public async Task DeleteUser(int id) {
        await _repo.DeleteAsync(id);
    }

    [HttpGet("get/{id}")]
    public async Task<EmployeeViewDTO> GetById(int id) {
       return await _repo.GetAsync(id);
    }
    
    [HttpGet("get/all")]
    public async Task<List<EmployeeViewDTO>> GetAll() {
        return await _repo.GetAllAsync();
    }
}