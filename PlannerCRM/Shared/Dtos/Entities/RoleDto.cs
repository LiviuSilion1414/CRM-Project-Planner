using PlannerCRM.Shared.Dtos.JunctionEntities;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Shared.Dtos.Entities;

public class RoleDto
{
    public int Id { get; set; }
    public Roles RoleName { get; set; }

    // Navigation properties
    //public List<EmployeeDto> Employees { get; set; }
    //public List<EmployeeRoleDto> EmployeeRoles { get; set; }
}
