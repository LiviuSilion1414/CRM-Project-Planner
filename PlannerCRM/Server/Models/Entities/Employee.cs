namespace PlannerCRM.Server.Models.Entities;

public class Employee
{
    public Guid Guid { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string Phone { get; set; }
    public bool IsActive { get; set; }
    public bool LockoutEnd { get; set; }
    public DateTime LastSeen { get; set; }

    // Navigation properties
    public List<WorkTime> WorkTimes { get; set; }
    public List<Activity> Activities { get; set; }
    public List<Salary> Salaries { get; set; }
    public List<EmployeeRole> EmployeeRoles { get; set; }
    public List<EmployeeSalary> EmployeeSalaries { get; set; }
    public List<EmployeeActivity> EmployeeActivities { get; set; }
}
