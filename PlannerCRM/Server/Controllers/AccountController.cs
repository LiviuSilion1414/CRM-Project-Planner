using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Models;
using PlannerCRM.Server.Services.ConcreteClasses;
using PlannerCRM.Shared;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly EmployeeRepository _repo;

    public AccountController(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager, 
        EmployeeRepository repo
    )  {
        _userManager = userManager;
        _signInManager = signInManager;
        _repo = repo;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(Employee employee) {
        if (string.IsNullOrEmpty(employee.Email)) {
            return BadRequest("""Il campo "Email" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.Password)) {
            return BadRequest("""Il campo "Password" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.Email) && string.IsNullOrEmpty(employee.Password)){
            return BadRequest("""I campi "Email" e "Password" non devono essere vuoti!""");
        } else {
            var user = await _userManager.FindByEmailAsync(employee.Email);

            if (user == null) {
                return BadRequest("Utente non trovato!");
            } else {
                var userPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, employee.Password);
                
                if (!userPasswordIsCorrect) {
                    return BadRequest("Password sbagliata!");
                } else {
                    await _signInManager.SignInAsync(user, true);
                    return Ok();
                }
            }
        }
    }

    [HttpPost("logout")]
    public async Task Logout() {
        await _signInManager.SignOutAsync();
    }
    
    [HttpPost("add/employee")]
    public async Task<ActionResult> AddEmployee(Employee employee) {
        if (string.IsNullOrEmpty(employee.Email)) {
            return BadRequest("""Il campo "Email" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.Password)) {
            return BadRequest("""Il campo "Password" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.FirstName)) {
            return BadRequest("""Il campo "Nome" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.LastName)) {
            return BadRequest("""Il campo "Cognome" non deve essere vuoto!""");
        } else if (employee.Birthday == null) {
            return BadRequest("""Il campo "Data di nascita" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.NumericCode)) {
            return BadRequest("""Il campo "Codice numerico" non deve essere vuoto!""");
        } else if (employee.StartDate == null) {
            return BadRequest("""Il campo "Data di inizio" non deve essere vuoto!""");
        } else if (employee.Salaries == null) {
            return BadRequest("""Il campo "Tariffa oraria" non deve essere vuoto!""");
        } else {
            await _repo.AddAsync(employee);
            return Ok();
        }
    }

    [HttpPut("edit/employee")]
    public async Task<ActionResult> EditEmployee(Employee employee) {
        if (string.IsNullOrEmpty(employee.Email)) {
            return BadRequest("""Il campo "Email" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.Password)) {
            return BadRequest("""Il campo "Password" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.FirstName)) {
            return BadRequest("""Il campo "Nome" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.LastName)) {
            return BadRequest("""Il campo "Cognome" non deve essere vuoto!""");
        } else if (employee.Birthday == null) {
            return BadRequest("""Il campo "Data di nascita" non deve essere vuoto!""");
        } else if (string.IsNullOrEmpty(employee.NumericCode)) {
            return BadRequest("""Il campo "Codice numerico" non deve essere vuoto!""");
        } else if (employee.StartDate == null) {
            return BadRequest("""Il campo "Data di inizio" non deve essere vuoto!""");
        } else if (employee.Salaries == null) {
            return BadRequest("""Il campo "Tariffa oraria" non deve essere vuoto!""");
        } else {
            await _repo.EditAsync(employee.Id, employee);
            return Ok();
        }
    }

    [HttpDelete("delete/employee")]
    public async Task<ActionResult> DeleteEmployee(Employee employee) {
        await _repo.DeleteAsync(employee.Id);
        return Ok();
    }

    [HttpGet("get/all")]
    public async Task<IEnumerable<Employee>> GetAll() {
        return await _repo.GetAllAsync();
    }
}