namespace PlannerCRM.Server.Services;

public class ApplicationUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly DtoValidatorService _validator;
    private readonly Logger<DtoValidatorService> _logger;

    public ApplicationUserRepository(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        DtoValidatorService validator,
        Logger<DtoValidatorService> logger) 
    {
        _userManager = userManager;    
        _roleManager = roleManager;
        _validator = validator;
        _logger = logger;
    }

    public async Task AddAsync(EmployeeFormDto dto) {
        try {
            _validator.ValidateEmployee(dto, OperationType.ADD, out var isValid);

            if (isValid) {
                var user = new IdentityUser {
                    Email = dto.Email,
                    EmailConfirmed = true,
                    UserName = dto.Email,
                };
                
                var foundUserRole = await _roleManager.Roles
                    .SingleAsync(aspRole => aspRole.Name == dto.Role.ToString());
        
                var creationResult = await _userManager.CreateAsync(user, dto.Password);
                if (!creationResult.Succeeded) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
                
                var assignmentResult = await _userManager.AddToRoleAsync(user, foundUserRole.Name);
                
                if (!creationResult.Succeeded || !assignmentResult.Succeeded) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_ADD);
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            throw;
        }
    }

    public async Task EditAsync(EmployeeFormDto dto) {
        try {
            _validator.ValidateEmployee(dto, OperationType.EDIT, out var isValid);
            
            if (isValid) {
                var user = await _userManager.FindByEmailAsync(dto.OldEmail);
                
                user.Email = dto.Email;
                user.EmailConfirmed = true;
                user.UserName = dto.Email;

                var passChangeResult = await _userManager.RemovePasswordAsync(user);
                var updateResult = await _userManager.AddPasswordAsync(user, dto.Password);

                if (!passChangeResult.Succeeded || !updateResult.Succeeded) {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
                
                var rolesList = await _userManager.GetRolesAsync(user);
                var userRole = rolesList.Single();

                var isInRole = await _userManager.IsInRoleAsync(user, userRole);
                
                if (isInRole) {
                    var deleteRoleResult = await _userManager.RemoveFromRoleAsync(user, userRole);
                    var reassignmentRoleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
                    
                    if (!deleteRoleResult.Succeeded || !reassignmentRoleResult.Succeeded) {
                        throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                    }
                } else {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_EDIT);
                } 
            } else {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_EDIT);
            }
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            throw;
        }
    }
    
    public async Task DeleteAsync(string email) {
        try {
            var user = await _validator.ValidateDeleteUserAsync(email);
    
            await _userManager.DeleteAsync(user);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            throw;
        }
    }
}