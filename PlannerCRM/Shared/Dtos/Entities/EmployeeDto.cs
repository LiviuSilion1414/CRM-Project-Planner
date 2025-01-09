using PlannerCRM.Shared.Dtos.JunctionEntities;

namespace PlannerCRM.Shared.Dtos.Entities;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }

    // Navigation properties
    public List<WorkTimeDto> WorkTimesDto { get; set; }
    public List<ActivityDto> ActivitiesDto { get; set; }
    public List<RoleDto> RolesDto { get; set; }
    public List<SalaryDto> SalariesDto { get; set; }
    public List<EmployeeRoleDto> EmployeeRolesDto { get; set; }
    public List<EmployeeSalaryDto> EmployeeSalariesDto { get; set; }
    public List<EmployeeActivityDto> EmployeeActivitiesDto { get; set; }
}
