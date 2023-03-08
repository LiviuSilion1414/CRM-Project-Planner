namespace PlannerCRM.Server.Models;

public class Activity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    
    public List<EmployeeActivity> EmployeeActivity { get; set; }
    public int WorkOrderId { get; set; }
}