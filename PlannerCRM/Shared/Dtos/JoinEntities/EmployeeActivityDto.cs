namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeActivityDto
{
    public int Id { get; set; } = 0;
    public int EmployeeId { get; set; } = 0;
    public int ActivityId { get; set; } = 0;

    // Navigation properties
    public EmployeeDto Employee { get; set; } = new();
    public ActivityDto Activity { get; set; } = new();
}
