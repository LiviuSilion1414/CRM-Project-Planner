namespace PlannerCRM.Shared.DTOs.EmployeeDto.Views;

public class EmployeeDeleteDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public List<EmployeeActivityDto> EmployeeActivities { get; set; }
}