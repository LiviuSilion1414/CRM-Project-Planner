using PlannerCRM.Shared.Models.Entities;

namespace PlannerCRM.Shared.Models.JunctionEntities;

public class EmployeeRoleDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public int RoleId { get; set; }

    // Navigation properties
    public EmployeeDto EmployeeDto { get; set; }
    public RoleDto RoleDto { get; set; }
}
