namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly AppDbContext _dbCcontext;

    public AccountController(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager,
        AppDbContext dbContext) 
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbCcontext = dbContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(EmployeeLoginDto dto) {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null) {
            return NotFound(LoginFeedBack.USER_NOT_FOUND);
        }

        if (dto.Email != ConstantValues.ADMIN_EMAIL) {
            var employee = await _dbCcontext.Employees.SingleOrDefaultAsync(em => em.Email == dto.Email);
            
            if (employee is null || employee.IsArchived || employee.IsDeleted) {
                return NotFound(LoginFeedBack.USER_NOT_FOUND);
            }
        }

        var userPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!userPasswordIsCorrect) {
            return BadRequest(LoginFeedBack.WRONG_PASSWORD);
        }
        
        await _signInManager.SignInAsync(user, false);

        return Ok(LoginFeedBack.CONNECTED);
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task LogoutAsync() => 
        await _signInManager.SignOutAsync();
    
    [HttpGet("user/role")]
    public async Task<string> GetUserRoleAsync() {
        if (User.Identity.IsAuthenticated) {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.SingleOrDefault();
        } else {
            return string.Empty;
        }
    }

    private async Task<string> GetCurrentUserIdAsync() {
        var user = await _userManager.FindByEmailAsync(User.Identity.Name);
        var employee = await _dbCcontext.Employees.SingleOrDefaultAsync(em => em.Username == User.Identity.Name);

        return user is not null && employee is not null ? employee.Id : string.Empty;
    }

    [HttpGet("current/user/info")]
    public async Task<CurrentUser> GetCurrentUserInfo() {
        if (User.Identity.IsAuthenticated) {
            return new CurrentUser {
                Id = await GetCurrentUserIdAsync(),
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Role = await GetUserRoleAsync(),
                Claims = User.Claims
                    .ToDictionary(c => c.Type, c => c.Value)
            };
        }

        return new();
    }
}