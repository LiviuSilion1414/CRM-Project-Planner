namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeSalaryDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
}