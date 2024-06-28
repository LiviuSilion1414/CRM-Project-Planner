namespace PlannerCRM.Server.Controllers;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
[ApiController]
[Route("api/[controller]")]
public class CalculatorController(CalculatorService calculator) : ControllerBase
{
    private readonly CalculatorService _calculator = calculator;

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrderCostsAsync(int limit, int offset) =>     
        await _calculator.GetPaginatedWorkOrdersCostsAsync(limit, offset);

    [HttpGet("generate/{workOrderId}")]
    public async Task<IActionResult> AddInvoiceAsync(int workOrderId) {
        await _calculator.AddInvoiceAsync(workOrderId);
        return Ok(SuccessfulCrudFeedBack.REPORT_CREATED);
    }

    [HttpGet("get/invoice/{workOrderId}")]
    public async Task<WorkOrderCostDto> GetInvoiceAsync(int workOrderId) =>
        await _calculator.GetWorkOrderCostForViewByIdAsync(workOrderId); 

    [HttpGet("get/size")]
    public async Task<int> GetCollectionSizeAsync() =>
        await _calculator.GetCollectionSizeAsync();
}