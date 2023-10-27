namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager) 
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(EmployeeLoginDto dto) {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null) return NotFound(LoginFeedBack.USER_NOT_FOUND);
        
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

            return roles.Single();
        } else {
            return string.Empty;
        }
    }

    [HttpGet("current/user/info")]
    public async Task<CurrentUser> GetCurrentUserInfo() {
        return new CurrentUser {
            IsAuthenticated = User.Identity.IsAuthenticated,
            UserName = User.Identity.Name,
            Role = await GetUserRoleAsync(),
            Claims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value)
        };
    }
}