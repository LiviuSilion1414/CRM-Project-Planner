namespace PlannerCRM.Shared.DTOs.CostDto;

public class WorkOrderCostDto 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int WorkOrderId { get; set; }
    public DateTime StartDate { get; set; } 
    public DateTime FinishDate { get; set; }
    public int TotalHours { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalActivities { get; set; }
    public decimal CostPerMonth { get; set; }
    public decimal TotalCost { get; set; }
    public TimeSpan TotalTime { get; set; }
    public List<ActivityViewDto> Activities { get; set; }
    public List<EmployeeViewDto> Employees { get; set; }
    public List<ActivityCostDto> MonthlyActivityCosts { get; set; }
    public int ClientId { get; set; }
}