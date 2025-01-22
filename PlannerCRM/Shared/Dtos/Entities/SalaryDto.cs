namespace PlannerCRM.Shared.Dtos.Entities;

public class SalaryDto
{
    public int Id { get; set; } = 0;
    public DateTime StartDate { get; set; }= DateTime.Now;
    public DateTime? EndDate { get; set; }= DateTime.Now;
    public decimal HourlyRate { get; set; } = 0M;
    public int EmployeeId { get; set; } = 0;

    // Navigation properties
    public EmployeeDto Employee { get; set; } = new();
    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; } = [];
}
