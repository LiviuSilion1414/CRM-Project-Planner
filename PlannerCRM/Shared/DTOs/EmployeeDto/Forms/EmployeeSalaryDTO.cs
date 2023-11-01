namespace PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

public class EmployeeSalaryDto
{
    public string Id { get; set; }
    public string EmployeeId { get; set; }
    public decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
}