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
    public ICollection<WorkTimeDto> WorkTimesDto { get; set; }
    public ICollection<ActivityDto> ActivitiesDto { get; set; }
    public ICollection<RoleDto> RolesDto { get; set; }
    public ICollection<SalaryDto> SalariesDto { get; set; }
    public ICollection<EmployeeRoleDto> EmployeeRolesDto { get; set; }
    public ICollection<EmployeeSalaryDto> EmployeeSalariesDto { get; set; }
    public ICollection<EmployeeActivityDto> EmployeeActivitiesDto { get; set; }
}
