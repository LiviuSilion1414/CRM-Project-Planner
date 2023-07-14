namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _repo;
    private readonly Logger<EmployeeRepository> _logger;

    public EmployeeController(EmployeeRepository repo, Logger<EmployeeRepository> logger) {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPost("add")]
    public async Task<ActionResult> AddUser(EmployeeFormDto dto) {
        try {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_ADD);
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPut("edit")]
    public async Task<ActionResult> EditUser(EmployeeFormDto dto) {
        try {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_EDIT);
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return NotFound(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (KeyNotFoundException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return NotFound(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpDelete("delete/{employeeId}")]
    public async Task<ActionResult> DeleteUser(int employeeId) {
        try {
            await _repo.DeleteAsync(employeeId);

            return Ok(SuccessfulCrudFeedBack.USER_DELETE);
        } catch (InvalidOperationException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize]
    [HttpGet("get/for/view/{employeeId}")]
    public async Task<EmployeeViewDto> GetForViewById(int employeeId) {
        try {
            return await _repo.GetForViewAsync(employeeId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/edit/{employeeId}")]
    public async Task<EmployeeFormDto> GetForEditById(int employeeId) {
        try {
            return await _repo.GetForEditAsync(employeeId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/delete/{employeeId}")]
    public async Task<EmployeeDeleteDto> GetForDeleteById(int employeeId) {
        try {
            return await _repo.GetForDeleteAsync(employeeId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new EmployeeDeleteDto();
        }
    }

    [Authorize]
    [HttpGet("search/{email}")]
    public async Task<List<EmployeeSelectDto>> SearchEmployee(string email) {
        try {
            return await _repo.SearchEmployeeAsync(email);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new List<EmployeeSelectDto>();
        }
    }

    [Authorize]
    [HttpGet("get/paginated/{skip}/{take}")]
    public async Task<List<EmployeeViewDto>> GetPaginatedEmployees(int skip = 0, int take = 5) {
        try {
            return await _repo.GetPaginatedEmployees(skip, take);
        } catch (Exception exc) {
             _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new List<EmployeeViewDto>();
        }
    }

    [Authorize]
    [HttpGet("get/id/{email}")]
    public async Task<CurrentEmployeeDto> GetUserId(string email) {
        try {
            return await _repo.GetUserIdAsync(email);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new CurrentEmployeeDto();
        }
    }

    [Authorize]
    [HttpGet("get/id-check/{email}")]
    public async Task<int> GetUserIdCheck(string email) {
        try {
            var currentEmployee = await _repo.GetUserIdAsync(email);
            return currentEmployee.Id;
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return default;
        }
    }

    [Authorize]
    [HttpGet("get/size")] 
    public async Task<int> GetEmployeesSize() {
        try {
            return await _repo.GetEmployeesSize();
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return default;
        }
    }
}