using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

namespace PlannerCRM.Shared.DTOs.ActivityDto.Forms;

public class EmployeeActivityDto
{
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    public EmployeeForm Employee { get; set; }

    public int ActivityId { get; set; }
    public ActivityForm Activity { get; set; }
}
