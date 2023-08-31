namespace PlannerCRM.Server.Controllers;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
[ApiController]
[Route("api/[controller]")]
public class ApplicationUserController : ControllerBase
{
    private readonly ApplicationUserRepository _repo;
    private readonly ILogger<ApplicationUserRepository> _logger;

    public ApplicationUserController(ApplicationUserRepository repo, Logger<ApplicationUserRepository> logger) {
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

            return BadRequest(exc);
        } catch (InvalidOperationException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DuplicateElementException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {    _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
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
        } catch (InvalidOperationException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpDelete("delete/{email}")]
    public async Task<IActionResult> DeleteUser(string email) {
        try {
            await _repo.DeleteAsync(email);  

            return Ok(SuccessfulCrudFeedBack.USER_DELETE);            
        } catch (NullReferenceException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (KeyNotFoundException exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
}