using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.DTOs.Workorder.Views;

namespace PlannerCRM.Shared.DTOs.ActivityDto.Views;

public class ActivitySelectHelperDTO
{
    public string SelectedWorkorder { get; set; }
    public string SelectedEmployee { get; set; }

    public WorkorderSelectDTO WorkorderDto { get; set; }
    public EmployeeSelectDTO EmployeeDto { get; set; }
}