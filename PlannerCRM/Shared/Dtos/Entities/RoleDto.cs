namespace PlannerCRM.Shared.Dtos.Entities;

public class RoleDto
{
    public Guid  Guid { get; set; }
    public Roles RoleName { get; set; }

    // Navigation properties
    //public List<EmployeeDto> Employees { get; set; }
    //public List<EmployeeRoleDto> EmployeeRoles { get; set; }
}
