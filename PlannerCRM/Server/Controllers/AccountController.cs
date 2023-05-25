using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;
using static PlannerCRM.Shared.Constants.LoginFeedBack;

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
    public async Task<ActionResult> Login(EmployeeLoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return NotFound(USER_NOT_FOUND);
        }
        else
        {
            var userPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!userPasswordIsCorrect)
            {
                return BadRequest(WRONG_PASSWORD);
            }
            else
            {
                await _signInManager.SignInAsync(user, false);

                return Ok(CONNECTED);
            }
        }
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    [HttpGet("user/role")]
    public async Task<string> GetUserRole()
    {
        if (User.Identity.IsAuthenticated) {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.Single();
        } else {
            return null;
        }
    }

    [HttpGet("current/user/info")]
    public CurrentUser CurrentUserInfo()
    {
        return new CurrentUser
        {
            IsAuthenticated = User.Identity.IsAuthenticated,
            UserName = User.Identity.Name,
            Claims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value)
        };
    }
}