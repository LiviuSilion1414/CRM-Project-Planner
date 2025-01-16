namespace PlannerCRM.Server.Models.JoinEntities;

public class EmployeeActivity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
    public Activity Activity { get; set; }
}
