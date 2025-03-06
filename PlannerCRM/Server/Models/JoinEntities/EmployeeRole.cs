namespace PlannerCRM.Server.Models.JoinEntities;

public class EmployeeRole
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }

    public Guid EmployeeId { get; set; }
    public Guid RoleId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
    public Role Role { get; set; }
}
