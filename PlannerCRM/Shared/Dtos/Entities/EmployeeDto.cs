namespace PlannerCRM.Shared.Dtos.Entities;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }

    // Navigation properties
    //public List<WorkTimeDto> WorkTimes { get; set; }
    //public List<ActivityDto> Activities { get; set; }
    //public List<RoleDto> Roles { get; set; }
    //public List<SalaryDto> Salaries { get; set; }
    //public List<EmployeeRoleDto> EmployeeRoles { get; set; }
    //public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }
    //public List<EmployeeActivityDto> EmployeeActivities { get; set; }
}
