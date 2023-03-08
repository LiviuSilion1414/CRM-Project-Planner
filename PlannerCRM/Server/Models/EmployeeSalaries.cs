namespace PlannerCRM.Server.Models;

public class EmployeeSalaries
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
}
