using System.Reflection.Metadata.Ecma335;

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
    public async Task<List<WorkOrderViewDto>> GetPaginatedAsync(int limit, int offset) {
        try {
            return await _calculator.GetPaginatedWorkOrdersCostsAsync(limit, offset);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    } 

    [HttpGet("generate/{workOrderId}")]
    public async Task<ActionResult> AddInvoice(int workOrderId) {
        try {
            await _calculator.AddInvoiceAsync(workOrderId);

            return Ok(SuccessfulCrudFeedBack.REPORT_CREATED);
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return BadRequest();
        }
    }

    [HttpGet("get/invoice/{workOrderId}")]
    public async Task<WorkOrderCostDto> GetInvoice(int workOrderId) {
        try {
            return await _calculator.GetWorkOrderCostForView(workOrderId); 
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/size")]
    public async Task<int> GetCollectionSize() {
        try {
            return await _calculator.GetCollectionSizeAsync();
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
}