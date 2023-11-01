namespace PlannerCRM.Server.Models;

public class EmployeeRole : IdentityRole
{
    public new int Id { get; set; } 
    public new string Name { get; set; }
}