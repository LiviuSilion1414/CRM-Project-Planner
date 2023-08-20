
namespace PlannerCRM.Client.Services;

public class ProjectManagerService
{
    private readonly HttpClient _http;

    public ProjectManagerService(HttpClient http) {
        _http = http;
    }

    public async Task<int> GetCollectionSize() {
        return await _http.GetFromJsonAsync<int>("api/calculator/get/size");
    }

    public async Task<List<WorkOrderCostDto>> GetPaginatedAsync(int limit = 5, int offset = 0) {
        return await _http.GetFromJsonAsync<List<WorkOrderCostDto>>($"api/calculator/get/paginated/{limit}/{offset}");
    }

    public async Task<WorkOrderCostDto> GetInvoiceAsync(int workOrderId) {
        return await _http.GetFromJsonAsync<WorkOrderCostDto>($"api/calculator/get/invoice/{workOrderId}");
    }
}