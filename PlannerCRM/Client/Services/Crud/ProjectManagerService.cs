namespace PlannerCRM.Client.Services.Crud;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
public class ProjectManagerService
{
    private readonly HttpClient _http;
    private readonly ILogger<ProjectManagerService> _logger;

    public ProjectManagerService(HttpClient http, ILogger<ProjectManagerService> logger) {
        _http = http;
        _logger = logger;       
    }

    public async Task<List<WorkOrderCostDto>> GetPaginatedAsync(int limit, int offset) {
        try {
            return await _http
                .GetFromJsonAsync<List<WorkOrderCostDto>>($"api/calculator/get/paginated/{limit}/{offset}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    } 

    public async Task<Dictionary<WorkOrderInvoiceDto, WorkOrderCostDto>> GetInvoice(int workOrderId) {
        try {
            return await _http
                .GetFromJsonAsync<Dictionary<WorkOrderInvoiceDto, WorkOrderCostDto>>($"api/calculator/get/invoice/{workOrderId}");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<int> GetCollectionSize() {
        try {
            return await _http
                .GetFromJsonAsync<int>("api/calculator/get/size");
        } catch (Exception exc) {
            _logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);

            return new();
        }
    }
}