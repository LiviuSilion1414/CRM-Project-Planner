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

    public async Task<List<WorkOrderViewDto>> GetPaginatedAsync(int limit = 0, int offset = 5) {
        try {
            return await _http
                .GetFromJsonAsync<List<WorkOrderViewDto>>($"api/calculator/get/paginated/{limit}/{offset}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    } 

    public async Task<WorkOrderCostDto> GetInvoiceAsync(int workOrderId) {
        try {
            return await _http
                .GetFromJsonAsync<WorkOrderCostDto>($"api/calculator/get/invoice/{workOrderId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<HttpResponseMessage> AddInvoiceAsync(int workOrderId) {
        try {
            return await _http
                .GetFromJsonAsync<HttpResponseMessage>($"api/calculator/generate/{workOrderId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<ClientViewDto> GetClientForViewAsync(int clientId) {
        try {
            return await _http
                .GetFromJsonAsync<ClientViewDto>($"api/client/get/for/view/{clientId}");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<int> GetCollectionSize() {
        try {
            return await _http
                .GetFromJsonAsync<int>("api/calculator/get/size");
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
}