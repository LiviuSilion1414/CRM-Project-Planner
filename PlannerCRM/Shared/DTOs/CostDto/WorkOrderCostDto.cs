namespace PlannerCRM.Shared.DTOs.CostDto;

public class WorkOrderCostDto 
{
    public int Id { get; init; }
    public int WorkOrderId { get; init; }
    public int TotalHours { get; init; }
    public int TotalEmployees { get; init; }
    public int TotalActivities { get; init; }
    public decimal CostPerMonth { get; init; }
    public decimal TotalCost { get; init; }
    public List<ActivityViewDto> Activities { get; init; }
    public List<EmployeeViewDto> Employees { get; init; }
    public List<ActivityCostDto> MonthlyActivityCosts { get; init; }
}