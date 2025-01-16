namespace PlannerCRM.Server.Models.Entities;

public class Employee : IdentityUser<int>
{
    public string Name { get; set; }

    // Navigation properties
    public ICollection<WorkTime> WorkTimes { get; set; }
    public ICollection<Activity> Activities { get; set; }
    public ICollection<Role> Roles { get; set; }
    public ICollection<Salary> Salaries { get; set; }
    public ICollection<EmployeeRole> EmployeeRoles { get; set; }
    public ICollection<EmployeeSalary> EmployeeSalaries { get; set; }
    public ICollection<EmployeeActivity> EmployeeActivities { get; set; }
}
