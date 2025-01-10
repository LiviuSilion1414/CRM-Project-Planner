using PlannerCRM.Shared.Dtos.Entities;

namespace PlannerCRM.Shared.Dtos.JunctionEntities;

public class EmployeeWorkTimeDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int WorkTimeId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public WorkTimeDto WorkTime { get; set; }
}
