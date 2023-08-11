namespace PlannerCRM.Server.Controllers;

[Authorize(Roles = $"""
    {nameof(Roles.OPERATION_MANAGER)}, 
    {nameof(Roles.SENIOR_DEVELOPER)}, 
    {nameof(Roles.JUNIOR_DEVELOPER)},
    {nameof(Roles.ACCOUNT_MANAGER)}
""" )]
[ApiController]
[Route("api/[controller]")]
public class WorkOrderController : ControllerBase
{
    private readonly WorkOrderRepository _repo;
    private readonly ILogger<WorkOrderRepository> _logger;

    public WorkOrderController( WorkOrderRepository repo, Logger<WorkOrderRepository> logger) {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddWorkorder(WorkOrderFormDto dto) {
        try {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKORDER_ADD);
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditWorkorder(WorkOrderFormDto dto) {
        try {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKORDER_EDIT);
        } catch (NullReferenceException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (ArgumentNullException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (DuplicateElementException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{workOrderId}")]
    public async Task<IActionResult> DeleteWorkorder(int workOrderId) {
        try {
            await _repo.DeleteAsync(workOrderId);

            return Ok(SuccessfulCrudFeedBack.WORKORDER_DELETE);
        } catch (InvalidOperationException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (DbUpdateException exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        }
    }

    [Authorize]
    [HttpGet("search/{workOrder}")]
    public async Task<List<WorkOrderSelectDto>> SearchWorkorder(string workOrder) {
        try {
            return await _repo.SearchWorkOrderAsync(workOrder);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{workOrderId}")]
    public async Task<WorkOrderFormDto> GetForEdit(int workOrderId) {
        try {
            return await _repo.GetForEditAsync(workOrderId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("get/for/view/{workOrderId}")]
    public async Task<WorkOrderViewDto> GetForViewId(int workOrderId) {
        try {
            return await _repo.GetForViewAsync(workOrderId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{workOrderId}")]
    public async Task<WorkOrderDeleteDto> GetForDeleteId(int workOrderId) {
        try {
            return await _repo.GetForDeleteAsync(workOrderId);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("get/size")] 
    public async Task<int> GetSize() {
        try
        {
            return await _repo.GetSize();
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return default;
        }
    }

    [Authorize]
    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkOrderViewDto>> GetPaginated(int limit = 0, int offset = 5) {
        try
        {
            return await _repo.GetPaginated(limit, offset);
        } catch (Exception exc) {
             _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
}