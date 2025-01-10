using PlannerCRM.Shared.Dtos.Entities;

namespace PlannerCRM.Shared.Dtos.JunctionEntities;
public class EmployeeActivityDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public ActivityDto Activity { get; set; }
}
