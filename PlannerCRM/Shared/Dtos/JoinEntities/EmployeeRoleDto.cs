namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeRoleDto
{
    public Guid  Guid { get; set; }

    public Guid EmployeeId { get; set; }
    public Guid RoleId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public RoleDto Role { get; set; }
}
