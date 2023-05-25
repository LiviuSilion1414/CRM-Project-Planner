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
        
        var isNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        if (isNull) {
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
            UserName = dto.Email
        };
        
        var foundUserRole = await _roleManager.Roles
            .FirstAsync(aspRole => Enum.GetNames(typeof(Roles))
                .Any(role => role == aspRole.Name));

        var userRole = new IdentityRole {
            Name = foundUserRole.Name,
            NormalizedName = foundUserRole.Name.ToUpper()
        };

        var creationResult = await _userManager.CreateAsync(user, dto.Password);
        var assignmentResult = await _userManager.AddToRoleAsync(user, userRole.NormalizedName);
        
        if (!creationResult.Succeeded || !assignmentResult.Succeeded) {
            throw new InvalidOperationException(IMPOSSIBILE_GOING_FORWARD);
        }
    }

    public async Task EditAsync(EmployeeEditFormDto dto, string oldEmail) {
        if (dto.GetType() == null) {
            throw new NullReferenceException(NULL_OBJECT);
        } 
        
        var isNull = dto.GetType().GetProperties()
            .Any(em => em.GetValue(dto) == null);
        
        if (isNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }

        if (string.IsNullOrEmpty(oldEmail)) {
            throw new NullReferenceException(NULL_OBJECT);
        }

        var user = await _userManager.FindByEmailAsync(oldEmail);
        
        if (user == null) {
            throw new KeyNotFoundException(OBJECT_NOT_FOUND);
        } 

        user.Email = dto.Email;
        user.EmailConfirmed = true;
        user.UserName = dto.Email;

        var passChangeResult = await _userManager.RemovePasswordAsync(user);
        var updateResult = await _userManager.AddPasswordAsync(user, dto.Password);

        if (!passChangeResult.Succeeded || !updateResult.Succeeded) {
            throw new InvalidOperationException(IMPOSSIBILE_GOING_FORWARD);
        } 
        
        var currentUser = await _userManager.FindByNameAsync(oldEmail);
        var rolesList = await _userManager.GetRolesAsync(currentUser);
        var userRole = rolesList.Single();

        var isInRole = await _userManager.IsInRoleAsync(user, userRole);
        
        if (isInRole) {
            await _userManager.RemoveFromRoleAsync(user, userRole);
            await _userManager.AddToRoleAsync(user, dto.Role.ToString());
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