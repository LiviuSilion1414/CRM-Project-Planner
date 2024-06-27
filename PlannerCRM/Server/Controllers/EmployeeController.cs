namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _repo;
    private readonly ILogger<EmployeeRepository> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public EmployeeController(
        EmployeeRepository repo, 
        Logger<EmployeeRepository> logger,
        UserManager<IdentityUser> userManager) 
    {
        _repo = repo;
        _logger = logger;
        _userManager = userManager;
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddEmployeeAsync(EmployeeFormDto dto) {

        await _repo.AddAsync(dto);
        return Ok(SuccessfulCrudFeedBack.USER_ADD);
    }

    [Authorize]
    [HttpPut("edit")]
    public async Task<IActionResult> EditEmployeeAsync(EmployeeFormDto dto) {
        return Ok(SuccessfulCrudFeedBack.USER_EDIT);
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpDelete("delete/{employeeId}")]
    public async Task<IActionResult> DeleteEmployeeAsync(string employeeId) {
        await _repo.DeleteAsync(employeeId);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("archive/{employeeId}")]
    public async Task<IActionResult> ArchiveEmployeeAsync(string employeeId) {
        await _repo.ArchiveAsync(employeeId);
        return Ok();
    }
    
    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("restore/{employeeId}")]
    public async Task<IActionResult> RestoreEmployeeAsync(string employeeId) {
        await _repo.RestoreAsync(employeeId);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/restore/{employeeId}")]
    public async Task<EmployeeSelectDto> GetEmployeeForRestoreByIdAsync(string employeeId) {
        return await _repo.GetForRestoreAsync(employeeId);
    }

    [HttpGet("get/for/view/{employeeId}")]
    public async Task<EmployeeViewDto> GetEmployeeForViewByIdAsync(string employeeId) {
        return await _repo.GetForViewByIdAsync(employeeId);
    }

    [HttpGet("get/for/edit/{employeeId}")] 
    public async Task<EmployeeFormDto> GetEmployeeForEditByIdAsync(string employeeId)  {
        return await _repo.GetForEditByIdAsync(employeeId);
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/delete/{employeeId}")]
    public async Task<EmployeeDeleteDto> GetEmployeeForDeleteByIdAsync(string employeeId) {
        return await _repo.GetForDeleteByIdAsync(employeeId);
    }

    [HttpGet("search/{email}")]
    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email) {
        return await _repo.SearchEmployeeAsync(email);
    }

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int limit, int offset) {
        return await _repo.GetPaginatedEmployeesAsync(limit, offset);
    }

    [HttpGet("get/id/{email}")]
    public async Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email) {
        return await _repo.GetEmployeeIdAsync(email);
    }

    [HttpGet("get/id-check/{email}")]
    public async Task<string> GetUserIdCheckAsync(string email) {
        return (await _repo.GetEmployeeIdAsync(email)).Id;
    }

    [HttpGet("get/size")] 
    public async Task<int> GetEmployeesSizeAsync() {
        return await _repo.GetEmployeesSizeAsync();
    }
}