namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController(
    EmployeeRepository repo,
    Logger<EmployeeRepository> logger,
    UserManager<IdentityUser> userManager) : ControllerBase
{
    private readonly EmployeeRepository _repo = repo;
    private readonly ILogger<EmployeeRepository> _logger = logger;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddEmployeeAsync(EmployeeFormDto dto)
    {
        await _repo.AddAsync(dto);
        return Ok(SuccessfulCrudFeedBack.USER_ADD);
    }

    [Authorize]
    [HttpPut("edit")]
    public async Task<IActionResult> EditEmployeeAsync(EmployeeFormDto dto)
    {
        await _repo.EditAsync(dto);
        return Ok(SuccessfulCrudFeedBack.USER_EDIT);
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpDelete("delete/{employeeId}")]
    public async Task<IActionResult> DeleteEmployeeAsync(int employeeId)
    {
        await _repo.DeleteAsync(employeeId);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("archive/{employeeId}")]
    public async Task<IActionResult> ArchiveEmployeeAsync(int employeeId)
    {
        await _repo.ArchiveAsync(employeeId);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("restore/{employeeId}")]
    public async Task<IActionResult> RestoreEmployeeAsync(int employeeId)
    {
        await _repo.RestoreAsync(employeeId);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/restore/{employeeId}")]
    public async Task<EmployeeSelectDto> GetEmployeeForRestoreByIdAsync(int employeeId) =>
        await _repo.GetForRestoreAsync(employeeId);

    [HttpGet("get/for/view/{employeeId}")]
    public async Task<EmployeeViewDto> GetEmployeeForViewByIdAsync(int employeeId) =>
        await _repo.GetForViewByIdAsync(employeeId);

    [HttpGet("get/for/edit/{employeeId}")]
    public async Task<EmployeeFormDto> GetEmployeeForEditByIdAsync(int employeeId) =>
        await _repo.GetForEditByIdAsync(employeeId);

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/delete/{employeeId}")]
    public async Task<EmployeeDeleteDto> GetEmployeeForDeleteByIdAsync(int employeeId) =>
        await _repo.GetForDeleteByIdAsync(employeeId);

    [HttpGet("search/{email}")]
    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email) =>
        await _repo.SearchEmployeeAsync(email);

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int limit, int offset) =>
        await _repo.GetPaginatedEmployeesAsync(limit, offset);

    [HttpGet("get/id/{email}")]
    public async Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email) =>
        await _repo.GetEmployeeIdAsync(email);

    [HttpGet("get/id-check/{email}")]
    public async Task<string> GetUserIdCheckAsync(string email) =>
        (await _repo.GetEmployeeIdAsync(email)).Id;

    [HttpGet("get/size")]
    public async Task<int> GetEmployeesSizeAsync() =>
        await _repo.GetEmployeesSizeAsync();
}