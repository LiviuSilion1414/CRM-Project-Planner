namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeRoleDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public int RoleId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public RoleDto Role { get; set; }
}
