using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Shared.DTOs.EmployeeDto.Views;

public class EmployeeSelectDto
{
    public int Id { get; set; }
    public string Email { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public Roles Role { get; set; }
    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }
    public List<EmployeeActivityDto> EmployeeActivities { get; set; }
}