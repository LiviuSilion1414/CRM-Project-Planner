using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

namespace PlannerCRM.Shared.DTOs.ActivityDto.Forms;

public partial class ActivityForm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }

    public int WorkorderId { get; set; }
    public string SelectedWorkorder { get; set; }
    public string SelectedEmployee { get; set; }
    public EmployeeForm SelectedEmployeeDto { get; set; }
    public List<EmployeeActivityDto> EmployeeActivityDtos { get; set; }
}