namespace PlannerCRM.Server.Models.Entities;

public class EmployeeLoginData
{
    public Guid Guid { get; set; }
    public DateTime LastSeen { get; set; }
    public string Token { get; set; }

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
}
