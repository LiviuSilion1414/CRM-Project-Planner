namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeActivityDto
{
    public Guid  Guid { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid ActivityId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public ActivityDto Activity { get; set; }
}
