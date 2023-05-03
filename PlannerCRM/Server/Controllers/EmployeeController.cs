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
    public async Task<ActionResult> AddUser(EmployeeAddForm employeeAdd) {
        var employees = await _repo.SearchEmployeeAsync(employeeAdd.Email);

        if (employeeAdd == null) {
            return BadRequest("Impossibile aggiungere l'utente.");
        } else {
            if (employees.Count() != 0) {
                return BadRequest("Utente già esistente!");
            } else {
                await _repo.AddAsync(employeeAdd);

                return Ok("Utente aggiunto con successo!");
            }
        }
    }

    [Authorize]
    [HttpPut("edit")]
    public async Task<ActionResult> EditUser(EmployeeEditForm employeeEdit) {
        var employees = await _repo.SearchEmployeeAsync(employeeEdit.Email);
        
        if (employeeEdit == null) {
            return NotFound("Utente non trovato!");
        } else {
            if (employees.Count() != 0) {
                return BadRequest("Utente già esistente!");
            } else {
                await _repo.EditAsync(employeeEdit);

                return Ok("Utente modificato con successo!");
            }
        }
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteUser(int id) {
        var employee = await _repo.GetForViewAsync(id);
        
        if (employee == null) {
            return NotFound("Utente non trovato!");
        } else {
            await _repo.DeleteAsync(id);

            return Ok("Utente eliminato con successo!");
        }
    }

    [Authorize]
    [HttpGet("get/for/view/{id}")]
    public async Task<EmployeeViewDTO> GetForViewById(int id) {
       return await _repo.GetForViewAsync(id);
    }

    [Authorize]
    [HttpGet("get/for/edit/{id}")]
    public async Task<EmployeeEditForm> GetForEditById(int id) {
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
	public async Task<EmployeeEditForm> SearchEmployeeComplete(string email) {
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