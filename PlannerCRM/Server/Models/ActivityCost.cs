namespace PlannerCRM.Server.Models;

public class ActivityCost
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    
    [NotMapped] 
    public List<Employee> Employees { get; set; }
    public decimal MonthlyCost { get; set; }
}