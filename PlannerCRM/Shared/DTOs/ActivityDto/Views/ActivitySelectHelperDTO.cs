using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using PlannerCRM.Shared.DTOs.Workorder.Views;

namespace PlannerCRM.Shared.DTOs.ActivityDto.Views;

public class ActivitySelectHelperDto
{   
    [Required]
    public string SelectedWorkorder { get; set; }
    
    [Required]
    public string SelectedEmployee { get; set; }

    [Required]
    public WorkOrderSelectDto WorkorderDto { get; set; }
    
    [Required]
    public EmployeeSelectDto EmployeeDto { get; set; }
}