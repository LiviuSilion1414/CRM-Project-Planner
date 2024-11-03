using PlannerCRM.Shared.Models.Entities;

namespace PlannerCRM.Shared.Models.JunctionEntities;
public class EmployeeActivityDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public EmployeeDto EmployeeDto { get; set; }
    public ActivityDto ActivityDto { get; set; }
}
