using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;

namespace PlannerCRM.Shared.DTOs.ActivityDto.Forms;

public class EmployeeActivityDTO
{
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    public EmployeeSelectDTO Employee { get; set; }

    public int ActivityId { get; set; }
    public ActivitySelectDTO Activity { get; set; }
}
