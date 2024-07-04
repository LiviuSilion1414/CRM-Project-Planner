namespace PlannerCRM.Server.Models;

public class EmployeeActivity
{
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    
    [NotMapped]
    public Employee Employee { get; set; }

    public int ActivityId { get; set; }
    
    [NotMapped]
    public Activity Activity { get; set; }
}