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

    public async Task AddAsync(EmployeeAddFormDto dto) {
        if (dto.GetType() == null) {
            throw new NullReferenceException("Oggetto null");
        } 
        
        var isNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }
        
        var person = await _userManager.FindByEmailAsync(dto.Email);
        
        if (person != null) {
            throw new InvalidOperationException("Utente già presente");
        }

        var isAlreadyPresent = await _userManager.Users
            .SingleOrDefaultAsync(user => user.UserName == dto.Email);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException("Oggetto già presente");
        }

        var user = new IdentityUser {
            Email = dto.Email,
            EmailConfirmed = true,
            UserName = dto.Email
        };
        
        var userRole = await _roleManager.Roles
            .FirstAsync(aspRole => Enum.GetNames(typeof(Roles))
                .Any(role => role == aspRole.Name));
        
        var creationResult = await _userManager.CreateAsync(user, dto.Password);
        var assignmentResult = await _userManager.AddToRoleAsync(user, userRole.Name);
        
        if (!creationResult.Succeeded || !assignmentResult.Succeeded) {
            throw new InvalidOperationException("Impossibile proseguire");
        }
    }

    public async Task EditAsync(EmployeeEditFormDto dto, string oldEmail) {
        if (dto.GetType() == null) {
            throw new NullReferenceException("Oggetto null");
        } 
        
        var isNull = dto.GetType().GetProperties()
            .Any(em => em.GetValue(dto) != null);
        
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

        user.Email = dto.Email;
        user.EmailConfirmed = true;
        user.UserName = dto.Email;

        var passChangeResult = await _userManager.ChangePasswordAsync(user, user.PasswordHash, dto.Password);
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
            await _userManager.AddToRoleAsync(user, dto.Role.ToString());
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