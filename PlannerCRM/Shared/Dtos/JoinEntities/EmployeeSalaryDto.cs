namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeSalaryDto
{
    public Guid  Guid { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid SalaryId { get; set; }

    // Navigation properties
    public EmployeeDto Employee { get; set; }
    public SalaryDto Salary { get; set; }
}
