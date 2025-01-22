namespace PlannerCRM.Shared.Dtos.Entities;

public class SalaryDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }= DateTime.Now;
    public DateTime? EndDate { get; set; }= DateTime.Now;
    public decimal HourlyRate { get; set; }
    public int EmployeeId { get; set; }

    // Navigation properties
    //public EmployeeDto Employee { get; set; }
    //public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }

}
