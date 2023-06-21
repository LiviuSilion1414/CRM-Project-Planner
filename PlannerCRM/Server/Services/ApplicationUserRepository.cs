using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;
using static PlannerCRM.Shared.Constants.ExceptionsMessages;
using static PlannerCRM.Shared.Constants.ConstantValues;

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
            throw new NullReferenceException(NULL_OBJECT);
        } 
        
        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        if (HasPropertiesNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }
        
        var person = await _userManager.FindByEmailAsync(dto.Email);
        
        if (person != null) {
            throw new InvalidOperationException(OBJECT_ALREADY_PRESENT);
        }

        if (dto.Role == ADMIN_ROLE) {
            throw new InvalidOperationException(NOT_ASSEGNABLE_ROLE);
        }

        var isAlreadyPresent = await _userManager.Users
            .SingleOrDefaultAsync(user => user.UserName == dto.Email);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException(OBJECT_ALREADY_PRESENT);
        }

        var user = new IdentityUser {
            Email = dto.Email,
            EmailConfirmed = true,
            UserName = dto.Email,
        };
        
        var foundUserRole = await _roleManager.Roles.SingleAsync(aspRole => aspRole.Name == dto.Role.ToString());

        var creationResult = await _userManager.CreateAsync(user, dto.Password);
        if (!creationResult.Succeeded) {
            foreach (var error in creationResult.Errors) {
                Console.WriteLine("Errors: {0}", error.Description);
            }
            throw new DbUpdateException();
        }
        
        var assignmentResult = await _userManager.AddToRoleAsync(user, foundUserRole.Name);
        
        if (!creationResult.Succeeded || !assignmentResult.Succeeded) {
            throw new InvalidOperationException(IMPOSSIBLE_GOING_FORWARD);
        }
    }

    public async Task EditAsync(EmployeeEditFormDto dto) {
        if (dto.GetType() == null) {
            throw new NullReferenceException(NULL_OBJECT);
        } 
        
        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(em => em.GetValue(dto) == null);
        
        if (HasPropertiesNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }

        if (dto.Role == ADMIN_ROLE) {
            throw new InvalidOperationException(NOT_ASSEGNABLE_ROLE);
        }

        var user = await _userManager.FindByEmailAsync(dto.OldEmail);
        
        if (user == null) {
            throw new KeyNotFoundException(OBJECT_NOT_FOUND);
        } 

        user.Email = dto.Email;
        user.EmailConfirmed = true;
        user.UserName = dto.Email;

        var passChangeResult = await _userManager.RemovePasswordAsync(user);
        var updateResult = await _userManager.AddPasswordAsync(user, dto.Password);

        if (!passChangeResult.Succeeded || !updateResult.Succeeded) {
            throw new InvalidOperationException(IMPOSSIBLE_GOING_FORWARD);
        } 
        
        var rolesList = await _userManager.GetRolesAsync(user);
        var userRole = rolesList.Single();

        var isInRole = await _userManager.IsInRoleAsync(user, userRole);
        
        if (isInRole) {
            var deleteRoleResult = await _userManager.RemoveFromRoleAsync(user, userRole);
            var reassignmentRoleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            
            if (!deleteRoleResult.Succeeded || !reassignmentRoleResult.Succeeded) {
                throw new InvalidOperationException(IMPOSSIBLE_GOING_FORWARD);
            }
        } else {
            throw new InvalidOperationException(IMPOSSIBLE_EDIT);
        } 
    }
    
    public async Task DeleteAsync(string email) {
        if (string.IsNullOrEmpty(email)) {
            throw new NullReferenceException(NULL_OBJECT);
        }

        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null) {
            throw new KeyNotFoundException(OBJECT_NOT_FOUND);
        }
        
        await _userManager.DeleteAsync(user);
    }
}