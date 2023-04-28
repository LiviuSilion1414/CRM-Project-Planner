using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;

namespace PlannerCRM.Shared.DTOs.ActivityDto.Forms;

public partial class ActivityForm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }

    public int WorkOrderId { get; set; }
    public List<EmployeeSelectDTO> EmployeesActivities { get; set; } //change to EmployeeActivity dto and change all mapping made to this object
}