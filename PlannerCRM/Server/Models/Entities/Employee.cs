namespace PlannerCRM.Server.Models.Entities;

public class Employee : IdentityUser<int>
{
    public string Name { get; set; }

    // Navigation properties
    public List<WorkTime> WorkTimes { get; set; }
    public List<Activity> Activities { get; set; }
    public List<Role> Roles { get; set; }
    public List<Salary> Salaries { get; set; }
    public List<EmployeeRole> EmployeeRoles { get; set; }
    public List<EmployeeSalary> EmployeeSalaries { get; set; }
    public List<EmployeeActivity> EmployeeActivities { get; set; }
}
