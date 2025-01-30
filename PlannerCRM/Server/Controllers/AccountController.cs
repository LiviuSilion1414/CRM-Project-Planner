namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController
    (
        IMapper mapper, 
        AppDbContext context,
        UserManager<Employee> userManager,
        RoleManager<EmployeeRole> roleManager,
        SignInManager<Employee> signInManager
    ) 
    : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly AppDbContext _context = context;
    private readonly UserManager<Employee> _userManager = userManager;
    private readonly SignInManager<Employee> _signInManager = signInManager;
    private readonly RoleManager<EmployeeRole> _roleManager = roleManager;

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(EmployeeLoginDto dto)
    {
        var model = _mapper.Map<EmployeeLogin>(dto);

        var foundEmployee = await _userManager.FindByEmailAsync(model.Email);

        if (foundEmployee is not null)
        {
            await _signInManager.SignInAsync(foundEmployee, true);

            return Ok(LoginFeedBack.CONNECTED);
        } 
        else
        {
            return NotFound(LoginFeedBack.USER_NOT_FOUND);
        }

    }

    [Authorize]
    [HttpGet]
    [Route("logout")]
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}
