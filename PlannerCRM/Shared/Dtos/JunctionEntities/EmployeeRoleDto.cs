using PlannerCRM.Shared.Dtos.Entities;

namespace PlannerCRM.Shared.Dtos.JunctionEntities;

public class EmployeeRoleDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public int RoleId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public RoleDto Role { get; set; }
}
