using PlannerCRM.Server.Services.Interfaces;

namespace PlannerCRM.Server.Services.ConcreteClasses;

public class CalculateService : ICalculateService
{
    public readonly IWorkTimeRecordRepository _records;

    public CalculateService(IWorkTimeRecordRepository records) {
        _records = records;
    }

    public async Task<decimal> CalculateOrderCost(int workOrderId) {
        var records = await _records.GetAll(workOrderId);

        return records
            .Sum(wtr => wtr.Hours * wtr.Employee.Salaries
                .SingleOrDefault(s => s.StartDate <= wtr.Date && wtr.Date <= s.FinishDate).Salary);
    }

    public async Task<decimal> CalculateOrderCostMonthly(int workOrderId, DateTime date) {
        var recordmonthly = await _records.GetAll(workOrderId);
        
        return recordmonthly
            .Where(wtr => wtr.Date.Month == date.Month)
            .Sum(wtr => wtr.Hours * wtr.Employee.Salaries
                .SingleOrDefault(s => s.StartDate <= wtr.Date && wtr.Date <= s.FinishDate).Salary);

    }
    
    public async Task<decimal> CalculateOrderCostYearly(int workOrderId, DateTime date) {
        var recordmonthly = await _records.GetAll(workOrderId);

        return recordmonthly
            .Where(wtr => wtr.Date.Year == date.Year)
            .Sum(wtr => wtr.Hours * wtr.Employee.Salaries
                .SingleOrDefault(s => s.StartDate <= wtr.Date && wtr.Date <= s.FinishDate).Salary);

    }
}