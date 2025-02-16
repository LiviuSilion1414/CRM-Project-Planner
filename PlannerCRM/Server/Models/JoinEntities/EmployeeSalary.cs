namespace PlannerCRM.Server.Models.JoinEntities;

public class EmployeeSalary
{
    public Guid Guid { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid SalaryId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
    public Salary Salary { get; set; }
}
