namespace PlannerCRM.Server.Models.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string Phone { get; set; }
    public bool IsActive { get; set; }
    public bool LockoutEnd { get; set; }

    // Navigation properties
    public List<EmployeeLoginData> LoginData { get; set; }
    public List<WorkTime> WorkTimes { get; set; }
    public List<Activity> Activities { get; set; }
    public List<Role> Roles { get; set; }
    public List<Salary> Salaries { get; set; }
    public List<EmployeeRole> EmployeeRoles { get; set; }
    public List<EmployeeSalary> EmployeeSalaries { get; set; }
    public List<EmployeeActivity> EmployeeActivities { get; set; }
}
