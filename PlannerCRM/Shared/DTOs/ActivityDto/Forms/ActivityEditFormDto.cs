using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.Attributes;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.DTOs.ActivityDto.Forms;

public partial class ActivityEditFormDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? FinishDate { get; set; }

    public int? WorkOrderId { get; set; }

    public List<EmployeeActivityDto> EmployeesActivities { get; set; }
}