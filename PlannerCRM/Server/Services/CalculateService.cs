namespace PlannerCRM.Server.Services;

public class CalculateService
{
   /* public readonly IWorkTimeRecordRepository _records;

    public CalculateService(IWorkTimeRecordRepository records) {
        _records = records;
    }

    public async Task<decimal> CalculateOrderCostAsync(int workOrderId) {
        var records = await _records.GetAllAsync(workOrderId);

        return records
            .Sum(wtr => wtr.Hours * wtr.Employee.Salaries
                .SingleOrDefault(s => s.StartDate <= wtr.Date && wtr.Date <= s.FinishDate).Salary);
    }

    public async Task<decimal> CalculateOrderCostMonthlyAsync(int workOrderId, DateTime date) {
        var recordmonthly = await _records.GetAllAsync(workOrderId);
        
        return recordmonthly
            .Where(wtr => wtr.Date.Month == date.Month)
            .Sum(wtr => wtr.Hours * wtr.Employee.Salaries
                .SingleOrDefault(s => s.StartDate <= wtr.Date && wtr.Date <= s.FinishDate).Salary);

    }
    
    public async Task<decimal> CalculateOrderCostYearlyAsync(int workOrderId, DateTime date) {
        var recordmonthly = await _records.GetAllAsync(workOrderId);

        return recordmonthly
            .Where(wtr => wtr.Date.Year == date.Year)
            .Sum(wtr => wtr.Hours * wtr.Employee.Salaries
                .SingleOrDefault(s => s.StartDate <= wtr.Date && wtr.Date <= s.FinishDate).Salary);

    } */
}