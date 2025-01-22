namespace PlannerCRM.Shared.Dtos.Entities;

public class RoleDto
{
    public int Id { get; set; } = 0;
    public Roles RoleName { get; set; } = Roles.JUNIOR_DEVELOPER;

    // Navigation properties
    public List<EmployeeDto> Employees { get; set; } = [];
    public List<EmployeeRoleDto> EmployeeRoles { get; set; } = [];
}
