namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController(
    IRepository<EmployeeFormDto> repo,
    IEmployeeRepository employeeRepository) : ControllerBase
{
    private readonly IRepository<EmployeeFormDto> _repo = repo;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

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
        await _employeeRepository.ArchiveAsync(employeeId);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("restore/{employeeId}")]
    public async Task<IActionResult> RestoreEmployeeAsync(int employeeId)
    {
        await _employeeRepository.RestoreAsync(employeeId);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/restore/{employeeId}")]
    public async Task<EmployeeSelectDto> GetEmployeeForRestoreByIdAsync(int employeeId) =>
        await _employeeRepository.GetForRestoreAsync(employeeId);

    [HttpGet("get/for/view/{employeeId}")]
    public async Task<EmployeeViewDto> GetEmployeeForViewByIdAsync(int employeeId) =>
        await _employeeRepository.GetForViewByIdAsync(employeeId);

    [HttpGet("get/for/edit/{employeeId}")]
    public async Task<EmployeeFormDto> GetEmployeeForEditByIdAsync(int employeeId) =>
        await _employeeRepository.GetForEditByIdAsync(employeeId);

    [HttpGet("get/employee/salaries/by/employeeid/{employeeId}")]
    public async Task<List<EmployeeSalaryDto>> GetEmployeeSalariesByIdAsync(int employeeId) =>
        await _employeeRepository.GetEmployeeSalariesAsync(employeeId);

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/delete/{employeeId}")]
    public async Task<EmployeeDeleteDto> GetEmployeeForDeleteByIdAsync(int employeeId) =>
        await _employeeRepository.GetForDeleteByIdAsync(employeeId);

    [HttpGet("search/{email}")]
    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email) =>
        await _employeeRepository.SearchEmployeeAsync(email);

    [HttpGet("get/paginated/{offset}/{limit}")]
    public async Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int offset, int limit) =>
        await _employeeRepository.GetPaginatedEmployeesAsync(offset, limit);

    [HttpGet("get/id/{email}")]
    public async Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email) =>
        await _employeeRepository.GetEmployeeIdAsync(email);

    [HttpGet("get/id-check/{email}")]
    public async Task<int> GetUserIdCheckAsync(string email) =>
        (await _employeeRepository.GetEmployeeIdAsync(email)).Id;

    [HttpGet("get/size")]
    public async Task<int> GetEmployeesSizeAsync() =>
        await _employeeRepository.GetEmployeesSizeAsync();
}