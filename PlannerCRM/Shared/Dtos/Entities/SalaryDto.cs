namespace PlannerCRM.Shared.Dtos.Entities;

public class SalaryDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public int EmployeeId { get; set; }

    // Navigation properties
    //public EmployeeDto Employee { get; set; }
    //public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }

}
