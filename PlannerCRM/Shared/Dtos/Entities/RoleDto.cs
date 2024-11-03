using PlannerCRM.Shared.Dtos.JunctionEntities;

namespace PlannerCRM.Shared.Dtos.Entities;

public class RoleDto
{
    public int Id { get; set; }
    public Roles RoleName { get; set; }

    // Navigation properties
    public ICollection<EmployeeDto> EmployeesDto { get; set; }
    public ICollection<EmployeeRoleDto> EmployeeRolesDto { get; set; }
}
