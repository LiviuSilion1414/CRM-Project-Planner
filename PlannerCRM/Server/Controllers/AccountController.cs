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
    public async Task<ActionResult> Login(EmployeeLoginDto dto) {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null) return NotFound(LoginFeedBack.USER_NOT_FOUND);
        
        var userPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!userPasswordIsCorrect) {
            return BadRequest(LoginFeedBack.WRONG_PASSWORD);
        } else {
            await _signInManager.SignInAsync(user, false);

            return Ok(LoginFeedBack.CONNECTED);
        }
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task Logout() => await _signInManager.SignOutAsync();
    

    [HttpGet("user/role")]
    public async Task<string> GetUserRole() {
        if (User.Identity.IsAuthenticated) {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.Single();
        } else {
            return null;
        }
    }

    [HttpGet("current/user/info")]
    public CurrentUser CurrentUserInfo() {
        return new CurrentUser {
            IsAuthenticated = User.Identity.IsAuthenticated,
            UserName = User.Identity.Name,
            Claims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value)
        };
    }
}