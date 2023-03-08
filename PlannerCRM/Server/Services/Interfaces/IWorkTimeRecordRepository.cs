using PlannerCRM.Server.Services.Interfaces;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Services.Interfaces;

public interface IWorkTimeRecordRepository: IRepository<WorkTimeRecord>
{
    Task<List<WorkTimeRecord>> GetAllAsync(int workOrderId);
} 
