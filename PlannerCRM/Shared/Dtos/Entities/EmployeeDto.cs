namespace PlannerCRM.Shared.Dtos.Entities;

public class EmployeeDto
{
    public Guid  Guid { get; set; }

    [Required]
    [MinLength(5, ErrorMessage = "The name should be at least {0} characters")]
    public string Name { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string UserName { get; set; }
    
    [Required]
    [PasswordValidator]
    public string Password { get; set; }

    // Navigation properties
    //public List<WorkTimeDto> WorkTimes { get; set; }
    //public List<ActivityDto> Activities { get; set; }
    //public List<RoleDto> Roles { get; set; }
    //public List<SalaryDto> Salaries { get; set; }
    //public List<EmployeeRoleDto> EmployeeRoles { get; set; }
    //public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }
    //public List<EmployeeActivityDto> EmployeeActivities { get; set; }
}
