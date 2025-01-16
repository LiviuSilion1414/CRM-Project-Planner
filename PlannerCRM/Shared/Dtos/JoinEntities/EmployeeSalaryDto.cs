namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeSalaryDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int SalaryId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public SalaryDto Salary { get; set; }
}
