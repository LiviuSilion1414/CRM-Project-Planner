namespace PlannerCRM.Server.Services.Interfaces;

public interface ICalculateService
{
   Task<decimal> CalculateOrderCostMonthly(int workOrderId, DateTime date);
   Task<decimal> CalculateOrderCostYearly(int workOrderId, DateTime date);
   Task<decimal> CalculateOrderCost(int workOrderId);
}