namespace PlannerCRM.Server.Interfaces;

public interface IActivityRepository
{
    public Task<ActivityViewDto> GetForViewByIdAsync(int id);
    public Task<ActivityFormDto> GetForEditByIdAsync(int activityId);
    public Task<ActivityDeleteDto> GetForDeleteByIdAsync(int activityId);
    public Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(int workOrderId);
    public Task<List<ActivityViewDto>> GetActivityByEmployeeId(int employeeId, int limit, int offset);
    public Task<int> GetCollectionSizeByEmployeeIdAsync(int employeeId);
}