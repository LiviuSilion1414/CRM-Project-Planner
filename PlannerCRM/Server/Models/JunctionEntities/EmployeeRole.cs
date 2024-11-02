namespace PlannerCRM.Server.Models.JunctionEntities;

using PlannerCRM.Server.Models.Entities;

public class EmployeeRole : IdentityRole<int>
{
    public int EmployeeId { get; set; }
    public int RoleId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
    public Role Role { get; set; }
}
