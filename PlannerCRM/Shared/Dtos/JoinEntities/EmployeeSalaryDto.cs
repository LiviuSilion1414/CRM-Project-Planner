namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeSalaryDto
{
    public int Id { get; set; } = 0;
    public int EmployeeId { get; set; } = 0;
    public int SalaryId { get; set; } = 0;

    // Navigation properties
    public EmployeeDto Employee { get; set; } = new();
    public SalaryDto Salary { get; set; } = new();
}
