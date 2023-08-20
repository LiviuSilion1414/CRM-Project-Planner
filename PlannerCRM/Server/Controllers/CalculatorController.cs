namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
public class CalculatorController : ControllerBase
{
    private readonly CalculatorService _calculator;
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorController(CalculatorService calculator, ILogger<CalculatorService> logger) {
        _calculator = calculator;
        _logger = logger;
    }    

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkOrderCostDto>> GetPaginatedAsync(int limit, int offset) {
        throw new NotImplementedException();
    } 

    [HttpGet("get/invoice/{workOrderId}")]
    public async Task<WorkOrderCostDto> GetInvoice(int workOrderId) {
        throw new NotImplementedException();
    }

    [HttpGet("get/size")]
    public async Task<int> GetCollectionSize() {
        throw new NotImplementedException();
    }
}