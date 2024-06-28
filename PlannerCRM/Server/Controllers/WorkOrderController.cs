namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkOrderController(IRepository<WorkOrderFormDto> repo, IWorkOrderRepository workOrderRepository) : ControllerBase
{
    private readonly IRepository<WorkOrderFormDto> _repo = repo;
    private readonly IWorkOrderRepository _workOrderRepository = workOrderRepository;

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

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{workOrderId}")]
    public async Task<WorkOrderFormDto> GetWorkOrderForEditByIdAsync(int workOrderId) =>
        await _workOrderRepository.GetForEditByIdAsync(workOrderId);

    [HttpGet("get/for/view/{workOrderId}")]
    public async Task<WorkOrderViewDto> GetWorkOrderForViewByIdAsync(int workOrderId) =>
        await _workOrderRepository.GetForViewByIdAsync(workOrderId);

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{workOrderId}")]
    public async Task<WorkOrderDeleteDto> GetWorkOrderForDeleteByIdAsync(int workOrderId) =>
        await _workOrderRepository.GetForDeleteByIdAsync(workOrderId);
    
    [HttpGet("search/{workOrder}")]
    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrderName) =>
        await _workOrderRepository.SearchWorkOrderAsync(workOrderName);

    [HttpGet("get/size")] 
    public async Task<int> GetSizeAsync() =>
        await _workOrderRepository.GetWorkOrdersSizeAsync();

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersAsync(int limit = 0, int offset = 5) =>
        await _workOrderRepository.GetPaginatedWorkOrdersAsync(limit, offset);
}