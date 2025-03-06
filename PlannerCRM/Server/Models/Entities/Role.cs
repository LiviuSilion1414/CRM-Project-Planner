using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Models.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }

    // Navigation properties
    public List<EmployeeRole> EmployeeRoles { get; set; }
}
