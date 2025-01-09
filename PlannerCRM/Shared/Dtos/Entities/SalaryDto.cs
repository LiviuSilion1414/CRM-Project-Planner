using PlannerCRM.Shared.Dtos.JunctionEntities;

namespace PlannerCRM.Shared.Dtos.Entities;

public class SalaryDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public int EmployeeId { get; set; }

    // Navigation properties
    public EmployeeDto EmployeeDto { get; set; }
    public List<EmployeeSalaryDto> EmployeeSalariesDto { get; set; }

}
