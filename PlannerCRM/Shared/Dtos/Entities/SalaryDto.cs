namespace PlannerCRM.Shared.Dtos.Entities;

public class SalaryDto
{
    public int Id { get; set; }
    
    public DateTime StartDate { get; set; }= DateTime.Now;
    public string StartDateString { get => string.Format("{0:dd/MM/yyyy}", StartDate); }
    
    public DateTime? EndDate { get; set; }= DateTime.Now;
    public string EndDateString { get => string.Format("{0:dd/MM/yyyy}", EndDate); }

    public decimal HourlyRate { get; set; }
    public int EmployeeId { get; set; }

    // Navigation properties
    //public EmployeeDto Employee { get; set; }
    //public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }

}
