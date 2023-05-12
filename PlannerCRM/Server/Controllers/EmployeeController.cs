using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using static PlannerCRM.Shared.Constants.ConstantValues;

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
    public async Task<ActionResult> AddUser(EmployeeAddFormDto employeeAddFormDto) {
        var employees = await _repo.SearchEmployeeAsync(employeeAddFormDto.Email);

        if (employeeAddFormDto == null) {
            return BadRequest("Impossibile aggiungere l'utente.");
        } else {
            if (employees.Count() != 0) {
                return BadRequest("Utente già esistente!");
            } else {
                await _repo.AddAsync(employeeAddFormDto);

                return Ok("Utente aggiunto con successo!");
            }
        }
    }

    [Authorize]
    [HttpPut("edit")]
    public async Task<ActionResult> EditUser(EmployeeEditFormDto employeeEditFormDto) {
        var employees = await _repo.SearchEmployeeAsync(employeeEditFormDto.Email);
        
        if (employeeEditFormDto == null) {
            return NotFound("Utente non trovato!");
        } else {
            await _repo.EditAsync(employeeEditFormDto);

            return Ok("Utente modificato con successo!");
        }
    }

    [Authorize]
    [HttpDelete("delete/{employeeId}")]
    public async Task<ActionResult> DeleteUser(int employeeId) {
        var employee = await _repo.GetForViewAsync(employeeId);
        
        if (employee == null) {
            return NotFound("Utente non trovato!");
        } else {
            await _repo.DeleteAsync(employeeId);

            return Ok("Utente eliminato con successo!");
        }
    }

    [Authorize]
    [HttpGet("get/for/view/{employeeId}")]
    public async Task<EmployeeViewDto> GetForViewById(int employeeId) {
       return await _repo.GetForViewAsync(employeeId);
    }

    [Authorize]
    [HttpGet("get/for/edit/{employeeId}")]
    public async Task<EmployeeEditFormDto> GetForEditById(int employeeId) {
       return await _repo.GetForEditAsync(employeeId);
    }

    [Authorize]
    [HttpGet("get/for/delete/{employeeId}")]
    public async Task<EmployeeDeleteDto> GetForDeleteById(int employeeId) {
       return await _repo.GetForDeleteAsync(employeeId);
    }

    [Authorize]
	[HttpGet("search/{email}")]
	public async Task<List<EmployeeSelectDto>> SearchEmployee(string email) {
		return await _repo.SearchEmployeeAsync(email);
	}

    [Authorize]
	[HttpGet("search/complete/{email}")]
	public async Task<EmployeeEditFormDto> SearchEmployeeComplete(string email) {
		return await _repo.SearchEmployeeCompleteAsync(email);
	}

    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<EmployeeViewDto>> GetAll() {
        return await _repo.GetAllAsync();
    }

    [Authorize]
    [HttpGet("get/id/{email}")]
    public async Task<CurrentEmployeeDto> GetUserId(string email) {
        var emptyEmployee = new CurrentEmployeeDto {
            Id = INVALID_ID,
            Email = null
        };
        
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email)) {
            return emptyEmployee;
        } else {
            var employee = await _repo.SearchEmployeeAsync(email);

            if (employee == null) {
                return emptyEmployee;
            }
        }
    
        return await _repo.GetUserIdAsync(email);
    }

    [HttpGet("get/id-check/{email}")]
    public async Task<ActionResult> GetUserIdCheck(string email) {
        var emptyEmployee = new CurrentEmployeeDto {
            Id = INVALID_ID,
            Email = null
        };
        
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email)) {
            return BadRequest("L'email non può essere vuoto!");
        } else {
            var employee = await _repo.SearchEmployeeAsync(email);
            
            if (employee == null) {
                return NotFound("Utente non trovato!");
            }
        }
        return Ok("OK");
    }
}