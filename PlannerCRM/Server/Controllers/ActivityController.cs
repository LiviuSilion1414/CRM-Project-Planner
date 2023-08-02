namespace PlannerCRM.Server.Controllers;

[Authorize(Roles = $"""
    {nameof(Roles.OPERATION_MANAGER)}, 
    {nameof(Roles.SENIOR_DEVELOPER)}, 
    {nameof(Roles.JUNIOR_DEVELOPER)}
""" )]
[ApiController]
[Route("[controller]")]
public class ActivityController : ControllerBase
{
    private readonly ActivityRepository _repo;
    private readonly Logger<ActivityRepository> _logger;

    public ActivityController(ActivityRepository repo, Logger<ActivityRepository> logger) {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<ActionResult> AddActivity(ActivityFormDto dto) {
        try {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.ACTIVITY_ADD);
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

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<ActionResult> EditActivity(ActivityFormDto dto) {
        try {
            if (await _repo.EditAsync(dto)) {
                return Ok(SuccessfulCrudFeedBack.ACTIVITY_EDIT);
            }

            return BadRequest(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
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

            return BadRequest(exc.Message);
        }
    }
    
    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{activityId}")]
    public async Task<ActionResult> DeleteActivity(int activityId) {
        try {
            await _repo.DeleteAsync(activityId);

            return Ok(SuccessfulCrudFeedBack.ACTIVITY_DELETE);
        } catch (InvalidOperationException exc) {
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

    [Authorize(Roles = $"""
        {nameof(Roles.OPERATION_MANAGER)}, 
        {nameof(Roles.SENIOR_DEVELOPER)}, 
        {nameof(Roles.JUNIOR_DEVELOPER)}
    """ )]
    [HttpGet("get/{activityId}")]
    public async Task<ActivityViewDto> GetForView(int activityId) {
        try {
            return await _repo.GetForViewAsync(activityId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{activityId}")]
    public async Task<ActivityFormDto> GetForEdit(int activityId) {
        try {
            return await _repo.GetForEditAsync(activityId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{activityId}")]
    public async Task<ActivityDeleteDto> GetForDelete(int activityId) {
        try {
            return await _repo.GetForDeleteAsync(activityId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = $"""
        {nameof(Roles.OPERATION_MANAGER)}, 
        {nameof(Roles.SENIOR_DEVELOPER)}, 
        {nameof(Roles.JUNIOR_DEVELOPER)}
    """ )]
    [HttpGet("get/activity/per/workorder/{workOrderId}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkorderAsync(int workOrderId) {
        try {
            return await _repo.GetActivitiesPerWorkOrderAsync(workOrderId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = $"""
        {nameof(Roles.OPERATION_MANAGER)}, 
        {nameof(Roles.SENIOR_DEVELOPER)}, 
        {nameof(Roles.JUNIOR_DEVELOPER)}
    """ )]
    [HttpGet("get/activity/per/employee/{employeeId}")]
    public async Task<List<ActivityFormDto>> GetActivitiesPerEmployee(int employeeId) {
        try {
            return await _repo.GetActivityByEmployeeId(employeeId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }
    
    [Authorize(Roles = $"""
        {nameof(Roles.OPERATION_MANAGER)}, 
        {nameof(Roles.SENIOR_DEVELOPER)}, 
        {nameof(Roles.JUNIOR_DEVELOPER)}
    """ )]
    [HttpGet("get/all")]
    public async Task<List<ActivityViewDto>> GetAll() {
        try {
            return await _repo.GetAllAsync();
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

            return new();
        }
    }
}