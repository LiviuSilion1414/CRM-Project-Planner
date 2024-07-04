namespace PlannerCRM.Server.Models;

public class WorkOrderCost
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int WorkOrderId { get; set; }
    public DateTime StartDate { get; set; } 
    public DateTime FinishDate { get; set; }
    public DateTime IssuedDate { get; set; }
    public bool IsCreated { get; init; }
    public int TotalHours { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalActivities { get; set; }
    public decimal CostPerMonth { get; set; }
    public decimal TotalCost { get; set; }
    public TimeSpan TotalTime { get; set; }
    public int ClientId { get; set; }
    
    [NotMapped]
    public List<Activity> Activities { get; set; }

    [NotMapped]
    public List<Employee> Employees { get; set; }

    [NotMapped]
    public List<ActivityCost> MonthlyActivityCosts { get; set; }
}