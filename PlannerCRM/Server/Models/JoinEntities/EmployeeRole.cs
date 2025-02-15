namespace PlannerCRM.Server.Models.JoinEntities;

public class EmployeeRole
{
    public int Id { get; set; }
    public string RoleName { get; set; }

    public int EmployeeId { get; set; }
    public int RoleId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
    public Role Role { get; set; }
}
