namespace PlannerCRM.Server.Services.Interfaces;

public interface ICalculateService
{
   Task<decimal> CalculateOrderCostMonthlyAsync(int workOrderId, DateTime date);
   Task<decimal> CalculateOrderCostYearlyAsync(int workOrderId, DateTime date);
   Task<decimal> CalculateOrderCostAsync(int workOrderId);
}