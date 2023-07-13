namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ActivityController : ControllerBase
{
    private readonly ActivityRepository _repo;
    private readonly Logger<ActivityRepository> _logger;

    public ActivityController(
        ActivityRepository repo,
        Logger<ActivityRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<ActionResult> AddActivity(ActivityFormDto dto) 
    {
        try
        {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.ACTIVITY_ADD);
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error,nullRefExc.Message, nullRefExc.StackTrace);
            return BadRequest(nullRefExc.Message);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error, argNullExc, argNullExc.Message, argNullExc.StackTrace);
            return BadRequest(argNullExc.Message);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.Log(LogLevel.Error,duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return BadRequest(duplicateElemExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.Log(LogLevel.Error,dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<ActionResult> EditActivity(ActivityFormDto dto)
    {
        try
        {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.ACTIVITY_EDIT);
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error,nullRefExc.Message, nullRefExc.StackTrace);
            return NotFound(nullRefExc.Message);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error,argNullExc.Message, argNullExc.StackTrace);
            return BadRequest(argNullExc.Message);
        }
        catch (KeyNotFoundException keyNotFoundExc)
        {
            _logger.Log(LogLevel.Error,keyNotFoundExc.Message, keyNotFoundExc.StackTrace);
            return NotFound(keyNotFoundExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.Log(LogLevel.Error,dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return BadRequest(exc.Message);
        }
    }
    
    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{activityId}")]
    public async Task<ActionResult> DeleteActivity(int activityId)
    {
        try
        {
            await _repo.DeleteAsync(activityId);

            return Ok(SuccessfulCrudFeedBack.ACTIVITY_DELETE);
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.Log(LogLevel.Error,invalidOpExc.Message, invalidOpExc.StackTrace);
            return BadRequest(invalidOpExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.Log(LogLevel.Error,dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize]
    [HttpGet("get/{activityId}")]
    public async Task<ActivityViewDto> GetForView(int activityId)
    {
        try {
            return await _repo.GetForViewAsync(activityId);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return new ActivityViewDto();
        }
    }

    [Authorize]
    [HttpGet("get/for/edit/{activityId}")]
    public async Task<ActivityFormDto> GetForEdit(int activityId)
    {
        try
        {
            return await _repo.GetForEditAsync(activityId);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return new ActivityFormDto();
        }
    }

    [Authorize]
    [HttpGet("get/for/delete/{activityId}")]
    public async Task<ActivityDeleteDto> GetForDelete(int activityId)
    {
        try
        {
            return await _repo.GetForDeleteAsync(activityId);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return new ActivityDeleteDto();
        }
    }

    [Authorize]
    [HttpGet("get/activity/per/workorder/{workOrderId}")]
    public async Task<List<ActivityFormDto>> GetActivitiesPerWorkorderAsync(int workOrderId)
    {
        try
        {
            return await _repo.GetActivitiesPerWorkOrderAsync(workOrderId);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return new List<ActivityFormDto>();
        }
    }

    [Authorize]
    [HttpGet("get/activity/per/employee/{employeeId}")]
    public async Task<List<ActivityFormDto>> GetActivitiesPerEmployee(int employeeId)
    {
        try
        {
            return await _repo.GetActivityByEmployeeId(employeeId);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return new List<ActivityFormDto>();
        }
    }

    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<ActivityViewDto>> GetAll()
    {
        try
        {
            return await _repo.GetAllAsync();
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return new List<ActivityViewDto>();
        }
    }
}