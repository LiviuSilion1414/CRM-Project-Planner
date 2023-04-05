using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(EmployeeLoginDTO employee) {
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

    [Authorize]
    [HttpPost("add/user")]
    public async Task<ActionResult> AddUser(EmployeeForm employeeAdd) {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        var person = await _userManager.FindByEmailAsync(employeeAdd.Email);

        if (person == null) {
            var user = new IdentityUser {
                Email = employeeAdd.Email,
                EmailConfirmed = true,
                UserName = employeeAdd.Email
            };

            var result = await _userManager.CreateAsync(user, employeeAdd.Password);

            await _userManager.AddToRoleAsync(user, employeeAdd.Role.ToString());
        }
        return Ok();
    }

    [Authorize]
    [HttpPut("edit/user/{oldEmail}")]
    public async Task EditUser(EmployeeForm employeeEdit, string oldEmail) {
        var person = await _userManager.FindByEmailAsync(oldEmail);

        if (person != null) {
            person = new IdentityUser {
                Email = employeeEdit.Email,
                EmailConfirmed = true,
                UserName = employeeEdit.Email
            };

            await _userManager.CreateAsync(person, employeeEdit.Password);

            await _userManager.AddToRoleAsync(person, employeeEdit.Role.ToString());
        }
    }
    
    [Authorize]
    [HttpDelete("delete/user/{email}")]
    public async Task DeleteUser(string email) {
        var user = await _userManager.FindByEmailAsync(email);

        await _userManager.DeleteAsync(user);
    }

    [HttpGet]
    [Route("user/role")]
    public async Task<IList<string>> GetUserRole() {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var roles = await _userManager.GetRolesAsync(user);
        
        return roles;
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task Logout() {
        await _signInManager.SignOutAsync();
    }

    [HttpGet("current/user/info")]
    public CurrentUser CurrentUserInfo() {
        return new CurrentUser {
            IsAuthenticated = User.Identity.IsAuthenticated,
            UserName = User.Identity.Name,
            Claims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value)
        };
    }
}