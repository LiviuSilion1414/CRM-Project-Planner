namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeWorkTimeDto
{
    public Guid  Guid { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid WorkTimeId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public WorkTimeDto WorkTime { get; set; }
}
