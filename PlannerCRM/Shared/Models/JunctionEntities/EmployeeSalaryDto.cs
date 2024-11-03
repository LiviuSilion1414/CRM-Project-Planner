using PlannerCRM.Shared.Models.Entities;

namespace PlannerCRM.Shared.Models.JunctionEntities;

public class EmployeeSalaryDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int SalaryId { get; set; }

    // Navigation properties
    public EmployeeDto EmployeeDto { get; set; }
    public SalaryDto SalaryDto { get; set; }
}
