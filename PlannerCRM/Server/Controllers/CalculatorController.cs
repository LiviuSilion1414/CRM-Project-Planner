namespace PlannerCRM.Server.Controllers;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly CalculatorService _calculator;
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorController(
        CalculatorService calculator, 
        ILogger<CalculatorService> logger) 
    {
        _calculator = calculator;
        _logger = logger;
    }    

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrderCostsAsync(int limit, int offset) {        
        return await _calculator.GetPaginatedWorkOrdersCostsAsync(limit, offset);
    } 

    [HttpGet("generate/{workOrderId}")]
    public async Task<IActionResult> AddInvoiceAsync(int workOrderId) {
        await _calculator.AddInvoiceAsync(workOrderId);

        return Ok(SuccessfulCrudFeedBack.REPORT_CREATED);
    }

    [HttpGet("get/invoice/{workOrderId}")]
    public async Task<WorkOrderCostDto> GetInvoiceAsync(int workOrderId) {
        return await _calculator.GetWorkOrderCostForViewByIdAsync(workOrderId); 
    }

    [HttpGet("get/size")]
    public async Task<int> GetCollectionSizeAsync() {
        return await _calculator.GetCollectionSizeAsync();
    }
}