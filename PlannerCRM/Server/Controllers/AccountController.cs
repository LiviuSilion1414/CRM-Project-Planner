using PlannerCRM.Shared.DTOs.EmployeeProfilePictureDto;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(
    UserManager<Employee> userManager,
    SignInManager<Employee> signInManager,
    AppDbContext dbContext) : ControllerBase
{
    private readonly UserManager<Employee> _userManager = userManager;
    private readonly SignInManager<Employee> _signInManager = signInManager;
    private readonly AppDbContext _dbCcontext = dbContext;

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(EmployeeLoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null || user.IsArchived || user.IsDeleted)
        {
            return NotFound(LoginFeedBack.USER_NOT_FOUND);
        }

        var userPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!userPasswordIsCorrect)
        {
            return BadRequest(LoginFeedBack.WRONG_PASSWORD);
        }

        await _signInManager.SignInAsync(user, false);
        return Ok(LoginFeedBack.CONNECTED);
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task LogoutAsync() => await _signInManager.SignOutAsync();

    [HttpGet("user/role")]
    public async Task<string> GetUserRoleAsync()
    {
        if (User is not null && User.Identity.IsAuthenticated)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Single();
        }
        return string.Empty;
    }

    private async Task<int> GetCurrentUserIdAsync() =>
        (await _userManager.FindByEmailAsync(User.Identity.Name)).Id;

    private async Task<ProfilePictureDto> GetCurrentUserProfilePicAsync()
    {
        var profilePic = await _dbCcontext.ProfilePictures
            .SingleAsync(pp => _dbCcontext.Users
                .Any(em => pp.EmployeeInfo.Email == em.Email && em.Email == User.Identity.Name)) ?? new();
        return new ProfilePictureDto()
        {
            ImageType = profilePic.ImageType,
            Thumbnail = profilePic.Thumbnail
        };
    }

    private async Task<string> GetCurrentUserFullName()
    {
        if (User.Identity.IsAuthenticated)
        {
            return (await _userManager.FindByEmailAsync(User.Identity.Name))
                .FullName;
        }
        return string.Empty;
    }

    [HttpGet("current/user/info")]
    public async Task<CurrentUser> GetCurrentUserInfo()
    {
        if (User.Identity.IsAuthenticated)
        {
            return new CurrentUser
            {
                Id = await GetCurrentUserIdAsync(),
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                FullName = await GetCurrentUserFullName(),
                Role = await GetUserRoleAsync(),
                ProfilePicture = await GetCurrentUserProfilePicAsync(),
                Claims = User.Claims
                    .ToDictionary(c => c.Type, c => c.Value)
            };
        }
        return new();
    }
}