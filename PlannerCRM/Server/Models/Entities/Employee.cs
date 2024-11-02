namespace PlannerCRM.Server.Models.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }

    // Navigation properties
    public ICollection<WorkTime> WorkTimes { get; set; } = new List<WorkTime>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<Salary> Salaries { get; set; } = new List<Salary>();
}
