using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ApplicationUserController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationUserController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager) 
    {
        _userManager = userManager;    
        _roleManager = roleManager;
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPost("add/user")]
    public async Task<ActionResult> AddUser(EmployeeAddForm employeeAdd) {
        var person = await _userManager.FindByEmailAsync(employeeAdd.Email);
        
        if (person != null) {
            return BadRequest("Utente già esistente.");
        } else {
            var user = new IdentityUser {
                Email = employeeAdd.Email,
                EmailConfirmed = true,
                UserName = employeeAdd.Email
            };
            
            var userRole = await _roleManager.Roles
                .Where(aspRole => Enum.GetNames(typeof(Roles)).Any(role => role == aspRole.Name))
                .FirstAsync();
            
            await _userManager.CreateAsync(user, employeeAdd.Password);
            await _userManager.AddToRoleAsync(user, userRole.Name);
        }

        return Ok("Utente aggiunto con successo!");
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPut("edit/user/{oldEmail}")]
    public async Task<ActionResult> EditUser(EmployeeEditForm employeeEdit, string oldEmail) {
        var person = await _userManager.FindByEmailAsync(oldEmail);
        
        if (person == null) {
            return BadRequest("Utente non trovato!");
        } else if (person != null) {
            person.Email = employeeEdit.Email;
            person.EmailConfirmed = true;
            person.UserName = employeeEdit.Email;

            await _userManager.ChangePasswordAsync(person, person.PasswordHash, employeeEdit.Password);
            await _userManager.UpdateAsync(person);            
            
            var user = await _userManager.FindByNameAsync(oldEmail);
            var rolesList = await _userManager.GetRolesAsync(user);
            var userRole = rolesList.Single();

            var isInRole = await _userManager.IsInRoleAsync(person, userRole);
            if (isInRole) {
                await _userManager.RemoveFromRoleAsync(person, userRole);
                await _userManager.AddToRoleAsync(person, employeeEdit.Role.ToString());
                
                return Ok("Ruolo riassegnato.");
            } else {
                return BadRequest("Impossibile riassegnare il ruolo.");
            }
        }
        
        return Ok("Utente modificato con successo!");
    }
    
    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpDelete("delete/user/{email}")]
    public async Task<ActionResult> DeleteUser(string email) {
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null) {
            return BadRequest("Utente non trovato.");
        }
        
        await _userManager.DeleteAsync(user);
        return Ok("Utente eliminato con successo!");
    }
}