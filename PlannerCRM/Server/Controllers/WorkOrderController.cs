namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkOrderController : ControllerBase
{
    private readonly WorkOrderRepository _repo;
    private readonly ILogger<WorkOrderRepository> _logger;

    public WorkOrderController(
        WorkOrderRepository repo, 
        Logger<WorkOrderRepository> logger) 
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddWorkOrderAsync(WorkOrderFormDto dto) {
        await _repo.AddAsync(dto);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditWorkOrderAsync(WorkOrderFormDto dto) {
        await _repo.EditAsync(dto);
        return Ok();
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{workOrderId}")]
    public async Task<IActionResult> DeleteWorkOrderAsync(int workOrderId) {
        await _repo.DeleteAsync(workOrderId);
        return Ok();
    }

    [HttpGet("search/{workOrder}")]
    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrderName) {
        return await _repo.SearchWorkOrderAsync(workOrderName);
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{workOrderId}")]
    public async Task<WorkOrderFormDto> GetWorkOrderForEditByIdAsync(int workOrderId) {
        return await _repo.GetForEditByIdAsync(workOrderId);
    }

    [HttpGet("get/for/view/{workOrderId}")]
    public async Task<WorkOrderViewDto> GetWorkOrderForViewByIdAsync(int workOrderId) {
        return await _repo.GetForViewByIdAsync(workOrderId);
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{workOrderId}")]
    public async Task<WorkOrderDeleteDto> GetWorkOrderForDeleteByIdAsync(int workOrderId) {
        return await _repo.GetForDeleteByIdAsync(workOrderId);
    }

    [HttpGet("get/size")] 
    public async Task<int> GetSizeAsync() {
        return await _repo.GetWorkOrdersSizeAsync();
    }

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersAsync(int limit = 0, int offset = 5) {
        return await _repo.GetPaginatedWorkOrdersAsync(limit, offset);
    }
}