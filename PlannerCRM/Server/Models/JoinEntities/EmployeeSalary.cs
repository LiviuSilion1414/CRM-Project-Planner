namespace PlannerCRM.Server.Models.JoinEntities;

public class EmployeeSalary
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int SalaryId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
    public Salary Salary { get; set; }
}
