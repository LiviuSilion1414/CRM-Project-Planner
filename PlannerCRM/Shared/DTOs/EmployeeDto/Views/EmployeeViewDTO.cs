using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Views;

public class EmployeeViewDto
{ 
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public decimal HourlyRate { get; set; }
    public DateTime BirthDay { get; set; }
    public DateTime StartDate { get; set; }
    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }
    public List<EmployeeActivityDto> EmployeeActivities { get; set; }
}
