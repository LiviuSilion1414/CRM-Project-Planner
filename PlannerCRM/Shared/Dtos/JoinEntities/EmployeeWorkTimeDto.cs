namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeWorkTimeDto
{
    public int Id { get; set; } = 0;
    public int EmployeeId { get; set; } = 0;
    public int WorkTimeId { get; set; } = 0;

    // Navigation properties
    public EmployeeDto Employee { get; set; } = new();
    public WorkTimeDto WorkTime { get; set; } = new();
}
