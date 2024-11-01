namespace PlannerCRM.Server.Mappers;

public static class WorkOrderCostMapper
{
    public static WorkOrderCostDto MapToWorkOrderCostDto(this WorkOrderCost workOrderCost)
    {
        return new()
        {
            Id = workOrderCost.Id,
            Name = workOrderCost.Name,
            StartDate = workOrderCost.StartDate,
            FinishDate = workOrderCost.FinishDate,
            TotalHours = workOrderCost.TotalHours,
            TotalEmployees = workOrderCost.TotalEmployees,
            TotalActivities = workOrderCost.TotalActivities,
            TotalCost = workOrderCost.TotalCost,
            CostPerMonth = workOrderCost.CostPerMonth,
            TotalTime = workOrderCost.TotalTime,
            IsInvoiceCreated = workOrderCost.IsCreated,
            Employees = [],
            Activities = [],
            MonthlyActivityCosts = [],
            WorkOrder = new(),
            Client = new()
        };
    }
}
