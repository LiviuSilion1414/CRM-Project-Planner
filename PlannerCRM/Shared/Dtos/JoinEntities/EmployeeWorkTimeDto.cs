namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeWorkTimeDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int WorkTimeId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public WorkTimeDto WorkTime { get; set; }
}
