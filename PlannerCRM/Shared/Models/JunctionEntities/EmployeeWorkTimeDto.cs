using PlannerCRM.Shared.Models.Entities;

namespace PlannerCRM.Shared.Models.JunctionEntities;

public class EmployeeWorkTimeDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int WorkTimeId { get; set; }

    // Navigation properties
    public EmployeeDto EmployeeDto { get; set; }
    public WorkTimeDto WorkTimeDto { get; set; }
}
