namespace PlannerCRM.Server.Models.JunctionEntities;

using PlannerCRM.Server.Models.Entities;

public class EmployeeWorkTime
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int WorkTimeId { get; set; }

    // Navigation properties
    public Employee Employee { get; set; }
    public WorkTime WorkTime { get; set; }
}
