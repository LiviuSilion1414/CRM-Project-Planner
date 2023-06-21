using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Shared.Constants;

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
        if (dto.GetType() is null)
            throw new NullReferenceException(ExceptionsMessages.NULL_OBJECT);
        
        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) is null);
        if (HasPropertiesNull)
            throw new ArgumentNullException(ExceptionsMessages.NULL_PARAM);

        var person = await _userManager.FindByEmailAsync(dto.Email);
        
        if (person != null)
            throw new InvalidOperationException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);

        if (dto.Role == ConstantValues.ADMIN_ROLE)
            throw new InvalidOperationException(ExceptionsMessages.NOT_ASSEGNABLE_ROLE);

        var isAlreadyPresent = await _userManager.Users
            .SingleOrDefaultAsync(user => user.UserName == dto.Email);
        if (isAlreadyPresent != null)
            throw new DuplicateElementException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);

        var user = new IdentityUser {
            Email = dto.Email,
            EmailConfirmed = true,
            UserName = dto.Email,
        };
        
        var foundUserRole = await _roleManager.Roles
            .SingleAsync(aspRole => aspRole.Name == dto.Role.ToString());

        var creationResult = await _userManager.CreateAsync(user, dto.Password);
        if (!creationResult.Succeeded)
            throw new DbUpdateException();
        
        var assignmentResult = await _userManager.AddToRoleAsync(user, foundUserRole.Name);
        
        if (!creationResult.Succeeded || !assignmentResult.Succeeded)
            throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
    }

    public async Task EditAsync(EmployeeEditFormDto dto) {
        if (dto.GetType() is null)
            throw new NullReferenceException(ExceptionsMessages.NULL_OBJECT);
        
        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(em => em.GetValue(dto) is null);
        
        if (HasPropertiesNull)
            throw new ArgumentNullException(ExceptionsMessages.NULL_PARAM);

        if (dto.Role == ConstantValues.ADMIN_ROLE)
            throw new InvalidOperationException(ExceptionsMessages.NOT_ASSEGNABLE_ROLE);

        var user = await _userManager.FindByEmailAsync(dto.OldEmail);
        
        if (user is null)
            throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);

        user.Email = dto.Email;
        user.EmailConfirmed = true;
        user.UserName = dto.Email;

        var passChangeResult = await _userManager.RemovePasswordAsync(user);
        var updateResult = await _userManager.AddPasswordAsync(user, dto.Password);

        if (!passChangeResult.Succeeded || !updateResult.Succeeded)
            throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
        
        var rolesList = await _userManager.GetRolesAsync(user);
        var userRole = rolesList.Single();

        var isInRole = await _userManager.IsInRoleAsync(user, userRole);
        
        if (isInRole) {
            var deleteRoleResult = await _userManager.RemoveFromRoleAsync(user, userRole);
            var reassignmentRoleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            
            if (!deleteRoleResult.Succeeded || !reassignmentRoleResult.Succeeded) 
                throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
        } else {
            throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_EDIT);
        } 
    }
    
    public async Task DeleteAsync(string email) {
        if (string.IsNullOrEmpty(email))
            throw new NullReferenceException(ExceptionsMessages.NULL_OBJECT);

        var user = await _userManager.FindByEmailAsync(email);
        
        if (user is null)
            throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
        
        await _userManager.DeleteAsync(user);
    }
}