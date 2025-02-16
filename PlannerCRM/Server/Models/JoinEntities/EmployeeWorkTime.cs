namespace PlannerCRM.Server.Models.JoinEntities;

public class EmployeeWorkTime
{
    public Guid Guid { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid WorkTimeId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
    public WorkTime WorkTime { get; set; }
}
