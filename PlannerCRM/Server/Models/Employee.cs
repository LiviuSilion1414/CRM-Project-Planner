namespace PlannerCRM.Server.Models;

public class Employee : IdentityUser<int>
{
    public int ProfilePictureId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime BirthDay { get; set; }
    public string NumericCode { get; set; }
    public decimal CurrentHourlyRate { get; set; }
    public Roles Role { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsArchived { get; set; }

    [NotMapped]
    public List<WorkTimeRecord> WorkTimeRecords { get; set; }

    [NotMapped]
    public List<EmployeeSalary> Salaries { get; set; }

    [NotMapped]
    public List<EmployeeActivity> EmployeeActivity { get; set; }
}