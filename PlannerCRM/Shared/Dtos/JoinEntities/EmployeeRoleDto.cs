namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeRoleDto
{
    public int Id { get; set; } = 0;

    public int EmployeeId { get; set; } = 0;
    public int RoleId { get; set; } = 0;

    // Navigation properties
    public EmployeeDto Employee { get; set; } = new();
    public RoleDto Role { get; set; } = new();
}
