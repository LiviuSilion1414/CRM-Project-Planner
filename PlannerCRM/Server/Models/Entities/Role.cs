namespace PlannerCRM.Server.Models.Entities;

public class Role
{
    public int Id { get; set; }
    public Roles RoleName { get; set; }

    // Navigation properties
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
