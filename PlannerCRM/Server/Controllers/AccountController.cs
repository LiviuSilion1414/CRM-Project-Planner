using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(EmployeeLoginDTO employeeLogin) {
        var user = await _userManager.FindByEmailAsync(employeeLogin.Email);

        if (user == null) {
            return BadRequest("Utente non trovato!");
        } else {
            var userPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, employeeLogin.Password);

            if (!userPasswordIsCorrect) {
                return BadRequest("Password sbagliata!");
            } else {
                await _signInManager.SignInAsync(user, true);
                return Ok("Connesso!");
            }
        }
    }
    
    [Authorize]
    [HttpGet("logout")]
    public async Task Logout() {
        await _signInManager.SignOutAsync();
    }

    [HttpGet]
    [Route("user/role")]
    public async Task<string> GetUserRole() {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var roles = (await _userManager.GetRolesAsync(user));

        return roles.Single();
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