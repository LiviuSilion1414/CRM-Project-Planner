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
}
