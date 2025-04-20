using PlannerCRM.Shared.Dtos;

namespace PlannerCRM.Shared.Dtos;

public class EmployeeActivityDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid ActivityId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public ActivityDto Activity { get; set; }
}
