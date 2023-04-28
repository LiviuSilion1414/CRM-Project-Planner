using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EmployeeController: ControllerBase
{
    private readonly EmployeeRepository _repo;

    public EmployeeController(EmployeeRepository repo) {
        _repo = repo;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task AddUser(EmployeeForm employeeAdd) {
        await _repo.AddAsync(employeeAdd);
    }

    [Authorize]
    [HttpPut("edit")]
    public async Task EditUser(EmployeeForm employeeEdit) {
        await _repo.EditAsync(employeeEdit);
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task DeleteUser(int id) {
        await _repo.DeleteAsync(id);
    }

    [Authorize]
    [HttpGet("get/for/view/{id}")]
    public async Task<EmployeeViewDTO> GetForViewById(int id) {
       return await _repo.GetForViewAsync(id);
    }

    [Authorize]
    [HttpGet("get/for/edit/{id}")]
    public async Task<EmployeeForm> GetForEditById(int id) {
       return await _repo.GetForEditAsync(id);
    }

    [Authorize]
    [HttpGet("get/for/delete/{id}")]
    public async Task<EmployeeDeleteDTO> GetForDeleteById(int id) {
       return await _repo.GetForDeleteAsync(id);
    }

    [Authorize]
	[HttpGet("search/{email}")]
	public async Task<List<EmployeeSelectDTO>> SearchEmployee(string email) {
		return await _repo.SearchEmployeeAsync(email);
	}

    [Authorize]
	[HttpGet("search/complete/{email}")]
	public async Task<EmployeeForm> SearchEmployeeComplete(string email) {
		return await _repo.SearchEmployeeCompleteAsync(email);
	}


    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<EmployeeViewDTO>> GetAll() {
        return await _repo.GetAllAsync();
    }

    [HttpGet("get/id/{email}")]
    public async Task<CurrentEmployee> GetUserId(string email) {
        return await _repo.GetUserIdAsync(email);
    }
}