namespace PlannerCRM.Server.Models.Entities;

public class Salary
{
    public Guid Guid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HourlyRate { get; set; }

    // Navigation properties
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public List<EmployeeSalary> EmployeeSalaries { get; set; }

}
