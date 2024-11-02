namespace PlannerCRM.Server.Models.Entities;

public class Salary
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public int EmployeeId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
}
