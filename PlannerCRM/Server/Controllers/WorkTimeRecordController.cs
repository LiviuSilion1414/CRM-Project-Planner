namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WorkTimeRecordController : ControllerBase
{
    private readonly WorkTimeRecordRepository _repo;
    private readonly Logger<WorkTimeRecordRepository> _logger;

    public WorkTimeRecordController(WorkTimeRecordRepository repo, Logger<WorkTimeRecordRepository> logger) {
        _repo = repo;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<ActionResult> AddWorkTimeRecord(WorkTimeRecordFormDto dto) {
        try {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKTIMERECORD_ADD);
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return NotFound(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, exc.Message);
        }
    }
    
    [Authorize]
    [HttpPut("edit")]
    public async Task<ActionResult> EditWorkTimeRecord(WorkTimeRecordFormDto dto) {
        try {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKTIMERECORD_EDIT);
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (KeyNotFoundException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return NotFound(exc.Message);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return BadRequest(exc.Message);
        } catch (Exception exc) {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, exc.Message);
        }
    }
    
    [Authorize]
    [HttpGet("get/{workOrderId}/{activityId}/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetWorkTimeRecord(int workOrderId, int activityId, int employeeId) {
        try {
            return await _repo.GetAsync(workOrderId, activityId, employeeId);
        } catch (KeyNotFoundException exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }
    
    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<WorkTimeRecordViewDto>> GetAllWorkTimeRecords() {
        try {
            return await _repo.GetAllAsync();
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }
    
    [Authorize]
    [HttpGet("get/by/employee/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetAllWorkTimeRecordsByWorkOrder(int employeeId) {
        try {
            return await _repo.GetByEmployeeIdAsync(employeeId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }
}
