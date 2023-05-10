using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;

namespace PlannerCRM.Shared.DTOs.ActivityDto.Forms;

public class EmployeeActivityDto
{
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    public EmployeeSelectDto Employee { get; set; }

    public int ActivityId { get; set; }
    public ActivitySelectDto Activity { get; set; }
}
