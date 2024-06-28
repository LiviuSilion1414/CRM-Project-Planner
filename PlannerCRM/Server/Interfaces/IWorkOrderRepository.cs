namespace PlannerCRM.Server.Interfaces;

public interface IWorkOrderRepository
{
    public Task<WorkOrderViewDto> GetForViewByIdAsync(int id);
    public Task<WorkOrderFormDto> GetForEditByIdAsync(int id);
    public Task<WorkOrderDeleteDto> GetForDeleteByIdAsync(int id);
    public Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrderName);
    public Task<int> GetWorkOrdersSizeAsync();
    public Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersAsync(int limit = 0, int offset = 5);
}