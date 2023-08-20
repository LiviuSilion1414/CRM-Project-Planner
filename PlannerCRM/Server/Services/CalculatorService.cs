using PlannerCRM.Shared.DTOs.CostDto;

namespace PlannerCRM.Server.Services;

public class CalculatorService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorService(AppDbContext dbContext, ILogger<CalculatorService> logger) {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<int> GetCollectionSizeAsync(int id) {
        throw new NotImplementedException();
    }

    public async Task<decimal> IssueInvoiceAsync(int workOrderId) {
        throw new NotImplementedException();
    }

    public async Task<List<WorkOrderCostDto>> GetPaginatedWorkOrdersCostsAsync(int limit, int offset) {
        throw new NotImplementedException();
    } 

    private async Task<int> CalculateTotalHoursAsync(int workOrderId) {
        throw new NotImplementedException();
    }

    private async Task<int> GetRelatedEmployeesSizeAsync(int workOrderId) {
        throw new NotImplementedException();
    }

    private async Task<int> GetRelatedActivitiesSizeAsync(int workOrderId) {
        throw new NotImplementedException();
    }

    private async Task<List<EmployeeViewDto>> GetAllRelatedEmployeesAsync(int workOrderId) {
        throw new NotImplementedException();
    }

    private async Task<List<ActivityViewDto>> GetAllRelatedActivitiesAsync(int workOrderId) {
        throw new NotImplementedException();
    }

    private async Task<List<ActivityCostDto>> CalculateMonthlyCostAsync(int workOrderId) {
        throw new NotImplementedException();
    }
}