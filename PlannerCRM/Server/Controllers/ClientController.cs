namespace PlannerCRM.Server.Controllers;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase 
{
    private readonly ClientRepository _repo;
    private readonly ILogger<ClientRepository> _logger;

    public ClientController(ClientRepository repo, Logger<ClientRepository> logger) {
        _repo = repo;
        _logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddUser(ClientFormDto dto) {
        try {
            await _repo.AddClientAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_ADD);
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [HttpPut("edit")]
    public async Task<IActionResult> EditUser(ClientFormDto dto) {
        try {
            await _repo.EditClientAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_EDIT);
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (KeyNotFoundException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [HttpDelete("delete/{clientId}")]
    public async Task<IActionResult> DeleteUser(int clientId) {
        try {
            await _repo.DeleteClientAsync(clientId);

            return Ok(SuccessfulCrudFeedBack.USER_DELETE);
        } catch (InvalidOperationException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [HttpGet("get/for/view/{clientId}")]
    public async Task<ClientViewDto> GetForViewById(int clientId) {
        try {
            return await _repo.GetClientForViewAsync(clientId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<ClientViewDto>> GetPaginatedClients(int limit, int offset) {
        try {
            return await _repo.GetPaginatedAsync(limit, offset);
        } catch (Exception exc) {
             _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
}