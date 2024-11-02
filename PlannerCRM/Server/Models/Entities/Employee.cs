using PlannerCRM.Server.Models.JunctionEntities;

namespace PlannerCRM.Server.Models.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }

    // Navigation properties
    public ICollection<WorkTime> WorkTimes { get; set; }
    public ICollection<Activity> Activities { get; set; }
    public ICollection<Role> Roles { get; set; }
    public ICollection<Salary> Salaries { get; set; }
    public ICollection<EmployeeRole> EmployeeRoles { get; set; }
    public ICollection<EmployeeSalary> EmployeeSalaries { get; set; }
    public ICollection<EmployeeActivity> EmployeeActivities { get; set; }
}
