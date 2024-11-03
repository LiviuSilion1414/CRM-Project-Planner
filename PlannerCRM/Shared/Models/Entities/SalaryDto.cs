using PlannerCRM.Shared.Models.JunctionEntities;

namespace PlannerCRM.Shared.Models.Entities;

public class SalaryDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public int EmployeeId { get; set; }

    // Navigation properties
    public EmployeeDto EmployeeDto { get; set; }
    public ICollection<EmployeeSalaryDto> EmployeeSalariesDto { get; set; }

}
