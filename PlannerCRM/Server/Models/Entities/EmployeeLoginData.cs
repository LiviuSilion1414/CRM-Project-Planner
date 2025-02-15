namespace PlannerCRM.Server.Models.Entities;

public class EmployeeLoginData
{
    public int Id { get; set; }
    public DateTime LastSeen { get; set; }
    public string Token { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
}
