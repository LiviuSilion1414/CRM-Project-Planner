namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _repo;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeController(EmployeeRepository repo, Logger<EmployeeRepository> logger) {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddUser(EmployeeFormDto dto) {
        try {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_ADD);
        } catch (NullReferenceException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DuplicateElementException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditUser(EmployeeFormDto dto) {
        try {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_EDIT);
        } catch (NullReferenceException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (KeyNotFoundException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpDelete("delete/{employeeId}")]
    public async Task<IActionResult> DeleteUser(int employeeId) {
        try {
            await _repo.DeleteAsync(employeeId);

            return Ok(SuccessfulCrudFeedBack.USER_DELETE);
        } catch (InvalidOperationException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("archive/{employeeId}")]
    public async Task<IActionResult> ArchiveUser(int employeeId) {
        try {
            await _repo.ArchiveAsync(employeeId);

            return Ok(SuccessfulCrudFeedBack.USER_ARCHIVE);
        } catch (InvalidOperationException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
    
    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("restore/{employeeId}")]
    public async Task<IActionResult> RestoreUser(int employeeId) {
        try {
            await _repo.RestoreAsync(employeeId);

            return Ok(SuccessfulCrudFeedBack.USER_ARCHIVE);
        } catch (InvalidOperationException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize]
    [HttpGet("get/for/restore/{employeeId}")]
    public async Task<EmployeeSelectDto> GetForRestoreById(int employeeId) {
        try {
            return await _repo.GetForRestoreAsync(employeeId);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("get/for/view/{employeeId}")]
    public async Task<EmployeeViewDto> GetForViewById(int employeeId) {
        try {
            return await _repo.GetForViewAsync(employeeId);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/edit/{employeeId}")]
    public async Task<EmployeeFormDto> GetForEditById(int employeeId) {
        try {
            return await _repo.GetForEditAsync(employeeId);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/delete/{employeeId}")]
    public async Task<EmployeeDeleteDto> GetForDeleteById(int employeeId) {
        try {
            return await _repo.GetForDeleteAsync(employeeId);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("search/{email}")]
    public async Task<List<EmployeeSelectDto>> SearchEmployee(string email) {
        try {
            return await _repo.SearchEmployeeAsync(email);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<EmployeeViewDto>> GetPaginatedEmployees(int limit, int offset) {
        try {
            return await _repo.GetPaginatedEmployees(limit, offset);
        } catch (Exception exc) {
             _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("get/id/{email}")]
    public async Task<CurrentEmployeeDto> GetUserId(string email) {
        try {
            return await _repo.GetUserIdAsync(email);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("get/id-check/{email}")]
    public async Task<int> GetUserIdCheck(string email) {
        try {
            var currentEmployee = await _repo.GetUserIdAsync(email);
            return currentEmployee.Id;
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return default;
        }
    }

    [Authorize]
    [HttpGet("get/size")] 
    public async Task<int> GetEmployeesSize() {
        try {
            return await _repo.GetEmployeesSize();
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return default;
        }
    }
}