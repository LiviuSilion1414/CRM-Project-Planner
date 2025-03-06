namespace PlannerCRM.Server.Models.JoinEntities;

public class EmployeeActivity
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid ActivityId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
    public Activity Activity { get; set; }
}
