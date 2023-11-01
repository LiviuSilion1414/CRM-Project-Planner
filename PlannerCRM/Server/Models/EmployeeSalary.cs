namespace PlannerCRM.Server.Models;

public class EmployeeSalary
{
    public string Id { get; set; }
    public string EmployeeId { get; set; }
    public decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
}
