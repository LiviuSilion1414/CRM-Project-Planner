using PlannerCRM.Server.Models.JunctionEntities;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Models.Entities;

public class Role
{
    public int Id { get; set; }
    public Roles RoleName { get; set; }

    // Navigation properties
    public ICollection<Employee> Employees { get; set; }
    public ICollection<EmployeeRole> EmployeeRoles { get; set; }
}
