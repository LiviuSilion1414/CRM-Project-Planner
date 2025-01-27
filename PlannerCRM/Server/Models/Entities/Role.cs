using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Models.Entities;

public class Role
{
    public int Id { get; set; }
    public Roles RoleName { get; set; }

    // Navigation properties
    public List<Employee> Employees { get; set; }
    public List<EmployeeRole> EmployeeRoles { get; set; }
}
