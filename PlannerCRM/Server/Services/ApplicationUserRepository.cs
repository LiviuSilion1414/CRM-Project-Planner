using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Server.CustomExceptions;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Services;

public class ApplicationUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationUserRepository(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager) 
    {
        _userManager = userManager;    
        _roleManager = roleManager;
    }

    public async Task AddAsync(EmployeeAddFormDto employeeAddFormDto) {
        if (employeeAddFormDto.GetType() == null) {
            throw new NullReferenceException("Oggetto null");
        } 
        
        var isNull = employeeAddFormDto.GetType().GetProperties()
            .All(prop => prop.GetValue(employeeAddFormDto) != null);
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }
        
        var person = await _userManager.FindByEmailAsync(employeeAddFormDto.Email);
        
        if (person != null) {
            throw new InvalidOperationException("Utente già presente");
        }

        var isAlreadyPresent = await _userManager.Users
            .SingleOrDefaultAsync(user => user.UserName == employeeAddFormDto.Email);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException("Oggetto già presente");
        }

        var user = new IdentityUser {
            Email = employeeAddFormDto.Email,
            EmailConfirmed = true,
            UserName = employeeAddFormDto.Email
        };
        
        var userRole = await _roleManager.Roles
            .FirstAsync(aspRole => Enum.GetNames(typeof(Roles))
                .Any(role => role == aspRole.Name));
        
        var creationResult = await _userManager.CreateAsync(user, employeeAddFormDto.Password);
        var assignmentResult = await _userManager.AddToRoleAsync(user, userRole.Name);
        
        if (!creationResult.Succeeded || !assignmentResult.Succeeded) {
            throw new InvalidOperationException("Impossibile proseguire");
        }
    }

    public async Task EditAsync(EmployeeEditFormDto employeeEditFormDto, string oldEmail) {
        if (employeeEditFormDto.GetType() == null) {
            throw new NullReferenceException("Oggetto null");
        } 
        
        var isNull = employeeEditFormDto.GetType().GetProperties()
            .All(em => em.GetValue(employeeEditFormDto) != null);
        
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }

        if (string.IsNullOrEmpty(oldEmail)) {
            throw new NullReferenceException("Oggetto null");
        }

        var user = await _userManager.FindByEmailAsync(oldEmail);
        
        if (user == null) {
            throw new KeyNotFoundException("Oggetto non trovato");
        } 

        user.Email = employeeEditFormDto.Email;
        user.EmailConfirmed = true;
        user.UserName = employeeEditFormDto.Email;

        var passChangeResult = await _userManager.ChangePasswordAsync(user, user.PasswordHash, employeeEditFormDto.Password);
        var updateResult = await _userManager.UpdateAsync(user);

        if (!passChangeResult.Succeeded || !updateResult.Succeeded) {
            throw new InvalidOperationException("Impossibile proseguire");
        } 
        
        var currentUser = await _userManager.FindByNameAsync(oldEmail);
        var rolesList = await _userManager.GetRolesAsync(currentUser);
        var userRole = rolesList.Single();

        var isInRole = await _userManager.IsInRoleAsync(user, userRole);
        
        if (isInRole) {
            await _userManager.RemoveFromRoleAsync(user, userRole);
            await _userManager.AddToRoleAsync(user, employeeEditFormDto.Role.ToString());
        } else {
            throw new InvalidOperationException("Impossibile modificare il ruolo.");
        } 
    }
    
    public async Task DeleteAsync(string email) {
        if (string.IsNullOrEmpty(email)) {
            throw new NullReferenceException("Oggetto null");
        }

        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null) {
            throw new KeyNotFoundException("Utente non trovato.");
        }
        
        await _userManager.DeleteAsync(user);
    }
}